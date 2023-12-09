using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;

namespace NATechProxy;

public class TinsoftProxy
{
	public class TinsoftData
	{
		public bool DangLay { get; set; }

		public DateTime LastChange { get; set; }

		public int ThoiGianCho { get; set; }

		public ProxyItems LastProxy { get; set; }
	}

	public class TinsoftModel
	{
		public int Type { get; set; }

		public string Key { get; set; }

		public string Location { get; set; }
	}

	public string errorCode = "";

	private string svUrl = "http://proxy.tinsoftsv.com";

	private int lastRequest = 0;

	public string api_key { get; set; }

	public string proxy { get; set; }

	public string ip { get; set; }

	public int port { get; set; }

	public int timeout { get; set; }

	public int next_change { get; set; }

	public int location { get; set; }

	public TinsoftProxy()
	{
	}

	public TinsoftProxy(string api_key, int location = 0)
	{
		this.api_key = api_key;
		proxy = "";
		ip = "";
		port = 0;
		timeout = 0;
		next_change = 0;
		this.location = location;
	}

	public bool changeProxy()
	{
		if (checkLastRequest())
		{
			errorCode = "";
			next_change = 0;
			proxy = "";
			ip = "";
			port = 0;
			timeout = 0;
			string sVContent = getSVContent(svUrl + "/api/changeProxy.php?key=" + api_key + "&location=" + location);
			if (sVContent != "")
			{
				try
				{
					JObject jObject = JObject.Parse(sVContent);
					if (bool.Parse(jObject["success"].ToString()))
					{
						proxy = jObject["proxy"].ToString();
						string[] array = proxy.Split(':');
						ip = array[0];
						port = int.Parse(array[1]);
						timeout = int.Parse(jObject["timeout"].ToString());
						next_change = int.Parse(jObject["next_change"].ToString());
						errorCode = "";
						return true;
					}
					errorCode = jObject["description"].ToString();
				}
				catch
				{
				}
			}
			else
			{
				errorCode = "request server timeout!";
			}
		}
		else
		{
			errorCode = "Request so fast!";
		}
		return false;
	}

	public void stopProxy()
	{
		errorCode = "";
		proxy = "";
		ip = "";
		port = 0;
		timeout = 0;
		if (api_key != "")
		{
			getSVContent(svUrl + "/api/stopProxy.php?key=" + api_key);
		}
	}

	public bool getProxyStatus()
	{
		if (checkLastRequest())
		{
			errorCode = "";
			proxy = "";
			ip = "";
			port = 0;
			timeout = 0;
			string sVContent = getSVContent(svUrl + "/api/getProxy.php?key=" + api_key);
			if (sVContent != "")
			{
				try
				{
					JObject jObject = JObject.Parse(sVContent);
					if (bool.Parse(jObject["success"].ToString()))
					{
						proxy = jObject["proxy"].ToString();
						string[] array = proxy.Split(':');
						ip = array[0];
						port = int.Parse(array[1]);
						timeout = int.Parse(jObject["timeout"].ToString());
						next_change = int.Parse(jObject["next_change"].ToString());
						errorCode = "";
						return true;
					}
					errorCode = jObject["description"].ToString();
				}
				catch
				{
				}
			}
		}
		else
		{
			errorCode = "Request so fast!";
		}
		return false;
	}

	public DataTable getLocations()
	{
		DataTable dataTable = new DataTable("Locations");
		if (!dataTable.Columns.Contains("location"))
		{
			dataTable.Columns.Add("location", typeof(string));
		}
		if (!dataTable.Columns.Contains("name"))
		{
			dataTable.Columns.Add("name", typeof(string));
		}
		if (checkLastRequest())
		{
			errorCode = "";
			string sVContent = getSVContent(svUrl + "/api/getLocations.php");
			if (sVContent != "")
			{
				try
				{
					JObject jObject = JObject.Parse(sVContent);
					if (bool.Parse(jObject["success"].ToString()))
					{
						List<JToken> list = jObject["data"].ToList();
						foreach (JToken item in list)
						{
							DataRow dataRow = dataTable.NewRow();
							dataRow["location"] = item["location"].ToString();
							dataRow["name"] = item["name"].ToString();
							dataTable.Rows.Add(dataRow);
						}
						errorCode = "";
					}
					else
					{
						errorCode = jObject["description"].ToString();
					}
				}
				catch
				{
				}
			}
		}
		else
		{
			errorCode = "Request so fast!";
		}
		return dataTable;
	}

	private bool checkLastRequest()
	{
		try
		{
			DateTime dateTime = new DateTime(2001, 1, 1);
			long ticks = DateTime.Now.Ticks - dateTime.Ticks;
			int num = (int)new TimeSpan(ticks).TotalSeconds;
			if (num - lastRequest >= 10)
			{
				lastRequest = num;
				return true;
			}
		}
		catch
		{
		}
		return false;
	}

	private string getSVContent(string url)
	{
		Console.WriteLine(url);
		string text = "";
		try
		{
			using (WebClient webClient = new WebClient())
			{
				webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
				text = webClient.DownloadString(url);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "";
			}
		}
		catch
		{
			text = "";
		}
		return text;
	}

	public DataTable GetProxyType()
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

	public void InitData(ref DataTable dtTinsoft, string KeywordFileName)
	{
		if (dtTinsoft == null)
		{
			dtTinsoft = new DataTable("TinSoft");
		}
		if (File.Exists(KeywordFileName))
		{
			try
			{
				dtTinsoft.ReadXml(KeywordFileName);
			}
			catch (Exception)
			{
				dtTinsoft = new DataTable("TinSoft");
			}
		}
		if (!dtTinsoft.Columns.Contains("Type"))
		{
			dtTinsoft.Columns.Add("Type", typeof(int));
		}
		if (!dtTinsoft.Columns.Contains("Key"))
		{
			dtTinsoft.Columns.Add("Key", typeof(string));
		}
		if (!dtTinsoft.Columns.Contains("TinhTP"))
		{
			dtTinsoft.Columns.Add("TinhTP", typeof(string));
		}
		if (!dtTinsoft.Columns.Contains("Select"))
		{
			dtTinsoft.Columns.Add("Select", typeof(bool));
		}
		dtTinsoft.Columns["Select"].DefaultValue = true;
		foreach (DataRow row in dtTinsoft.Rows)
		{
			if (Misc.IsNullOrDbNull(row["Select"]))
			{
				row["Select"] = true;
			}
		}
	}

	public int LayThoiGianCho(int v)
	{
		return v switch
		{
			1 => 121, 
			2 => 121, 
			3 => 61, 
			_ => 121, 
		};
	}
}
