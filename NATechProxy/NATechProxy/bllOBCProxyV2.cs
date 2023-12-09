using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using NATechProxy.Class;

namespace NATechProxy;

public class bllOBCProxyV2
{
	public void InitOBCv2ProxyV2Table(ref DataTable dtOBCProxyV2Local)
	{
		if (dtOBCProxyV2Local == null)
		{
			dtOBCProxyV2Local = new DataTable("OBCProxyV2");
		}
		if (!dtOBCProxyV2Local.Columns.Contains("ServiceUrl"))
		{
			dtOBCProxyV2Local.Columns.Add("ServiceUrl", typeof(string));
		}
		if (!dtOBCProxyV2Local.Columns.Contains("stt"))
		{
			dtOBCProxyV2Local.Columns.Add("stt", typeof(int));
		}
		if (!dtOBCProxyV2Local.Columns.Contains("imei"))
		{
			dtOBCProxyV2Local.Columns.Add("imei", typeof(string));
		}
		if (!dtOBCProxyV2Local.Columns.Contains("public_ip"))
		{
			dtOBCProxyV2Local.Columns.Add("public_ip", typeof(string));
		}
		if (!dtOBCProxyV2Local.Columns.Contains("proxy_port"))
		{
			dtOBCProxyV2Local.Columns.Add("proxy_port", typeof(int));
		}
		if (!dtOBCProxyV2Local.Columns.Contains("sock_port"))
		{
			dtOBCProxyV2Local.Columns.Add("sock_port", typeof(int));
		}
		if (!dtOBCProxyV2Local.Columns.Contains("system"))
		{
			dtOBCProxyV2Local.Columns.Add("system", typeof(string));
		}
		if (!dtOBCProxyV2Local.Columns.Contains("sim_status"))
		{
			dtOBCProxyV2Local.Columns.Add("sim_status", typeof(int));
		}
		if (!dtOBCProxyV2Local.Columns.Contains("proxy_full"))
		{
			dtOBCProxyV2Local.Columns.Add("proxy_full", typeof(string));
		}
		if (!dtOBCProxyV2Local.Columns.Contains("proxy_ip"))
		{
			dtOBCProxyV2Local.Columns.Add("proxy_ip", typeof(string));
		}
		if (!dtOBCProxyV2Local.Columns.Contains("IsRun"))
		{
			dtOBCProxyV2Local.Columns.Add("IsRun", typeof(bool));
		}
		if (!dtOBCProxyV2Local.Columns.Contains("proxy_ip"))
		{
			dtOBCProxyV2Local.Columns.Add("proxy_ip", typeof(string));
		}
	}

	public void RefreshOBCv2ProxyList(ref string err, string OBCProxyV2Host, ref DataTable dtDataXProxy, bool IsClearAll)
	{
		try
		{
			InitOBCv2ProxyV2Table(ref dtDataXProxy);
			if (IsClearAll)
			{
				dtDataXProxy.Rows.Clear();
			}
			string text = $"{OBCProxyV2Host}/proxy_list";
			string text2 = Misc.SplitToList(OBCProxyV2Host, ":")[1].TrimStart('/', '/');
			HttpClient httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri(text);
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			HttpResponseMessage result = httpClient.GetAsync(text).Result;
			if (result.IsSuccessStatusCode)
			{
				IEnumerable<OBCv2Item> result2 = result.Content.ReadAsAsync<IEnumerable<OBCv2Item>>().Result;
				int num = 0;
				foreach (OBCv2Item item in result2)
				{
					num++;
					DataRow[] array = dtDataXProxy.Select($"imei='{item.imei}'");
					if (array.Length == 0)
					{
						DataRow dataRow = dtDataXProxy.NewRow();
						dataRow["ServiceUrl"] = OBCProxyV2Host;
						dataRow["stt"] = num;
						dataRow["imei"] = item.imei;
						if (!string.IsNullOrEmpty(item.public_ip))
						{
							dataRow["public_ip"] = item.public_ip;
						}
						if (item.proxy_port.HasValue)
						{
							dataRow["proxy_port"] = item.proxy_port;
						}
						if (item.sock_port.HasValue)
						{
							dataRow["sock_port"] = item.sock_port;
						}
						if (!string.IsNullOrEmpty(item.system))
						{
							dataRow["system"] = item.system;
						}
						dataRow["proxy_full"] = $"{text2}:{item.proxy_port}";
						dataRow["proxy_ip"] = text2;
						dataRow["IsRun"] = true;
						dtDataXProxy.Rows.Add(dataRow);
					}
					else
					{
						DataRow dataRow2 = array[0];
						dataRow2["stt"] = num;
						if (!string.IsNullOrEmpty(item.public_ip))
						{
							dataRow2["public_ip"] = item.public_ip;
						}
						if (item.proxy_port.HasValue)
						{
							dataRow2["proxy_port"] = item.proxy_port;
						}
						if (item.sock_port.HasValue)
						{
							dataRow2["sock_port"] = item.sock_port;
						}
						if (!string.IsNullOrEmpty(item.system))
						{
							dataRow2["system"] = item.system;
						}
						dataRow2["proxy_full"] = $"{text2}:{item.proxy_port}";
						dataRow2["proxy_ip"] = text2;
					}
				}
			}
			else
			{
				err = $"{result.StatusCode} ({result.ReasonPhrase})";
			}
			httpClient.Dispose();
		}
		catch (Exception ex)
		{
			err = ex.Message;
		}
	}

	public bool ResetOBCProxyV2(string OBCProxyV2Host, string ip, string port)
	{
		string requestUri = $"{OBCProxyV2Host}/reset?proxy={ip}:{port}";
		HttpClient httpClient = new HttpClient();
		httpClient.BaseAddress = new Uri(OBCProxyV2Host);
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

	public bool CheckStatusOBCProxyV2(string OBCProxyV2Host, string ip, string port, ref string msg)
	{
		string requestUri = $"{OBCProxyV2Host}/status?proxy={ip}:{port}";
		HttpClient httpClient = new HttpClient();
		httpClient.BaseAddress = new Uri(OBCProxyV2Host);
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
}
