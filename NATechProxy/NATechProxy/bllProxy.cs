using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using NATechProxy.Class;

namespace NATechProxy;

public class bllProxy
{
	public enum TypeIp
	{
		IPv4 = 1,
		IPv6
	}

	public enum ProxyPort
	{
		HttpIPv4 = 4000,
		SocksIPv4 = 5000,
		HttpIPv6 = 6000,
		SocksIPv6 = 7000
	}

	public enum TypeProxy
	{
		Http = 1,
		Socks5
	}

	[DllImport("wininet.dll")]
	private static extern bool InternetGetConnectedState(out int Description, int ReservedValue);

	public List<ProxyItems> GetFreeProxy(string memProxy)
	{
		string[] array = Misc.Split(memProxy, "\r\n");
		List<ProxyItems> list = new List<ProxyItems>();
		string[] array2 = array;
		foreach (string s in array2)
		{
			string[] array3 = Misc.Split(s, ":");
			if (array3.Length >= 2)
			{
				ProxyItems proxyItems = new ProxyItems();
				proxyItems.ip = array3[0];
				proxyItems.port = array3[1];
				if (array3.Length >= 4)
				{
					proxyItems.userName = array3[2];
					proxyItems.passWord = array3[3];
				}
				proxyItems.TypeProxy = "FreeProxy";
				list.Add(proxyItems);
			}
		}
		return list;
	}

	public Dictionary<string, ProxyCommand> GetProxySupplier()
	{
		Dictionary<string, ProxyCommand> dictionary = new Dictionary<string, ProxyCommand>();
		dictionary.Add("OBC Proxy", new ProxyCommand
		{
			Reset = "reset?proxy",
			CheckStatusCmd = "status?proxy",
			HomePage = "http://obc.vn",
			Name = "OBC Proxy"
		});
		dictionary.Add("xProxy", new ProxyCommand
		{
			Reset = "reset?proxy",
			CheckStatusCmd = "status?proxy",
			HomePage = "https://www.facebook.com/xproxyvn",
			Name = "xProxy"
		});
		dictionary.Add("Eager Proxy", new ProxyCommand
		{
			Reset = "reset?proxy",
			CheckStatusCmd = "status?proxy",
			HomePage = "https://www.facebook.com/EagerProxyTaiNha",
			Name = "Eager Proxy"
		});
		dictionary.Add("SProxy", new ProxyCommand
		{
			Reset = "api/v1/reset?proxy",
			CheckStatusCmd = "api/v1/status?proxy",
			HomePage = "https://www.facebook.com/groups/325341675281268",
			Name = "SProxy"
		});
		dictionary.Add("MobiProxy", new ProxyCommand
		{
			Reset = "reset?proxy",
			CheckStatusCmd = "status?proxy",
			HomePage = "https://www.facebook.com/MobiProxy",
			Name = "MobiProxy"
		});
		dictionary.Add("MobiProxy v2", new ProxyCommand
		{
			Reset = "reset?proxy",
			CheckStatusCmd = "status?proxy",
			HomePage = "https://www.facebook.com/MobiProxy",
			Name = "MobiProxy"
		});
		dictionary.Add("MobiProxy v3", new ProxyCommand
		{
			Reset = "proxy_recreat?proxy",
			CheckStatusCmd = "proxy_check?proxy",
			HomePage = "https://www.facebook.com/MobiProxy",
			Name = "MobiProxy"
		});
		return dictionary;
	}

	public bool ResetCircuitProxy(string ServiceUrl, string ip, string port, string cmd)
	{
		try
		{
			string requestUri = $"{ServiceUrl}/reset?proxy={ip}:{port}";
			if (!string.IsNullOrEmpty(cmd))
			{
				requestUri = string.Format("{0}/{3}={1}:{2}", ServiceUrl, ip, port, cmd);
			}
			HttpClient httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri(ServiceUrl);
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			HttpResponseMessage result = httpClient.GetAsync(requestUri).Result;
			if (result.IsSuccessStatusCode)
			{
				httpClient.Dispose();
				return true;
			}
			httpClient.Dispose();
			return false;
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			return false;
		}
	}

	public bool CheckCircuitProxy(string ServiceUrl, string ip, string port, ref string msg, string cmd)
	{
		try
		{
			string requestUri = $"{ServiceUrl}/status?proxy={ip}:{port}";
			if (!string.IsNullOrEmpty(cmd))
			{
				requestUri = string.Format("{0}/{3}={1}:{2}", ServiceUrl, ip, port, cmd);
			}
			HttpClient httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri(ServiceUrl);
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			HttpResponseMessage result = httpClient.GetAsync(requestUri).Result;
			if (result.IsSuccessStatusCode)
			{
				XProxyStatus result2 = result.Content.ReadAsAsync<XProxyStatus>().Result;
				httpClient.Dispose();
				if (result2 != null)
				{
					msg = result2.msg;
					return result2.status;
				}
				return false;
			}
			httpClient.Dispose();
			return false;
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			return false;
		}
	}

	public void InitCircuitProxyTable(ref DataTable dtCircuitProxy)
	{
		if (dtCircuitProxy == null)
		{
			dtCircuitProxy = new DataTable("XProxyV2");
		}
		if (!dtCircuitProxy.Columns.Contains("ServiceUrl"))
		{
			dtCircuitProxy.Columns.Add("ServiceUrl", typeof(string));
		}
		if (!dtCircuitProxy.Columns.Contains("fullProxy"))
		{
			dtCircuitProxy.Columns.Add("fullProxy", typeof(string));
		}
		if (!dtCircuitProxy.Columns.Contains("ip"))
		{
			dtCircuitProxy.Columns.Add("ip", typeof(string));
		}
		if (!dtCircuitProxy.Columns.Contains("port"))
		{
			dtCircuitProxy.Columns.Add("port", typeof(string));
		}
		if (!dtCircuitProxy.Columns.Contains("user"))
		{
			dtCircuitProxy.Columns.Add("user", typeof(string));
		}
		if (!dtCircuitProxy.Columns.Contains("pass"))
		{
			dtCircuitProxy.Columns.Add("pass", typeof(string));
		}
	}

	public string GetIP(ref int Lan, TypeIp typeIp)
	{
		try
		{
			Lan++;
			string text = "";
			WebRequest webRequest = WebRequest.Create(typeIp switch
			{
				TypeIp.IPv4 => "http://v4.ipv6-test.com/api/myip.php", 
				TypeIp.IPv6 => "http://v6.ipv6-test.com/api/myip.php", 
				_ => "http://v4v6.ipv6-test.com/api/myip.php", 
			});
			WebResponse response = webRequest.GetResponse();
			StreamReader streamReader = new StreamReader(response.GetResponseStream());
			return streamReader.ReadToEnd().Trim();
		}
		catch (Exception ex)
		{
			if (Lan >= 30)
			{
				return $"Get IP error: {ex.Message}";
			}
			Thread.Sleep(1000);
			return GetIP(ref Lan, typeIp);
		}
	}

	public bool CheckNet()
	{
		int Description;
		return InternetGetConnectedState(out Description, 0);
	}

	public ProxyPort GetProxyPort(string port)
	{
		if (string.IsNullOrEmpty(port))
		{
			return ProxyPort.HttpIPv4;
		}
		return port[0] switch
		{
			'4' => ProxyPort.HttpIPv4, 
			'5' => ProxyPort.SocksIPv4, 
			'6' => ProxyPort.HttpIPv6, 
			'7' => ProxyPort.SocksIPv6, 
			_ => ProxyPort.HttpIPv4, 
		};
	}

	public List<int> LayTinhTP(string v)
	{
		List<string> list = Misc.SplitToList(v, ",");
		List<int> list2 = new List<int>();
		foreach (string item in list)
		{
			list2.Add(Misc.ObjInt(item.Trim()));
		}
		return list2;
	}

	public bool CheckNetworkStatus(string ProfileName)
	{
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		NetworkInterface[] array = allNetworkInterfaces;
		foreach (NetworkInterface networkInterface in array)
		{
			if (networkInterface.Name == ProfileName)
			{
				return true;
			}
		}
		return false;
	}

	public void Sleep(int Interval)
	{
		Thread.Sleep(Interval);
	}

	public void ResetAdapter()
	{
		string value = Misc.GetNetworkCurrentId().ToLower();
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		NetworkInterface[] array = allNetworkInterfaces;
		foreach (NetworkInterface networkInterface in array)
		{
			if (networkInterface.Id.ToLower().Contains(value))
			{
				DisableAdapter(networkInterface.Name);
			}
		}
		Sleep(3000);
		NetworkInterface[] array2 = allNetworkInterfaces;
		foreach (NetworkInterface networkInterface2 in array2)
		{
			if (networkInterface2.Id.ToLower().Contains(value))
			{
				EnableAdapter(networkInterface2.Name);
			}
		}
		Sleep(1000);
		NetworkInterface[] array3 = allNetworkInterfaces;
		foreach (NetworkInterface networkInterface3 in array3)
		{
			if (networkInterface3.Id.ToLower().Contains(value))
			{
				EnableAdapter(networkInterface3.Name);
			}
		}
		Sleep(1000);
		NetworkInterface[] array4 = allNetworkInterfaces;
		foreach (NetworkInterface networkInterface4 in array4)
		{
			if (networkInterface4.Id.ToLower().Contains(value))
			{
				EnableAdapter(networkInterface4.Name);
			}
		}
	}

	public void EnableAdapter(string interfaceName)
	{
		try
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" enable");
			processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			Process process = new Process();
			process.StartInfo = processStartInfo;
			process.Start();
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}

	public void DisableAdapter(string interfaceName)
	{
		try
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" disable");
			processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			Process process = new Process();
			process.StartInfo = processStartInfo;
			process.Start();
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}

	public string GetRandomMacAddress()
	{
		Random random = new Random();
		byte[] array = new byte[5];
		random.NextBytes(array);
		string text = string.Concat(array.Select((byte x) => string.Format("{0}", x.ToString("X2"))).ToArray());
		return "02" + text.TrimEnd(':');
	}

	public string RunCMD(string cmd, int Delay)
	{
		Process process = new Process();
		process.StartInfo.FileName = "cmd.exe";
		process.StartInfo.Arguments = "/c " + cmd;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.CreateNoWindow = true;
		process.Start();
		process.WaitForExit(Delay);
		string text = process.StandardOutput.ReadToEnd();
		if (string.IsNullOrEmpty(text))
		{
			return "";
		}
		return text;
	}

	public bool IsConnect(string NetworkName)
	{
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		NetworkInterface[] array = allNetworkInterfaces;
		foreach (NetworkInterface networkInterface in array)
		{
			if ((networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ppp) && networkInterface.Name.Equals(NetworkName))
			{
				return networkInterface.OperationalStatus == OperationalStatus.Up;
			}
		}
		return false;
	}

	public bool IsConnect(List<string> lstNetworkName)
	{
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		NetworkInterface[] array = allNetworkInterfaces;
		foreach (NetworkInterface networkInterface in array)
		{
			if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Ethernet && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Wireless80211 && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Ppp)
			{
				continue;
			}
			foreach (string item in lstNetworkName)
			{
				if (networkInterface.Name.Equals(item) && networkInterface.OperationalStatus == OperationalStatus.Up)
				{
					return true;
				}
			}
		}
		return false;
	}

	public ProxyAllType GetProxyFromData(DataRow dr)
	{
		ProxyAllType result = null;
		if (dr == null)
		{
			return result;
		}
		result = new ProxyAllType();
		string text = Misc.ToString(dr["proxy"]).Trim();
		string key = Misc.ToString(dr["Key"]).Trim();
		int num = Misc.ObjInt(dr["NCCProxy"]);
		int typeProxy = Misc.ObjInt(dr["LoaiProxy"]);
		string location = Misc.ToString(dr["TinhTP"]).Trim();
		switch (num)
		{
		case 0:
		{
			if (string.IsNullOrEmpty(text))
			{
				break;
			}
			List<string> list2 = Misc.SplitToList(text, ":");
			if (list2.Count >= 2)
			{
				result.ip = list2[0];
				result.port = list2[1];
				if (list2.Count >= 4)
				{
					result.userName = list2[2];
					result.passWord = list2[3];
				}
			}
			break;
		}
		case 1:
			if (!string.IsNullOrEmpty(text))
			{
				List<string> list3 = Misc.SplitToList(text, ":");
				if (list3.Count > 0)
				{
					result.ip = list3[0];
					if (list3.Count > 1)
					{
						result.port = list3[1];
					}
				}
			}
			result.Supplier = 1;
			result.Key = key;
			result.Location = location;
			result.TypeProxy = typeProxy;
			break;
		case 2:
			if (!string.IsNullOrEmpty(text))
			{
				List<string> list = Misc.SplitToList(text, ":");
				if (list.Count > 0)
				{
					result.ip = list[0];
					if (list.Count > 1)
					{
						result.port = list[1];
					}
				}
			}
			result.Supplier = 2;
			result.Key = key;
			result.Location = location;
			result.TypeProxy = typeProxy;
			break;
		}
		return result;
	}

	public DataTable GetDeviceType()
	{
		DataTable dataTable = new DataTable("DeviceType");
		if (!dataTable.Columns.Contains("ID"))
		{
			dataTable.Columns.Add("ID", typeof(int));
		}
		if (!dataTable.Columns.Contains("NAME"))
		{
			dataTable.Columns.Add("NAME", typeof(string));
		}
		dataTable.Rows.Add(0, "Tất cả");
		dataTable.Rows.Add(1, "Mobile");
		dataTable.Rows.Add(2, "PC");
		return dataTable;
	}

	public DataTable GetBrowserLanguage()
	{
		DataTable dataTable = new DataTable("BrowserLanguage");
		if (!dataTable.Columns.Contains("ID"))
		{
			dataTable.Columns.Add("ID", typeof(string));
		}
		if (!dataTable.Columns.Contains("NAME"))
		{
			dataTable.Columns.Add("NAME", typeof(string));
		}
		if (!dataTable.Columns.Contains("VALUE"))
		{
			dataTable.Columns.Add("VALUE", typeof(string));
		}
		dataTable.Rows.Add("vi-VN", "Tiếng Việt", "vi-VN,vi;q=0.9");
		dataTable.Rows.Add("en-US", "English", "en-US,en;q=0.9");
		return dataTable;
	}

	public DataTable GetTypeProxy()
	{
		DataTable dataTable = new DataTable("TypeProxy");
		if (!dataTable.Columns.Contains("ID"))
		{
			dataTable.Columns.Add("ID", typeof(int));
		}
		if (!dataTable.Columns.Contains("NAME"))
		{
			dataTable.Columns.Add("NAME", typeof(string));
		}
		dataTable.Rows.Add(1, "Key thường");
		dataTable.Rows.Add(2, "Key VIP");
		dataTable.Rows.Add(3, "Key dùng nhanh");
		return dataTable;
	}

	public string GetValueBrowserLanguage(DataTable dtData, string Value)
	{
		if (dtData == null || string.IsNullOrEmpty(Value))
		{
			return string.Empty;
		}
		List<string> list = Misc.SplitToList(Value, ",");
		List<string> list2 = new List<string>();
		foreach (string item in list)
		{
			DataRow[] array = dtData.Select("ID='" + item + "'");
			if (array != null && array.Length != 0)
			{
				list2.Add(Misc.ToString(array[0]["VALUE"]));
			}
		}
		if (list2.Count > 0)
		{
			return string.Join(",", list2.ToArray());
		}
		return string.Empty;
	}
}
