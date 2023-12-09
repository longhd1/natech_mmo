using System;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NATechProxy;

public class bllOrbita
{
	public class OptionBrowser
	{
		public string sourceDirectory = string.Empty;

		public string targetDirectory = string.Empty;

		public string UA = string.Empty;

		public ProxyItems proxy = null;

		public bool isHiddenImage = false;

		public bool hiddenBrowser = false;

		public bool savePass = false;
	}

	private Random r = new Random();

	public static string BinaryLocation = string.Empty;

	public ChromeDriver GetChromeDriver(ChromeDriver driver, OptionBrowser option)
	{
		try
		{
			if (string.IsNullOrEmpty(BinaryLocation) || !File.Exists(BinaryLocation))
			{
				throw new Exception("Tham số BinaryLocation không tồn tại: " + BinaryLocation);
			}
			if (driver == null)
			{
				ChromeOptions chromeOptions = new ChromeOptions();
				ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
				chromeDriverService.HideCommandPromptWindow = true;
				chromeOptions.BinaryLocation = BinaryLocation;
				chromeOptions.AddArguments("start-maximized");
				chromeOptions.AddExcludedArgument("enable-automation");
				if (option.isHiddenImage)
				{
					chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
				}
				if (option.sourceDirectory.Length > 0 && option.targetDirectory.Length > 0)
				{
					if (Directory.Exists(option.targetDirectory))
					{
						try
						{
							Directory.Delete(option.targetDirectory, recursive: true);
						}
						catch (Exception)
						{
						}
					}
					DirectoryInfo source = new DirectoryInfo(option.sourceDirectory);
					DirectoryInfo directoryInfo = new DirectoryInfo(option.targetDirectory);
					CopyFilesRecursively(source, directoryInfo);
					string path = $"{directoryInfo}\\Default\\Preferences";
					string value = File.ReadAllText(path);
					dynamic val = JsonConvert.DeserializeObject(value);
					if (option.UA.Length > 0)
					{
						val["gologin"]["userAgent"] = option.UA;
					}
					if (option.proxy != null && !string.IsNullOrEmpty(option.proxy.userName) && !string.IsNullOrEmpty(option.proxy.passWord))
					{
						val["gologin"]["proxy"]["username"] = option.proxy.userName;
						val["gologin"]["proxy"]["password"] = option.proxy.passWord;
					}
					if (!option.savePass)
					{
						val["credentials_enable_service"] = false;
					}
					string text = JsonConvert.SerializeObject(val, Formatting.Indented);
					File.WriteAllText(path, File.ReadAllText("D:\\NATechSeo_GoLogin\\AutoClickAds\\bin\\Debug\\browser\\Preferences"));
					chromeOptions.AddArguments("user-data-dir=" + option.targetDirectory);
				}
				chromeOptions.AddArguments("lang=en-US");
				chromeOptions.AddArguments("disable-encryption");
				chromeOptions.AddArguments("restore-last-session");
				chromeOptions.AddArguments("font-masking-mode=2");
				if (option.proxy != null && !string.IsNullOrEmpty(option.proxy.ip) && !string.IsNullOrEmpty(option.proxy.port))
				{
					if (option.proxy.proxyType == bllProxy.TypeProxy.Socks5)
					{
						chromeOptions.AddArguments("proxy-server=socks5://" + option.proxy.ip + ":" + option.proxy.port);
						chromeOptions.AddArguments("host-resolver-rules=\"MAP * 0.0.0.0, EXCLUDE " + option.proxy.ip + "\"");
					}
					else
					{
						chromeOptions.AddArguments("proxy-server=" + option.proxy.ip + ":" + option.proxy.port);
						chromeOptions.AddArguments("host-resolver-rules=\"MAP * 0.0.0.0, EXCLUDE " + option.proxy.ip + "\"");
					}
				}
				chromeOptions.AddArguments("flag-switches-begin");
				chromeOptions.AddArguments("flag-switches-end");
				driver = new ChromeDriver(chromeDriverService, chromeOptions);
				if (option.hiddenBrowser)
				{
					driver.Manage().Window.Minimize();
				}
				else
				{
					driver.Manage().Window.Maximize();
				}
			}
			return driver;
		}
		catch (Exception ex2)
		{
			throw ex2;
		}
	}

	public ChromeDriver GetChromeDriverExist(ChromeDriver driver, OptionBrowser option)
	{
		try
		{
			if (string.IsNullOrEmpty(BinaryLocation) || !File.Exists(BinaryLocation))
			{
				throw new Exception("Tham số BinaryLocation không tồn tại: " + BinaryLocation);
			}
			if (driver == null)
			{
				ChromeOptions chromeOptions = new ChromeOptions();
				ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
				chromeDriverService.HideCommandPromptWindow = true;
				chromeOptions.BinaryLocation = BinaryLocation;
				chromeOptions.AddArguments("start-maximized");
				chromeOptions.AddExcludedArgument("enable-automation");
				if (option.isHiddenImage)
				{
					chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
				}
				if (!string.IsNullOrEmpty(option.targetDirectory))
				{
					DirectoryInfo arg = new DirectoryInfo(option.targetDirectory);
					string path = $"{arg}\\Default\\Preferences";
					string value = File.ReadAllText(path);
					dynamic val = JsonConvert.DeserializeObject(value);
					if (option.proxy != null && !string.IsNullOrEmpty(option.proxy.userName) && !string.IsNullOrEmpty(option.proxy.passWord))
					{
						val["gologin"]["proxy"]["username"] = option.proxy.userName;
						val["gologin"]["proxy"]["password"] = option.proxy.passWord;
					}
					if (!option.savePass)
					{
						val["credentials_enable_service"] = false;
					}
					string contents = JsonConvert.SerializeObject(val, Formatting.Indented);
					File.WriteAllText(path, contents);
					chromeOptions.AddArguments("user-data-dir=" + option.targetDirectory);
				}
				chromeOptions.AddArguments("lang=en-US");
				chromeOptions.AddArguments("disable-encryption");
				chromeOptions.AddArguments("restore-last-session");
				chromeOptions.AddArguments("font-masking-mode=2");
				if (option.proxy != null && !string.IsNullOrEmpty(option.proxy.ip) && !string.IsNullOrEmpty(option.proxy.port))
				{
					if (option.proxy.proxyType == bllProxy.TypeProxy.Socks5)
					{
						chromeOptions.AddArguments("proxy-server=socks5://" + option.proxy.ip + ":" + option.proxy.port);
						chromeOptions.AddArguments("host-resolver-rules=\"MAP * 0.0.0.0, EXCLUDE " + option.proxy.ip + "\"");
					}
					else
					{
						chromeOptions.AddArguments("proxy-server=" + option.proxy.ip + ":" + option.proxy.port);
						chromeOptions.AddArguments("host-resolver-rules=\"MAP * 0.0.0.0, EXCLUDE " + option.proxy.ip + "\"");
					}
				}
				chromeOptions.AddArguments("flag-switches-begin");
				chromeOptions.AddArguments("flag-switches-end");
				driver = new ChromeDriver(chromeDriverService, chromeOptions);
				if (option.hiddenBrowser)
				{
					driver.Manage().Window.Minimize();
				}
				else
				{
					driver.Manage().Window.Maximize();
				}
			}
			return driver;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private decimal SaiLechThongSo(decimal vl)
	{
		double num = randomDouble(r, 0.001, 0.02);
		decimal num2 = (decimal)num * vl;
		return vl + num2;
	}

	public void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
	{
		DirectoryInfo[] directories = source.GetDirectories();
		foreach (DirectoryInfo directoryInfo in directories)
		{
			CopyFilesRecursively(directoryInfo, target.CreateSubdirectory(directoryInfo.Name));
		}
		FileInfo[] files = source.GetFiles();
		foreach (FileInfo fileInfo in files)
		{
			fileInfo.CopyTo(Path.Combine(target.FullName, fileInfo.Name), overwrite: true);
		}
	}

	public double randomDouble(Random rand, double start, double end)
	{
		if (end < start)
		{
			end = start;
		}
		return rand.NextDouble() * Math.Abs(end - start) + start;
	}

	public IWebElement FindelementByClass(ChromeDriver dri, string cl)
	{
		try
		{
			return dri.FindElement(By.ClassName(cl));
		}
		catch (Exception)
		{
			return null;
		}
	}

	public IWebElement FindelementByID(ChromeDriver dri, string id)
	{
		try
		{
			return dri.FindElement(By.Id(id));
		}
		catch (Exception)
		{
			return null;
		}
	}

	public IWebElement ElementFindElementBy(IWebElement elementSource, By by)
	{
		IWebElement result = null;
		try
		{
			result = elementSource.FindElement(by);
		}
		catch (Exception)
		{
		}
		return result;
	}

	public IWebElement FindElementByCssSelector(ChromeDriver dri, string css)
	{
		try
		{
			return dri.FindElement(By.CssSelector(css));
		}
		catch (Exception)
		{
			return null;
		}
	}

	public ReadOnlyCollection<IWebElement> FindElementsBy(ChromeDriver driver, By b)
	{
		ReadOnlyCollection<IWebElement> result = null;
		try
		{
			result = driver.FindElements(b);
		}
		catch (Exception)
		{
		}
		return result;
	}

	public IWebElement FindElementBy(ChromeDriver driver, By b)
	{
		IWebElement result = null;
		try
		{
			result = driver.FindElement(b);
		}
		catch (Exception)
		{
		}
		return result;
	}

	public ReadOnlyCollection<IWebElement> ElementFindElementsBy(IWebElement elm, By b)
	{
		ReadOnlyCollection<IWebElement> result = null;
		try
		{
			result = elm.FindElements(b);
		}
		catch (Exception)
		{
		}
		return result;
	}

	public IWebElement FindelementByName(ChromeDriver dri, string name)
	{
		try
		{
			return dri.FindElement(By.Name(name));
		}
		catch (Exception)
		{
			return null;
		}
	}

	public ChromeDriver InitDriverFireFox(ProxyItems proxy, string agent, string targetDirectory, string ExecutablePath)
	{
		try
		{
			ChromeDriver chromeDriver = null;
			if (chromeDriver == null)
			{
				OptionBrowser optionBrowser = new OptionBrowser();
				optionBrowser.sourceDirectory = Path.GetDirectoryName(ExecutablePath).TrimEnd('\\') + string.Format("\\browser\\profileDefault\\{0}", "1");
				optionBrowser.targetDirectory = targetDirectory;
				optionBrowser.UA = agent;
				optionBrowser.proxy = proxy;
				chromeDriver = GetChromeDriver(null, optionBrowser);
			}
			return chromeDriver;
		}
		catch (Exception)
		{
			return null;
		}
	}
}
