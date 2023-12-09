using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using NATechApi;
using NATechApi.Models;
using NATechProxy;

namespace NATechDriver;

public class NATechDriver : ChromeDriver
{
	private bool _headless = false;

	private ChromeOptions _options = null;

	private ChromeDriverService _service = null;

	private Process _browser = null;

	private bool _keepUserDataDir = true;

	private string _userDataDir = null;

	private NATechDriver(ChromeDriverService service, ChromeOptions options)
		: base(service, options)
	{
		_options = options;
		_service = service;
	}

	public static NATechDriver Create(ChromeOptions options = null, string userDataDir = null, string driverExecutablePath = null, string browserExecutablePath = null, int logLevel = 3, bool headless = false, bool suppressWelcome = true, Dictionary<string, object> prefs = null)
	{
		Patcher patcher = new Patcher(driverExecutablePath);
		patcher.Auto();
		if (options == null)
		{
			options = new ChromeOptions();
		}
		string text = "127.0.0.1";
		int num = findFreePort();
		if (options.DebuggerAddress == null)
		{
			options.DebuggerAddress = $"{text}:{num}";
		}
		options.AddArgument("--remote-debugging-host=" + text);
		options.AddArgument($"--remote-debugging-port={num}");
		bool keepUserDataDir = true;
		if (string.IsNullOrEmpty(userDataDir))
		{
			keepUserDataDir = false;
			userDataDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
		}
		options.AddArgument("--user-data-dir=" + userDataDir);
		string name = CultureInfo.CurrentCulture.Name;
		options.AddArgument("--lang=" + name);
		if (browserExecutablePath == null)
		{
			browserExecutablePath = findChromeExecutable();
			if (browserExecutablePath == null)
			{
				throw new Exception("Not found chrome.exe.");
			}
		}
		options.BinaryLocation = browserExecutablePath;
		if (suppressWelcome)
		{
			options.AddArguments("--no-default-browser-check", "--no-first-run");
		}
		if (headless)
		{
			options.AddArguments("--headless");
			options.AddArguments("--window-size=1920,1080");
			options.AddArguments("--start-maximized");
			options.AddArguments("--no-sandbox");
		}
		options.AddArguments($"--log-level={logLevel}");
		if (prefs != null)
		{
			handlePrefs(userDataDir, prefs);
		}
		try
		{
			string path = Path.Combine(userDataDir, "Default\\Preferences");
			string input = File.ReadAllText(path, Encoding.GetEncoding("ISO-8859-1"));
			Regex regex = new Regex("(?<=exit_type\":)(.*?)(?=,)");
			string value = regex.Match(input).Value;
			if (value != "" && value != "null")
			{
				input = regex.Replace(input, "null");
				File.WriteAllText(path, input, Encoding.GetEncoding("ISO-8859-1"));
			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
		string arguments = options.Arguments.Select((string it) => it.Trim()).Aggregate("", (string r, string it) => r + " " + (it.Contains(" ") ? ("\"" + it + "\"") : it));
		ProcessStartInfo processStartInfo = new ProcessStartInfo(options.BinaryLocation, arguments);
		processStartInfo.UseShellExecute = false;
		processStartInfo.RedirectStandardInput = true;
		processStartInfo.RedirectStandardOutput = true;
		processStartInfo.RedirectStandardError = true;
		Process browser = Process.Start(processStartInfo);
		if (driverExecutablePath == null)
		{
			throw new Exception("driverExecutablePath is required.");
		}
		ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(driverExecutablePath), Path.GetFileName(driverExecutablePath));
		chromeDriverService.HideCommandPromptWindow = true;
		NATechDriver NATechDriver = new NATechDriver(chromeDriverService, options);
		NATechDriver._headless = headless;
		NATechDriver._options = options;
		NATechDriver._service = chromeDriverService;
		NATechDriver._browser = browser;
		NATechDriver._keepUserDataDir = keepUserDataDir;
		NATechDriver._userDataDir = userDataDir;
		return NATechDriver;
	}

	public void GotoUrl(string url)
	{
		if (_headless)
		{
			ConfigureHeadless();
		}
		if (HasCdcProps())
		{
			HookCdcProps();
		}
		Navigate().GoToUrl(url);
	}

	private void ConfigureHeadless()
	{
		if (ExecuteScript("return navigator.webdriver") != null)
		{
			ExecuteCdpCommand("Page.addScriptToEvaluateOnNewDocument", new Dictionary<string, object> { ["source"] = "\r\n                            Object.defineProperty(window, 'navigator', {\r\n                                value: new Proxy(navigator, {\r\n                                        has: (target, key) => (key === 'webdriver' ? false : key in target),\r\n                                        get: (target, key) =>\r\n                                                key === 'webdriver' ?\r\n                                                false :\r\n                                                typeof target[key] === 'function' ?\r\n                                                target[key].bind(target) :\r\n                                                target[key]\r\n                                        })\r\n                            });\r\n                         " });
			ExecuteCdpCommand("Network.setUserAgentOverride", new Dictionary<string, object> { ["userAgent"] = ((string)ExecuteScript("return navigator.userAgent")).Replace("Headless", "") });
			ExecuteCdpCommand("Page.addScriptToEvaluateOnNewDocument", new Dictionary<string, object> { ["source"] = "\r\n                            Object.defineProperty(navigator, 'maxTouchPoints', {\r\n                                    get: () => 1\r\n                            });\r\n                         " });
		}
	}

	private bool HasCdcProps()
	{
		ReadOnlyCollection<object> readOnlyCollection = (ReadOnlyCollection<object>)ExecuteScript("\r\n                    let objectToInspect = window,\r\n                        result = [];\r\n                    while(objectToInspect !== null)\r\n                    { result = result.concat(Object.getOwnPropertyNames(objectToInspect));\r\n                      objectToInspect = Object.getPrototypeOf(objectToInspect); }\r\n                    return result.filter(i => i.match(/.+_.+_(Array|Promise|Symbol)/ig))\r\n                 ");
		return readOnlyCollection.Count > 0;
	}

	private void HookCdcProps()
	{
		ExecuteCdpCommand("Page.addScriptToEvaluateOnNewDocument", new Dictionary<string, object> { ["source"] = "\r\n                        let objectToInspect = window,\r\n                            result = [];\r\n                        while(objectToInspect !== null) \r\n                        { result = result.concat(Object.getOwnPropertyNames(objectToInspect));\r\n                          objectToInspect = Object.getPrototypeOf(objectToInspect); }\r\n                        result.forEach(p => p.match(/.+_.+_(Array|Promise|Symbol)/ig)\r\n                                            &&delete window[p]&&console.log('removed',p))\r\n                     " });
	}

	private static string findChromeExecutable()
	{
		List<string> list = new List<string>();
		string[] array = new string[3] { "Program Files", "Program Files (X86)", "LOCALAPPDATA" };
		foreach (string variable in array)
		{
			string[] array2 = new string[3] { "Google\\Chrome\\Application", "Google\\Chrome Beta\\Application", "Google\\Chrome Canary\\Application" };
			foreach (string path in array2)
			{
				string environmentVariable = Environment.GetEnvironmentVariable(variable);
				if (environmentVariable != null)
				{
					list.Add(Path.Combine(environmentVariable, path, "chrome.exe"));
				}
			}
		}
		foreach (string item in list)
		{
			if (File.Exists(item))
			{
				return item;
			}
		}
		return null;
	}

	private static int findFreePort()
	{
		int result = 0;
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		try
		{
			IPEndPoint localEP = new IPEndPoint(IPAddress.Any, 0);
			socket.Bind(localEP);
			localEP = (IPEndPoint)socket.LocalEndPoint;
			result = localEP.Port;
		}
		finally
		{
			socket.Close();
		}
		return result;
	}

	public static void CreateProfile(string ProfileDefaultPath, string ProfileSavePath, ref string ProfileName, string UA, ProxyNATech proxy, string BrowserLanguage, ref string sErr)
	{
		try
		{
			sErr = string.Empty;
			if (!Directory.Exists(ProfileDefaultPath))
			{
				sErr = "Thư mục Profile mẫu [" + ProfileDefaultPath + "] không tồn tại";
				return;
			}
			if (!Directory.Exists(ProfileSavePath))
			{
				Directory.CreateDirectory(ProfileSavePath);
			}
			if (string.IsNullOrEmpty(ProfileName))
			{
				ProfileName = "NATech_" + GenId(6);
			}
			string text = ProfileSavePath + "\\" + ProfileName;
			while (Directory.Exists(text))
			{
				ProfileName = "NATech_" + GenId(6);
				text = ProfileSavePath + "\\" + ProfileName;
			}
			GetProfileRequest strRequestData = new GetProfileRequest(ProfileName, UA, proxy, BrowserLanguage);
			string value = new ApiClientNATech().PostApi("Admin/LoadProfile", strRequestData, ApiClientNATech.loginResponse.Token, out sErr);
			if (string.IsNullOrEmpty(sErr))
			{
				string contents = new Sec().Decrypt(JsonConvert.DeserializeObject<string>(value));
				DirectoryCopy(ProfileDefaultPath, text);
				File.WriteAllText(text + "\\Default\\Preferences", contents);
			}
		}
		catch (Exception ex)
		{
			sErr = ex.Message;
		}
	}

	public static void DirectoryCopy(string sourceDir, string destDir)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(sourceDir);
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		if (!directoryInfo.Exists)
		{
			throw new DirectoryNotFoundException("Không tìm thấy thư mục: " + sourceDir);
		}
		if (!Directory.Exists(destDir))
		{
			Directory.CreateDirectory(destDir);
		}
		FileInfo[] files = directoryInfo.GetFiles();
		FileInfo[] array = files;
		FileInfo[] array2 = array;
		foreach (FileInfo fileInfo in array2)
		{
			string destFileName = Path.Combine(destDir, fileInfo.Name);
			try
			{
				fileInfo.CopyTo(destFileName, overwrite: true);
			}
			catch
			{
			}
		}
		DirectoryInfo[] array3 = directories;
		DirectoryInfo[] array4 = array3;
		foreach (DirectoryInfo directoryInfo2 in array4)
		{
			string destDir2 = Path.Combine(destDir, directoryInfo2.Name);
			DirectoryCopy(directoryInfo2.FullName, destDir2);
		}
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		try
		{
			_browser.Kill();
		}
		catch (Exception)
		{
		}
		if (_keepUserDataDir)
		{
			return;
		}
		for (int i = 0; i < 5; i++)
		{
			try
			{
				Directory.Delete(_userDataDir, recursive: true);
				break;
			}
			catch (Exception)
			{
				Thread.Sleep(100);
			}
		}
	}

	private static void handlePrefs(string userDataDir, Dictionary<string, object> prefs)
	{
		string text = Path.Combine(userDataDir, "Default");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		string path = Path.Combine(text, "Preferences");
		if (File.Exists(path))
		{
			using FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
			using StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("ISO-8859-1"));
			try
			{
				string data = streamReader.ReadToEnd();
				dictionary = Json.DeserializeData(data);
			}
			catch (Exception)
			{
			}
		}
		try
		{
			foreach (KeyValuePair<string, object> pref in prefs)
			{
				undotMerge(pref.Key, pref.Value, dictionary);
			}
		}
		catch (Exception)
		{
			throw new Exception("Prefs merge faild.");
		}
		using (FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
		{
			using StreamWriter streamWriter = new StreamWriter(stream2, Encoding.GetEncoding("ISO-8859-1"));
			string value2 = JsonConvert.SerializeObject(dictionary);
			streamWriter.Write(value2);
		}
		static void undotMerge(string key, object value, Dictionary<string, object> dict)
		{
			if (key.Contains("."))
			{
				string[] array = key.Split(new char[1] { '.' }, 2);
				string key2 = array[0];
				string key3 = array[1];
				if (!dict.ContainsKey(key2))
				{
					dict[key2] = new Dictionary<string, object>();
				}
				undotMerge(key3, value, dict[key2] as Dictionary<string, object>);
			}
			else
			{
				dict[key] = value;
			}
		}
	}

	public static string GetRandomOldProfileFolderPath(Random r, string DataSourceXmlPath)
	{
		if (!File.Exists(DataSourceXmlPath))
		{
			return string.Empty;
		}
		DataTable dataTable = new DataTable();
		dataTable.ReadXml(DataSourceXmlPath);
		if (dataTable.Rows.Count == 0)
		{
			return string.Empty;
		}
		return ToString(dataTable.Rows[r.Next(dataTable.Rows.Count)]["ProfilePath"]);
	}

	public static string ToString(object o)
	{
		if (o == null)
		{
			return string.Empty;
		}
		return o.ToString();
	}

	public static string GenId(int len)
	{
		return Guid.NewGuid().ToString().Replace("-", "")
			.Substring(0, len);
	}
}
