using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NATechProxy;

public class bllOBCProxy
{
	public void InitOBCProxyTable(ref DataTable dtOBCProxyLocal)
	{
		if (dtOBCProxyLocal == null)
		{
			dtOBCProxyLocal = new DataTable("OBCProxy");
		}
		if (!dtOBCProxyLocal.Columns.Contains("ServiceUrl"))
		{
			dtOBCProxyLocal.Columns.Add("ServiceUrl", typeof(string));
		}
		if (!dtOBCProxyLocal.Columns.Contains("name"))
		{
			dtOBCProxyLocal.Columns.Add("name", typeof(string));
		}
		if (!dtOBCProxyLocal.Columns.Contains("idKey"))
		{
			dtOBCProxyLocal.Columns.Add("idKey", typeof(string));
		}
		if (!dtOBCProxyLocal.Columns.Contains("proxyAddress"))
		{
			dtOBCProxyLocal.Columns.Add("proxyAddress", typeof(string));
		}
		if (!dtOBCProxyLocal.Columns.Contains("port"))
		{
			dtOBCProxyLocal.Columns.Add("port", typeof(int));
		}
		if (!dtOBCProxyLocal.Columns.Contains("sockPort"))
		{
			dtOBCProxyLocal.Columns.Add("sockPort", typeof(int));
		}
		if (!dtOBCProxyLocal.Columns.Contains("proxyStatus"))
		{
			dtOBCProxyLocal.Columns.Add("proxyStatus", typeof(string));
		}
		if (!dtOBCProxyLocal.Columns.Contains("socksEnable"))
		{
			dtOBCProxyLocal.Columns.Add("socksEnable", typeof(bool));
		}
		if (!dtOBCProxyLocal.Columns.Contains("proxy_full"))
		{
			dtOBCProxyLocal.Columns.Add("proxy_full", typeof(string));
		}
		if (!dtOBCProxyLocal.Columns.Contains("proxy_ip"))
		{
			dtOBCProxyLocal.Columns.Add("proxy_ip", typeof(string));
		}
		if (!dtOBCProxyLocal.Columns.Contains("publicIp"))
		{
			dtOBCProxyLocal.Columns.Add("publicIp", typeof(string));
		}
		if (!dtOBCProxyLocal.Columns.Contains("userNameAuth"))
		{
			dtOBCProxyLocal.Columns.Add("userNameAuth", typeof(string));
		}
		if (!dtOBCProxyLocal.Columns.Contains("passwordAuth"))
		{
			dtOBCProxyLocal.Columns.Add("passwordAuth", typeof(string));
		}
		if (!dtOBCProxyLocal.Columns.Contains("IsRun"))
		{
			dtOBCProxyLocal.Columns.Add("IsRun", typeof(bool));
		}
	}

	public void RefreshOBCProxyList(ref string err, string OBCProxyHost, ref DataTable dtDataOBCProxy, bool IsClearAll)
	{
		try
		{
			InitOBCProxyTable(ref dtDataOBCProxy);
			if (IsClearAll)
			{
				dtDataOBCProxy.Rows.Clear();
			}
			string text = $"{OBCProxyHost.TrimEnd('/')}/list";
			string text2 = Misc.SplitToList(OBCProxyHost, ":")[1].TrimStart('/', '/');
			HttpClient httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri(text);
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			HttpResponseMessage result = httpClient.GetAsync(text).Result;
			if (result.IsSuccessStatusCode)
			{
				IEnumerable<OBCProxyItem> result2 = result.Content.ReadAsAsync<IEnumerable<OBCProxyItem>>().Result;
				int num = 0;
				foreach (OBCProxyItem item in result2)
				{
					if (!"running".Equals(item.proxyStatus.ToLower()) && !"resetting".Equals(item.proxyStatus.ToLower()))
					{
						continue;
					}
					num++;
					DataRow[] array = dtDataOBCProxy.Select($"idKey='{item.idKey}'");
					if (array.Length == 0)
					{
						DataRow dataRow = dtDataOBCProxy.NewRow();
						dataRow["ServiceUrl"] = OBCProxyHost;
						dataRow["idKey"] = item.idKey;
						dataRow["name"] = item.name;
						if (!string.IsNullOrEmpty(item.proxyAddress))
						{
							dataRow["proxyAddress"] = item.proxyAddress;
						}
						if (!string.IsNullOrEmpty(item.proxyStatus))
						{
							dataRow["proxyStatus"] = item.proxyStatus;
						}
						dataRow["port"] = item.port;
						dataRow["sockPort"] = item.sockPort;
						dataRow["socksEnable"] = item.socksEnable;
						dataRow["proxy_full"] = $"{text2}:{item.port}";
						dataRow["proxy_ip"] = text2;
						if (!string.IsNullOrEmpty(item.publicIp))
						{
							dataRow["publicIp"] = item.publicIp;
						}
						if (!string.IsNullOrEmpty(item.userNameAuth))
						{
							dataRow["userNameAuth"] = item.userNameAuth;
						}
						if (!string.IsNullOrEmpty(item.passwordAuth))
						{
							dataRow["passwordAuth"] = item.passwordAuth;
						}
						dataRow["IsRun"] = true;
						dtDataOBCProxy.Rows.Add(dataRow);
					}
					else
					{
						DataRow dataRow2 = array[0];
						dataRow2["name"] = item.name;
						if (!string.IsNullOrEmpty(item.proxyAddress))
						{
							dataRow2["proxyAddress"] = item.proxyAddress;
						}
						if (!string.IsNullOrEmpty(item.proxyStatus))
						{
							dataRow2["proxyStatus"] = item.proxyStatus;
						}
						dataRow2["port"] = item.port;
						dataRow2["sockPort"] = item.sockPort;
						dataRow2["socksEnable"] = item.socksEnable;
						dataRow2["proxy_full"] = $"{text2}:{item.port}";
						dataRow2["proxy_ip"] = text2;
						if (!string.IsNullOrEmpty(item.publicIp))
						{
							dataRow2["publicIp"] = item.publicIp;
						}
						if (!string.IsNullOrEmpty(item.userNameAuth))
						{
							dataRow2["userNameAuth"] = item.userNameAuth;
						}
						if (!string.IsNullOrEmpty(item.passwordAuth))
						{
							dataRow2["passwordAuth"] = item.passwordAuth;
						}
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

	public bool ResetOBCProxy(string OBCProxyHost, string port)
	{
		string requestUri = $"{OBCProxyHost}/proxy_reset?proxy={port}";
		HttpClient httpClient = new HttpClient();
		httpClient.BaseAddress = new Uri(OBCProxyHost);
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

	public string GetStatusOBCProxy(string OBCProxyHost, string idKey)
	{
		string text = $"{OBCProxyHost}/proxy/info/{idKey}";
		HttpClient httpClient = new HttpClient();
		httpClient.BaseAddress = new Uri(text);
		httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		HttpResponseMessage result = httpClient.GetAsync(text).Result;
		if (result.IsSuccessStatusCode)
		{
			OBCProxyItem result2 = result.Content.ReadAsAsync<OBCProxyItem>().Result;
			httpClient.Dispose();
			if (result2 == null)
			{
				return "Waiting";
			}
			return result2.proxyStatus;
		}
		httpClient.Dispose();
		return "Error";
	}

	public string GetOBCProxyPublicIp(string OBCProxyHost, string idKey)
	{
		string text = $"{OBCProxyHost}/proxy/ip/{idKey}";
		HttpClient httpClient = new HttpClient();
		httpClient.BaseAddress = new Uri(text);
		httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		HttpResponseMessage result = httpClient.GetAsync(text).Result;
		if (result.IsSuccessStatusCode)
		{
			OBCProxyAddress result2 = result.Content.ReadAsAsync<OBCProxyAddress>().Result;
			httpClient.Dispose();
			if (result2 == null)
			{
				return string.Empty;
			}
			return result2.ipAddress;
		}
		httpClient.Dispose();
		return string.Empty;
	}
}
