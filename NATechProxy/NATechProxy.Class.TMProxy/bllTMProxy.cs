using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;

namespace NATechProxy.Class.TMProxy;

public class bllTMProxy
{
	public class TMProxyData
	{
		public bool DangLay { get; set; }

		public DateTime LastChange { get; set; }

		public int ThoiGianCho { get; set; }

		public ProxyItems LastProxy { get; set; }
	}

	public const string ApiServiceUrl = "https://tmproxy.com/api/proxy";

	public string errorCode = "";

	public string api_key { get; set; }

	public int location { get; set; }

	public string proxy { get; set; }

	public string ip { get; set; }

	public int port { get; set; }

	public int timeout { get; set; }

	public string sock5 { get; set; }

	public int socks5_port { get; set; }

	public int next_change { get; set; }

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
		dataTable.Rows.Add(1, "Nuôi Acc (10k/ngày)");
		dataTable.Rows.Add(2, "Đổi IP (4K/ngày)");
		dataTable.Rows.Add(3, "Đổi IP nhanh (15k/ngày)");
		dataTable.Rows.Add(4, "Đổi IP 2p (8k/ngày)");
		return dataTable;
	}

	public DataTable getLocations(out string sErr)
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
		sErr = string.Empty;
		string value = Misc.PostApi("https://tmproxy.com/api/proxy", "location", string.Empty, out sErr);
		if (string.IsNullOrEmpty(sErr))
		{
			dynamic val = JsonConvert.DeserializeObject<object>(value);
			if (val != null)
			{
				List<LocationModel> list = JsonConvert.DeserializeObject<List<LocationModel>>(Misc.ToString(val["data"]["locations"]));
				if (list != null)
				{
					foreach (LocationModel item in list)
					{
						DataRow dataRow = dataTable.NewRow();
						dataRow["location"] = Misc.ToString(item.id_location);
						dataRow["name"] = item.name;
						dataTable.Rows.Add(dataRow);
					}
				}
			}
		}
		return dataTable;
	}

	public bool changeProxy()
	{
		try
		{
			errorCode = "";
			next_change = 0;
			proxy = "";
			ip = "";
			port = 0;
			timeout = 0;
			string sErr = string.Empty;
			requestNewProxy requestNewProxy2 = new requestNewProxy();
			requestNewProxy2.api_key = api_key;
			string value = Misc.PostApi("https://tmproxy.com/api/proxy", "get-new-proxy", requestNewProxy2, out sErr);
			if (!string.IsNullOrEmpty(sErr))
			{
				errorCode = sErr;
				return false;
			}
			dynamic val = JsonConvert.DeserializeObject<object>(value);
			if (val == null)
			{
				errorCode = "Không có dữ liệu";
				return false;
			}
			if ((!string.IsNullOrEmpty(Misc.ToString(val["message"]))))
			{
				errorCode = Misc.ToString(val["message"]);
				return false;
			}
			responseNewProxy responseNewProxy2 = JsonConvert.DeserializeObject<responseNewProxy>(Misc.ToString(val["data"]));
			if (responseNewProxy2 != null)
			{
				proxy = responseNewProxy2.https;
				sock5 = responseNewProxy2.socks5;
				List<string> list = Misc.SplitToList(responseNewProxy2.https, ":");
				if (list.Count > 1)
				{
					ip = list[0];
					port = Misc.ObjInt(list[1]);
				}
				List<string> list2 = Misc.SplitToList(responseNewProxy2.socks5, ":");
				if (list2.Count > 1)
				{
					ip = list2[0];
					socks5_port = Misc.ObjInt(list2[1]);
				}
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			errorCode = ex.Message;
			return false;
		}
	}

	public void InitData(ref DataTable dtTMProxy, string KeywordFileName)
	{
		if (dtTMProxy == null)
		{
			dtTMProxy = new DataTable("TMProxy");
		}
		if (File.Exists(KeywordFileName))
		{
			try
			{
				dtTMProxy.ReadXml(KeywordFileName);
			}
			catch (Exception)
			{
				dtTMProxy = new DataTable("TMProxy");
			}
		}
		if (!dtTMProxy.Columns.Contains("Type"))
		{
			dtTMProxy.Columns.Add("Type", typeof(int));
		}
		if (!dtTMProxy.Columns.Contains("Key"))
		{
			dtTMProxy.Columns.Add("Key", typeof(string));
		}
		if (!dtTMProxy.Columns.Contains("TinhTP"))
		{
			dtTMProxy.Columns.Add("TinhTP", typeof(string));
		}
	}

	public int LayThoiGianCho(int v)
	{
		return v switch
		{
			1 => 600, 
			2 => 240, 
			3 => 60, 
			4 => 120, 
			_ => 120, 
		};
	}
}
