using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NATechProxy;

public class bllXProxy
{
	public void InitXProxyTable(ref DataTable dtXProxyLocal)
	{
		if (dtXProxyLocal == null)
		{
			dtXProxyLocal = new DataTable("XProxy");
		}
		if (!dtXProxyLocal.Columns.Contains("ServiceUrl"))
		{
			dtXProxyLocal.Columns.Add("ServiceUrl", typeof(string));
		}
		if (!dtXProxyLocal.Columns.Contains("stt"))
		{
			dtXProxyLocal.Columns.Add("stt", typeof(int));
		}
		if (!dtXProxyLocal.Columns.Contains("imei"))
		{
			dtXProxyLocal.Columns.Add("imei", typeof(string));
		}
		if (!dtXProxyLocal.Columns.Contains("public_ip"))
		{
			dtXProxyLocal.Columns.Add("public_ip", typeof(string));
		}
		if (!dtXProxyLocal.Columns.Contains("proxy_port"))
		{
			dtXProxyLocal.Columns.Add("proxy_port", typeof(int));
		}
		if (!dtXProxyLocal.Columns.Contains("sock_port"))
		{
			dtXProxyLocal.Columns.Add("sock_port", typeof(int));
		}
		if (!dtXProxyLocal.Columns.Contains("system"))
		{
			dtXProxyLocal.Columns.Add("system", typeof(string));
		}
		if (!dtXProxyLocal.Columns.Contains("sim_status"))
		{
			dtXProxyLocal.Columns.Add("sim_status", typeof(int));
		}
		if (!dtXProxyLocal.Columns.Contains("proxy_full"))
		{
			dtXProxyLocal.Columns.Add("proxy_full", typeof(string));
		}
		if (!dtXProxyLocal.Columns.Contains("proxy_ip"))
		{
			dtXProxyLocal.Columns.Add("proxy_ip", typeof(string));
		}
		if (!dtXProxyLocal.Columns.Contains("IsRun"))
		{
			dtXProxyLocal.Columns.Add("IsRun", typeof(bool));
		}
		if (!dtXProxyLocal.Columns.Contains("public_ip_v6"))
		{
			dtXProxyLocal.Columns.Add("public_ip_v6", typeof(string));
		}
		if (!dtXProxyLocal.Columns.Contains("proxy_port_v6"))
		{
			dtXProxyLocal.Columns.Add("proxy_port_v6", typeof(int));
		}
		if (!dtXProxyLocal.Columns.Contains("sock_port_v6"))
		{
			dtXProxyLocal.Columns.Add("sock_port_v6", typeof(int));
		}
	}

	public void InitXProxyV2Table(ref DataTable dtXProxyV2Local)
	{
		if (dtXProxyV2Local == null)
		{
			dtXProxyV2Local = new DataTable("XProxyV2");
		}
		if (!dtXProxyV2Local.Columns.Contains("ServiceUrl"))
		{
			dtXProxyV2Local.Columns.Add("ServiceUrl", typeof(string));
		}
		if (!dtXProxyV2Local.Columns.Contains("ip"))
		{
			dtXProxyV2Local.Columns.Add("ip", typeof(string));
		}
		if (!dtXProxyV2Local.Columns.Contains("port"))
		{
			dtXProxyV2Local.Columns.Add("port", typeof(string));
		}
	}

	public void RefreshXProxyList(ref string err, string XProxyHost, ref DataTable dtDataXProxy, bool IsClearAll)
	{
		RefreshXProxyList(ref err, XProxyHost, ref dtDataXProxy, IsClearAll, 1, 1);
	}

	public void RefreshXProxyList(ref string err, string XProxyHost, ref DataTable dtDataXProxy, bool IsClearAll, int TypeIpV4OrV6, int DeviceTypeDcomOrWAN)
	{
		try
		{
			InitXProxyTable(ref dtDataXProxy);
			if (IsClearAll)
			{
				dtDataXProxy.Rows.Clear();
			}
			string text = $"{XProxyHost}/proxy_list";
			string text2 = Misc.SplitToList(XProxyHost, ":")[1].TrimStart('/', '/');
			HttpClient httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri(text);
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			HttpResponseMessage result = httpClient.GetAsync(text).Result;
			if (result.IsSuccessStatusCode)
			{
				IEnumerable<XProxyItem> result2 = result.Content.ReadAsAsync<IEnumerable<XProxyItem>>().Result;
				int num = 0;
				foreach (XProxyItem item in result2)
				{
					if ((DeviceTypeDcomOrWAN != 1 || !(item.sim_status >= 0)) && DeviceTypeDcomOrWAN != 2)
					{
						continue;
					}
					num++;
					string text3 = ((DeviceTypeDcomOrWAN == 1) ? item.imei : item.position);
					DataRow[] array = dtDataXProxy.Select($"imei='{text3}'");
					if (array.Length == 0)
					{
						DataRow dataRow = dtDataXProxy.NewRow();
						dataRow["ServiceUrl"] = XProxyHost;
						dataRow["stt"] = num;
						dataRow["imei"] = text3;
						if (item.sim_status.HasValue)
						{
							dataRow["sim_status"] = item.sim_status;
						}
						if (item.proxy_port.HasValue)
						{
							dataRow["proxy_port"] = item.proxy_port;
						}
						if (item.sock_port.HasValue)
						{
							dataRow["sock_port"] = item.sock_port;
						}
						if (TypeIpV4OrV6 == 1)
						{
							if (!string.IsNullOrEmpty(item.public_ip))
							{
								dataRow["public_ip"] = item.public_ip;
							}
						}
						else if (!string.IsNullOrEmpty(item.public_ip_v6))
						{
							dataRow["public_ip"] = item.public_ip_v6;
						}
						if (item.proxy_port_v6.HasValue)
						{
							dataRow["proxy_port_v6"] = item.proxy_port_v6;
						}
						if (item.sock_port_v6.HasValue)
						{
							dataRow["sock_port_v6"] = item.sock_port_v6;
						}
						if (!string.IsNullOrEmpty(item.system))
						{
							dataRow["system"] = item.system;
						}
						dataRow["proxy_full"] = $"{text2}:{item.proxy_port}";
						dataRow["proxy_ip"] = text2;
						dataRow["IsRun"] = true;
						dtDataXProxy.Rows.Add(dataRow);
						continue;
					}
					DataRow dataRow2 = array[0];
					dataRow2["stt"] = num;
					if (item.sim_status.HasValue)
					{
						dataRow2["sim_status"] = item.sim_status;
					}
					if (item.proxy_port.HasValue)
					{
						dataRow2["proxy_port"] = item.proxy_port;
					}
					if (item.sock_port.HasValue)
					{
						dataRow2["sock_port"] = item.sock_port;
					}
					if (TypeIpV4OrV6 == 1)
					{
						if (!string.IsNullOrEmpty(item.public_ip))
						{
							dataRow2["public_ip"] = item.public_ip;
						}
					}
					else if (!string.IsNullOrEmpty(item.public_ip_v6))
					{
						dataRow2["public_ip"] = item.public_ip_v6;
					}
					if (item.proxy_port_v6.HasValue)
					{
						dataRow2["proxy_port_v6"] = item.proxy_port_v6;
					}
					if (item.sock_port_v6.HasValue)
					{
						dataRow2["sock_port_v6"] = item.sock_port_v6;
					}
					if (!string.IsNullOrEmpty(item.system))
					{
						dataRow2["system"] = item.system;
					}
					dataRow2["proxy_full"] = $"{text2}:{item.proxy_port}";
					dataRow2["proxy_ip"] = text2;
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

	public bool ResetXProxy(string XProxyHost, string ip, string port)
	{
		try
		{
			string requestUri = $"{XProxyHost}/reset?proxy={ip}:{port}";
			HttpClient httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri(XProxyHost);
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

	public bool CheckStatusXProxy(string XProxyHost, string ip, string port, ref string msg)
	{
		try
		{
			string requestUri = $"{XProxyHost}/status?proxy={ip}:{port}";
			HttpClient httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri(XProxyHost);
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
}
