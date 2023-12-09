using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using RestSharp;
using NATechApi.Models;

namespace NATechApi;

public class ApiClientNATech
{
	public class ParamRequestNATech
	{
		public string name { get; set; }

		public object value { get; set; }

		public string contentType { get; set; }

		public ParamRequestNATech()
		{
		}

		public ParamRequestNATech(string name, object value, string contentType)
		{
			this.name = name;
			this.value = value;
			this.contentType = contentType;
		}
	}

	private static string _HardKey = string.Empty;

	public static string ApiUrl { get; set; }

	public static int SoftId { get; set; }

	public static string LinkDownloadSoft { get; set; }

	public static string SoftwareZipFileName { get; set; }

	public static string PathEXE { get; set; }

	public static string HardKey
	{
		get
		{
			if (string.IsNullOrEmpty(_HardKey))
			{
				_HardKey = GetRequestContent();
			}
			return _HardKey;
		}
		set
		{
			_HardKey = value;
		}
	}

	public static LoginResponse loginResponse { get; set; }

	public string GetApi(string strAPIName, List<ParamRequestNATech> lstParam, string Token, out string sErr)
	{
		sErr = string.Empty;
		try
		{
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true));
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
			RestClient client = new RestClient(ApiUrl + "/" + strAPIName);
			RestRequest restRequest = new RestRequest(Method.GET);
			if (!string.IsNullOrEmpty(Token))
			{
				restRequest.AddHeader("Authorization", Token);
			}
			restRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
			restRequest.AddHeader("Accept", "application/json");
			if (lstParam != null)
			{
				foreach (ParamRequestNATech item in lstParam)
				{
					if (!string.IsNullOrEmpty(item.contentType))
					{
						restRequest.AddParameter(item.name, item.value, item.contentType, ParameterType.GetOrPost);
					}
					else
					{
						restRequest.AddParameter(item.name, item.value, ParameterType.GetOrPost);
					}
				}
			}
			IRestResponse restResponse = client.Get(restRequest);
			if (restResponse.StatusCode == HttpStatusCode.OK)
			{
				return restResponse.Content;
			}
			sErr = "CallAPI>Caller> response.StatusCode: " + restResponse.StatusCode.ToString() + "</br>" + restResponse.ErrorMessage;
			return string.Empty;
		}
		catch (Exception ex)
		{
			sErr = "CallAPI>Caller> " + ex.Message;
			return string.Empty;
		}
	}

	public string PostApi(string strAPIName, object strRequestData, string Token, out string sErr)
	{
		sErr = string.Empty;
		try
		{
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true));
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
			RestClient client = new RestClient(ApiUrl + "/" + strAPIName);
			RestRequest restRequest = new RestRequest(Method.POST);
			if (!string.IsNullOrEmpty(Token))
			{
				restRequest.AddHeader("Authorization", Token);
			}
			restRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
			restRequest.AddHeader("Accept", "application/json");
			restRequest.AddJsonBody(strRequestData);
			IRestResponse restResponse = client.Post(restRequest);
			if (restResponse.StatusCode == HttpStatusCode.OK)
			{
				return restResponse.Content;
			}
			sErr = "CallAPI>Caller> response.StatusCode: " + restResponse.StatusCode.ToString() + "</br>" + restResponse.ErrorMessage;
			return string.Empty;
		}
		catch (Exception ex)
		{
			sErr = "CallAPI>Caller> " + ex.Message;
			return string.Empty;
		}
	}

	public static string GetRequestContent()
	{
		return $"{GetMD5($"{HardwarePC.ComputerName(15)}_{HardwarePC.cpuId()}_{HardwarePC.MainBoardSerialNumber()}_{HardwarePC.biosSerialNumber()}").Substring(0, 10).ToUpper()}";
	}

	public static string GetMD5(string s)
	{
		try
		{
			int num = 0;
			int num2 = ((num != 1) ? 7 : 0);
			int num3 = ((num != 1) ? 7 : 0);
			int num4 = ((num != 1) ? 7 : 0);
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes, 0, bytes.Length);
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				text += $"{array[i]:x2}";
			}
			return text;
		}
		catch
		{
			return "";
		}
	}
}
