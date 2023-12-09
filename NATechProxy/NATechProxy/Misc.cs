using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Script.Serialization;
using System.Xml;
using Microsoft.Win32;
using RestSharp;

namespace NATechProxy;

public class Misc
{
	public static object ConvertErp(string Value, Type t)
	{
		string roundOrFormat = ((t == typeof(DateTime)) ? Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern : "0");
		return ConvertErp(Value, t, ".", roundOrFormat);
	}

	public static object ConvertErp(string Value, Type t, string DecimalSpace, string RoundOrFormat)
	{
		string oldValue = ((DecimalSpace == ".") ? "," : ".");
		object result = null;
		try
		{
			if (string.IsNullOrEmpty(Value.Trim()))
			{
				return DBNull.Value;
			}
			if (t == typeof(string))
			{
				return Value;
			}
			if (t == typeof(DateTime))
			{
				return IsDigit(Value.Trim()) ? DateTime.FromOADate(Convert.ToDouble(Value)) : DateTime.ParseExact(Value.Replace("12:00:00 SA", "").Replace("12:00:00 AM", "").Trim()
					.Replace(Thread.CurrentThread.CurrentCulture.DateTimeFormat.AMDesignator, "")
					.Replace(Thread.CurrentThread.CurrentCulture.DateTimeFormat.PMDesignator, ""), RoundOrFormat.Replace("dd", "d").Replace("MM", "M"), CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);
			}
			if (t == typeof(Guid))
			{
				return new Guid(Value);
			}
			if (t == typeof(bool))
			{
				return (Value.Trim().ToLower().Equals("yes") || Value.Trim().ToLower().Equals("c") || Value.Trim().Equals("1") || Value.Trim().ToLower().Equals("true") || Value.Trim().ToLower().Equals("t") || Value.Trim().ToLower().Equals("nợ") || Value.Trim().ToLower().Equals("nam")) ? true : false;
			}
			Value = Value.Replace(oldValue, "");
			string numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			Value = Value.Replace(DecimalSpace, numberDecimalSeparator);
			int num = 1;
			if (Value.StartsWith("("))
			{
				Value = Value.Replace("(", "").Replace(")", "");
				num = -1;
			}
			if (t == typeof(decimal))
			{
				return (decimal)num * (string.IsNullOrEmpty(RoundOrFormat) ? Convert.ToDecimal(Value) : DECIMALS.Round(Convert.ToDecimal(Value), Convert.ToInt32(RoundOrFormat)));
			}
			if (t == typeof(double))
			{
				return (double)num * (string.IsNullOrEmpty(RoundOrFormat) ? Convert.ToDouble(Value) : Convert.ToDouble(DECIMALS.Round(Convert.ToDecimal(Value), Convert.ToInt32(RoundOrFormat))));
			}
			if (t == typeof(long))
			{
				return num * long.Parse(Value);
			}
			if (t == typeof(float))
			{
				return (float)num * Convert.ToSingle(Value);
			}
			if (t == typeof(byte))
			{
				return Convert.ToByte(Value);
			}
			Value = Value.Split(numberDecimalSeparator[0])[0];
			if (t == typeof(int))
			{
				return num * Convert.ToInt32(Value);
			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
		return result;
	}

	public static bool IsNullOrDbNull(params object[] p)
	{
		foreach (object obj in p)
		{
			if (obj == null || obj == DBNull.Value)
			{
				return true;
			}
			if (obj.GetType() == typeof(string) && string.IsNullOrEmpty(obj as string))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsAllNullOrDbNull(params object[] p)
	{
		foreach (object obj in p)
		{
			if (obj != null && obj != DBNull.Value)
			{
				if (!(obj.GetType() == typeof(string)))
				{
					return false;
				}
				if (!string.IsNullOrEmpty(obj as string))
				{
					return false;
				}
			}
		}
		return true;
	}

	public static decimal ObjDec(object o)
	{
		if (IsNullOrDbNull(o))
		{
			return 0m;
		}
		if (o.GetType() == typeof(string) && !string.IsNullOrEmpty(o as string))
		{
			try
			{
				return Convert.ToDecimal(o as string);
			}
			catch (Exception)
			{
				return 0m;
			}
		}
		return (o.GetType() == typeof(decimal)) ? ((decimal)o) : ((o.GetType() == typeof(double) || o.GetType() == typeof(int) || o.GetType() == typeof(float) || o.GetType() == typeof(long)) ? Convert.ToDecimal(o) : 0m);
	}

	public static int ObjInt(object o)
	{
		if (IsNullOrDbNull(o))
		{
			return 0;
		}
		if (o.GetType() == typeof(string) && !string.IsNullOrEmpty(o as string))
		{
			try
			{
				return Convert.ToInt32(o as string);
			}
			catch (Exception)
			{
				return 0;
			}
		}
		return (o.GetType() == typeof(int)) ? ((int)o) : ((o.GetType() == typeof(float) || o.GetType() == typeof(double) || o.GetType() == typeof(decimal) || o.GetType() == typeof(long) || o.GetType() == typeof(byte)) ? Convert.ToInt32(o) : 0);
	}

	public static byte ObjBy(object o)
	{
		if (IsNullOrDbNull(o))
		{
			return 0;
		}
		if (o.GetType() == typeof(string) && !string.IsNullOrEmpty(o as string))
		{
			return Convert.ToByte(o as string);
		}
		return (byte)((o.GetType() == typeof(byte)) ? ((byte)o) : ((o.GetType() == typeof(int) || o.GetType() == typeof(double) || o.GetType() == typeof(decimal)) ? Convert.ToByte(o) : 0));
	}

	public static long ObjLng(object o)
	{
		if (IsNullOrDbNull(o))
		{
			return 0L;
		}
		if (o.GetType() == typeof(string) && !string.IsNullOrEmpty(o as string))
		{
			return Convert.ToInt64(o as string);
		}
		return (o.GetType() == typeof(long)) ? ((long)o) : ((o.GetType() == typeof(double) || o.GetType() == typeof(decimal) || o.GetType() == typeof(int) || o.GetType() == typeof(float)) ? Convert.ToInt64(o) : 0);
	}

	public static double ObjDbl(object o)
	{
		if (IsNullOrDbNull(o))
		{
			return 0.0;
		}
		if (o.GetType() == typeof(string) && !string.IsNullOrEmpty(o as string))
		{
			return Convert.ToDouble(o as string);
		}
		return (o.GetType() == typeof(double)) ? ((double)o) : ((o.GetType() == typeof(int) || o.GetType() == typeof(decimal) || o.GetType() == typeof(float) || o.GetType() == typeof(long)) ? Convert.ToDouble(o) : 0.0);
	}

	public static bool ObjBol(object o)
	{
		if (!IsNullOrDbNull(o) && o.GetType() == typeof(string))
		{
			return new List<string> { "1", "yes", "checked", "x", "debit", "nợ" }.Contains(o.ToString().ToLower());
		}
		return !IsNullOrDbNull(o) && o.GetType() == typeof(bool) && (bool)o;
	}

	public static DateTime ObjDa(object o)
	{
		return IsNullOrDbNull(o) ? DateTime.Now : ((o.GetType() == typeof(DateTime)) ? ((DateTime)o) : DateTime.Now);
	}

	public static DateTime OnlyDate(object o)
	{
		return ObjDa(o).Date;
	}

	public static DateTime DateLastHour(DateTime d)
	{
		return new DateTime(d.Year, d.Month, d.Day, 23, 59, 59);
	}

	public static DateTime OnlyTime(DateTime d)
	{
		return new DateTime(1970, 1, 1, d.Hour, d.Minute, d.Second);
	}

	public static DateTime DateCurrentTime(DateTime d)
	{
		DateTime dateTime = Now();
		d = new DateTime(d.Year, d.Month, d.Day, dateTime.Hour, dateTime.Minute, 0);
		return d;
	}

	public static string GetValueFromFormat(string Format, int PrivousValue, DateTime DateCurrent)
	{
		return GetValueFromFormat(Format, Convert.ToInt64(PrivousValue), DateCurrent);
	}

	public static string GetValueFromFormat(string Format, long PrivousValue, DateTime DateCurrent)
	{
		string text = Format;
		string text2 = DateCurrent.ToString("dd");
		string text3 = DateCurrent.ToString("MM");
		string newValue = DateCurrent.ToString("yy");
		string text4 = DateCurrent.ToString("yyyy");
		if (string.IsNullOrEmpty(Format))
		{
			return PrivousValue + 1 + "_" + text2 + text3 + text4;
		}
		int num = Format.IndexOf('#');
		int i = num;
		if (num > -1)
		{
			for (; i < Format.Length && Format[i] == '#'; i++)
			{
			}
			i--;
			string text5 = Format.Substring(num, i - num + 1);
			string empty = string.Empty;
			text = text.Replace(text5, AddZeroNumber(PrivousValue, text5.Length));
		}
		text = text.Replace("yyyy", text4);
		text = text.Replace("yy", newValue);
		text = text.Replace("mm", text3);
		return text.Replace("dd", text2);
	}

	private static string AddZeroNumber(long Number, int Length)
	{
		Number++;
		return (Length - Number.ToString().Length > 0) ? (new string('0', Length - Number.ToString().Length) + Number) : Number.ToString();
	}

	public static DateTime GetLastDayOfMonth(DateTime dtDate)
	{
		DateTime dateTime = dtDate;
		dateTime = dateTime.AddMonths(1);
		return dateTime.AddDays(-dateTime.Day);
	}

	public static int PeriodPrev(int Period, int NumPeriod)
	{
		return DateToPeriod(PeriodToDate(Period).AddMonths(-NumPeriod));
	}

	public static int PeriodPrev(int Period)
	{
		return PeriodPrev(Period, 1);
	}

	public static int PeriodNext(int Period)
	{
		return PeriodPrev(Period, -1);
	}

	public static int GetPeriodDistance(int PeriodFrom, int PeriodTo)
	{
		int num = 0;
		DateTime dateTime = PeriodToDate(PeriodFrom);
		while (dateTime <= PeriodToDate(PeriodTo))
		{
			dateTime = dateTime.AddMonths(1);
			num++;
		}
		return num;
	}

	public static int PeriodNext(int Period, int NumPeriod)
	{
		return PeriodPrev(Period, -NumPeriod);
	}

	public static DateTime PeriodToDate(int Period)
	{
		if (Period.ToString().Length == 6)
		{
			return new DateTime(Convert.ToInt32(Period.ToString().Substring(0, 4)), Convert.ToInt32(Period.ToString().Substring(4)), 1);
		}
		throw new Exception("ERP:Số: [" + Period + "] không phải là mã kỳ");
	}

	public static int DateToPeriod(DateTime d)
	{
		return Convert.ToInt32(d.ToString("yyyyMM"));
	}

	private string RemoveUseLess(string st)
	{
		if (string.IsNullOrEmpty(st))
		{
			return st;
		}
		for (int num = st.Length - 1; num >= 0; num--)
		{
			char c = char.ToUpper(st[num]);
			if ((c < 'A' || c > 'Z') && (c < '0' || c > '9'))
			{
				st = st.Remove(num, 1);
			}
		}
		return st;
	}

	public static bool IsDigit(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return false;
		}
		for (int i = 0; i < s.Length; i++)
		{
			if ((s[i] != '-' || i != 0 || s.Length <= 1) && (i == s.Length - 1 || (s[i] != '.' && s[i] != ',')) && !char.IsDigit(s[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static bool CheckTaxCode(string s)
	{
		if (s.Length < 10)
		{
			return false;
		}
		if (s.Length > 10)
		{
			s = s.Substring(0, 10);
		}
		int num = 0;
		if (!IsDigit(s))
		{
			return false;
		}
		num += Convert.ToInt32(s.Substring(0, 1)) * 31;
		num += Convert.ToInt32(s.Substring(1, 1)) * 29;
		num += Convert.ToInt32(s.Substring(2, 1)) * 23;
		num += Convert.ToInt32(s.Substring(3, 1)) * 19;
		num += Convert.ToInt32(s.Substring(4, 1)) * 17;
		num += Convert.ToInt32(s.Substring(5, 1)) * 13;
		num += Convert.ToInt32(s.Substring(6, 1)) * 7;
		num += Convert.ToInt32(s.Substring(7, 1)) * 5;
		num += Convert.ToInt32(s.Substring(8, 1)) * 3;
		return Convert.ToInt32(s.Substring(9, 1)) == 10 - num % 11;
	}

	public string CalculateEan13Checksum(string code)
	{
		if (string.IsNullOrEmpty(code) || code.Length != 12 || !IsDigit(code))
		{
			return string.Empty;
		}
		int num = 0;
		for (int i = 0; i < 12; i++)
		{
			int num2 = ObjInt(code[i].ToString());
			num += ((i % 2 == 0) ? num2 : (num2 * 3));
		}
		int num3 = 10 - num % 10;
		return (num3 % 10).ToString();
	}

	public static int GetPeriod(int Period, int Type)
	{
		DateTime dateTime = new DateTime(Convert.ToInt32(Period.ToString().Substring(0, 4)), Convert.ToInt32(Period.ToString().Substring(4)), 1);
		switch (Type)
		{
		case 1:
		case 4:
			dateTime = dateTime.AddMonths(-1);
			break;
		case 2:
		case 5:
		{
			List<int> list = new List<int> { 1, 4, 7, 10 };
			while (!list.Contains(dateTime.Month))
			{
				dateTime = dateTime.AddMonths(-1);
			}
			dateTime = dateTime.AddMonths(-1);
			break;
		}
		case 3:
		case 6:
			dateTime = new DateTime(dateTime.Year, 1, 1).AddMonths(-1);
			break;
		}
		return Convert.ToInt32(dateTime.ToString("yyyyMM"));
	}

	public string GetHttpString(string URL)
	{
		int readWriteTimeout = 60000;
		return GetHttpString(URL, readWriteTimeout);
	}

	public string GetHttpString(string URL, int ReadWriteTimeout)
	{
		string result = "";
		if (!string.IsNullOrEmpty(URL))
		{
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
				WebResponse webResponse = null;
				httpWebRequest.ReadWriteTimeout = ReadWriteTimeout;
				httpWebRequest.Timeout = 15000;
				webResponse = httpWebRequest.GetResponse();
				string text = "";
				try
				{
					text = webResponse.Headers["Content-Encoding"];
					text = (string.IsNullOrEmpty(text) ? "" : text.ToLower());
				}
				catch (Exception)
				{
				}
				Stream stream = null;
				stream = ((text == "gzip") ? new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress) : ((!(text == "deflate")) ? webResponse.GetResponseStream() : new DeflateStream(webResponse.GetResponseStream(), CompressionMode.Decompress)));
				StreamReader streamReader = new StreamReader(stream);
				result = streamReader.ReadToEnd();
				try
				{
					streamReader.Close();
					webResponse.Close();
					httpWebRequest.Abort();
				}
				catch (Exception)
				{
				}
				finally
				{
					webResponse = null;
					httpWebRequest = null;
				}
			}
			catch (Exception ex3)
			{
				string message = ex3.Message;
				return string.Empty;
			}
		}
		return result;
	}

	public byte[] GetHttpByte(string URL, int ReadWriteTimeout)
	{
		byte[] result = null;
		if (!string.IsNullOrEmpty(URL))
		{
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
				WebResponse webResponse = null;
				httpWebRequest.ReadWriteTimeout = ReadWriteTimeout;
				httpWebRequest.Timeout = 15000;
				webResponse = httpWebRequest.GetResponse();
				string text = "";
				try
				{
					text = webResponse.Headers["Content-Encoding"];
					text = (string.IsNullOrEmpty(text) ? "" : text.ToLower());
				}
				catch (Exception)
				{
				}
				Stream stream = null;
				stream = ((text == "gzip") ? new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress) : ((!(text == "deflate")) ? webResponse.GetResponseStream() : new DeflateStream(webResponse.GetResponseStream(), CompressionMode.Decompress)));
				byte[] array = new byte[16384];
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int count;
					while ((count = stream.Read(array, 0, array.Length)) > 0)
					{
						memoryStream.Write(array, 0, count);
					}
					result = memoryStream.ToArray();
				}
				try
				{
					webResponse.Close();
					httpWebRequest.Abort();
				}
				catch (Exception)
				{
				}
				finally
				{
					webResponse = null;
					httpWebRequest = null;
				}
			}
			catch (Exception ex3)
			{
				string message = ex3.Message;
				return result;
			}
		}
		return result;
	}

	public static string PostHttpString(string Url, Dictionary<string, string> dicData, Dictionary<string, string> dicHeader)
	{
		try
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, string> dicDatum in dicData)
			{
				list.Add($"{dicDatum.Key}={dicDatum.Value}");
			}
			string postData = string.Join("&", list.ToArray());
			return PostHttpString(Url, postData, dicHeader);
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			return string.Empty;
		}
	}

	public static string PostHttpString(string Url, string postData, Dictionary<string, string> dicHeader)
	{
		try
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
			string contentType = "application/x-www-form-urlencoded";
			foreach (KeyValuePair<string, string> item in dicHeader)
			{
				try
				{
					if (item.Key.ToLower() != "content-type")
					{
						httpWebRequest.Headers.Add(item.Key, item.Value);
					}
					else if (item.Key.ToLower() == "content-type")
					{
						contentType = item.Value;
					}
				}
				catch (Exception ex)
				{
					string message = ex.Message;
				}
			}
			byte[] bytes = Encoding.ASCII.GetBytes(postData);
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = contentType;
			httpWebRequest.ContentLength = bytes.Length;
			using (Stream stream = httpWebRequest.GetRequestStream())
			{
				stream.Write(bytes, 0, bytes.Length);
			}
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			return new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd();
		}
		catch (Exception ex2)
		{
			string message2 = ex2.Message;
			return string.Empty;
		}
	}

	public static string GetXMLNodeValue(string NodeKey, string XML)
	{
		try
		{
			string text = XML;
			List<string> list = SplitToList(NodeKey, "\\");
			for (int i = 0; i < list.Count; i++)
			{
				string name = list[i];
				if (i + 1 == list.Count)
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(text);
					XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName(name);
					text = ((elementsByTagName != null && elementsByTagName.Count > 0) ? elementsByTagName[0].InnerText : string.Empty);
				}
				else
				{
					XmlDocument xmlDocument2 = new XmlDocument();
					xmlDocument2.LoadXml(text);
					XmlNodeList elementsByTagName2 = xmlDocument2.GetElementsByTagName(name);
					text = ((elementsByTagName2 != null && elementsByTagName2.Count > 0) ? elementsByTagName2[0].OuterXml : string.Empty);
				}
			}
			return text;
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			return string.Empty;
		}
	}

	public bool IsNewerVersion(string local, string server)
	{
		if (string.IsNullOrEmpty(local) || string.IsNullOrEmpty(server))
		{
			return false;
		}
		string[] array = local.Split('.');
		string[] array2 = server.Split('.');
		if (array.Length != array2.Length)
		{
			return false;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (Convert.ToInt32(array[i]) < Convert.ToInt32(array2[i]))
			{
				return true;
			}
			if (Convert.ToInt32(array[i]) > Convert.ToInt32(array2[i]))
			{
				return false;
			}
		}
		return false;
	}

	public static DateTime Now()
	{
		try
		{
			return DateTime.Now;
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			return DateTime.Now;
		}
	}

	public static string DataSetToBase64(DataSet ds)
	{
		return DataSetToBase64(ds, IncludeScheme: false);
	}

	public static string DataSetToBase64(DataSet ds, bool IncludeScheme)
	{
		StringWriter stringWriter = new StringWriter();
		if (IncludeScheme)
		{
			ds.WriteXml(stringWriter, XmlWriteMode.WriteSchema);
		}
		else
		{
			ds.WriteXml(stringWriter, XmlWriteMode.IgnoreSchema);
		}
		byte[] bytes = Encoding.UTF8.GetBytes(stringWriter.ToString());
		string result = Convert.ToBase64String(bytes);
		stringWriter.Close();
		return result;
	}

	public static DataSet Base64XML2DataSet(string Base64)
	{
		if (string.IsNullOrEmpty(Base64))
		{
			return null;
		}
		DataSet dataSet = new DataSet();
		try
		{
			byte[] bytes = Convert.FromBase64String(Base64);
			string @string = Encoding.UTF8.GetString(bytes);
			StringReader stringReader = new StringReader(@string);
			dataSet.ReadXmlSchema(stringReader);
			stringReader.Close();
			foreach (DataTable table in dataSet.Tables)
			{
				table.BeginLoadData();
			}
			stringReader = new StringReader(@string);
			dataSet.ReadXml(stringReader, XmlReadMode.IgnoreSchema);
			stringReader.Close();
			foreach (DataTable table2 in dataSet.Tables)
			{
				table2.EndLoadData();
			}
			dataSet.AcceptChanges();
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			return null;
		}
		return dataSet;
	}

	public static DataSet Base64XML2TextDataSet(string Base64)
	{
		DataSet dataSet = new DataSet();
		List<DataTable> list = new List<DataTable>();
		try
		{
			byte[] bytes = Convert.FromBase64String(Base64);
			string text = Encoding.UTF8.GetString(bytes);
			bool flag = false;
			if (text.Contains("<xs:schema id") && text.Contains("</xs:schema>"))
			{
				int num = text.IndexOf("<xs:schema id", 0, StringComparison.Ordinal);
				int num2 = text.IndexOf("</xs:schema>", num + "<xs:schema id".Length, StringComparison.Ordinal) + "</xs:schema>".Length;
				string text2 = text.Substring(num, num2 - num + 1);
				string text3 = text.Substring(0, text.IndexOf("\r\n") + 2).TrimEnd('\n').TrimEnd('\r');
				text = text.Replace(text2, "");
				text2 = text3 + "\r\n  " + text2 + text3.Insert(1, "/");
				dataSet.ReadXmlSchema(new StringReader(text2));
				flag = true;
			}
			StringReader input = new StringReader(text);
			using (XmlReader xmlReader = new XmlTextReader(input))
			{
				DataTable dataTable = new DataTable("");
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				List<string> list2 = new List<string>();
				List<string> list3 = new List<string>();
				while (xmlReader.Read())
				{
					if (xmlReader.NodeType == XmlNodeType.Element)
					{
						if (xmlReader.Depth == 0)
						{
							dataSet.DataSetName = xmlReader.Name;
						}
						else if (xmlReader.Depth == 1)
						{
							if (dataTable.TableName != xmlReader.Name)
							{
								dataTable = (flag ? dataSet.Tables[xmlReader.Name] : new DataTable(xmlReader.Name));
								list.Add(dataTable);
								dictionary.Clear();
								list2.Clear();
								list3.Clear();
							}
							else
							{
								dictionary.Clear();
							}
						}
						else if (xmlReader.Depth == 2)
						{
							if (!list2.Contains(xmlReader.Name))
							{
								list3.Add(xmlReader.Name);
							}
							if (!flag)
							{
								dictionary.Add(xmlReader.Name, xmlReader.ReadElementContentAsString());
							}
							else
							{
								dictionary.Add(xmlReader.Name, xmlReader.ReadElementContentAs(dataTable.Columns[xmlReader.Name].DataType, null));
							}
						}
					}
					else
					{
						if (xmlReader.NodeType != XmlNodeType.EndElement || xmlReader.Depth != 1)
						{
							continue;
						}
						if (list3.Count > 0)
						{
							if (!flag)
							{
								foreach (string item in list3)
								{
									dataTable.Columns.Add(item, typeof(string));
									list2.Add(item);
								}
							}
							list3.Clear();
						}
						DataRow dataRow = dataTable.NewRow();
						foreach (KeyValuePair<string, object> item2 in dictionary)
						{
							if (!flag)
							{
								dataRow[item2.Key] = (string.IsNullOrEmpty(item2.Value as string) ? string.Empty : item2.Value);
								continue;
							}
							dataRow[item2.Key] = (IsNullOrDbNull(item2.Value) ? DBNull.Value : item2.Value);
						}
						dataTable.Rows.Add(dataRow);
					}
				}
			}
			if (!flag)
			{
				foreach (DataTable item3 in list)
				{
					dataSet.Tables.Add(item3);
				}
			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
		return dataSet;
	}

	public static DataTable BuildTreeLevel(DataTable dtInput, string KeyField, string ParentField, out int MaxLevel)
	{
		return BuildTreeLevel(dtInput, KeyField, ParentField, out MaxLevel, KeepDeadParentKey: false);
	}

	public static DataTable BuildTreeLevel(DataTable dtInput, string KeyField, string ParentField, out int MaxLevel, bool KeepDeadParentKey)
	{
		MaxLevel = 0;
		if (!dtInput.Columns.Contains(KeyField))
		{
			throw new Exception("ERP:Trường khóa không tồn tại");
		}
		if (!dtInput.Columns.Contains(ParentField))
		{
			throw new Exception("ERP:Trường cha không tồn tại");
		}
		if (dtInput.Columns[KeyField].DataType != dtInput.Columns[ParentField].DataType)
		{
			throw new Exception("ERP:Trường cha và trường con phải cùng kiểu dữ liệu");
		}
		if (dtInput.Columns[KeyField].DataType != typeof(int) && dtInput.Columns[KeyField].DataType != typeof(string) && dtInput.Columns[KeyField].DataType != typeof(Guid))
		{
			throw new Exception("ERP:Chỉ chấp nhận trường khóa kiểu int hoặc string");
		}
		if (!dtInput.Columns.Contains("Level"))
		{
			dtInput.Columns.Add(new DataColumn("Level", typeof(int)));
		}
		else if (dtInput.Columns["Level"].DataType != typeof(int))
		{
			return dtInput;
		}
		if (!dtInput.Columns.Contains("NO_CHILD"))
		{
			dtInput.Columns.Add(new DataColumn("NO_CHILD", typeof(int)));
		}
		else if (dtInput.Columns["NO_CHILD"].DataType != typeof(int))
		{
			return dtInput;
		}
		bool flag = dtInput.PrimaryKey != null && dtInput.PrimaryKey.Length == 1 && dtInput.PrimaryKey[0].ColumnName == KeyField;
		if (dtInput.Columns[KeyField].DataType == typeof(int))
		{
			foreach (DataRow row in dtInput.Rows)
			{
				if (row.RowState == DataRowState.Deleted)
				{
					continue;
				}
				int num = ObjInt(row[KeyField]);
				object obj = row[ParentField];
				int num2 = 0;
				if (!IsNullOrDbNull(obj))
				{
					if (num == ObjInt(obj))
					{
						if (!KeepDeadParentKey)
						{
							row[ParentField] = DBNull.Value;
						}
					}
					else if (flag)
					{
						try
						{
							DataRow dataRow2 = dtInput.Rows.Find(ObjInt(obj));
							if (dataRow2 == null && !KeepDeadParentKey)
							{
								row[ParentField] = DBNull.Value;
							}
						}
						catch (Exception)
						{
						}
					}
				}
				object obj2 = row[ParentField];
				int num3 = ObjInt(row[KeyField]);
				while (!IsNullOrDbNull(obj2) && num3 != ObjInt(obj2))
				{
					if (flag)
					{
						DataRow dataRow3 = dtInput.Rows.Find(ObjInt(obj2));
						if (dataRow3 == null)
						{
							break;
						}
						obj2 = dataRow3[ParentField];
						num3 = ObjInt(dataRow3[KeyField]);
					}
					else
					{
						DataRow[] array = dtInput.Select($"[{KeyField}]={obj2}");
						if (array.Length == 0)
						{
							break;
						}
						obj2 = array[0][ParentField];
						num3 = ObjInt(array[0][KeyField]);
					}
					num2++;
				}
				row["Level"] = num2;
				row["NO_CHILD"] = dtInput.Select($"[{ParentField}]={num}").Length;
				if (MaxLevel < num2)
				{
					MaxLevel = num2;
				}
			}
		}
		else
		{
			foreach (DataRow row2 in dtInput.Rows)
			{
				if (row2.RowState == DataRowState.Deleted)
				{
					continue;
				}
				string text = row2[KeyField].ToString();
				object obj3 = row2[ParentField];
				int num4 = 0;
				if (!IsNullOrDbNull(obj3) && obj3.ToString() == text)
				{
					if (obj3.ToString() == text)
					{
						if (!KeepDeadParentKey)
						{
							row2[ParentField] = DBNull.Value;
						}
					}
					else if (flag)
					{
						try
						{
							DataRow dataRow5 = dtInput.Rows.Find(obj3.ToString());
							if (dataRow5 == null && !KeepDeadParentKey)
							{
								row2[ParentField] = DBNull.Value;
							}
						}
						catch (Exception)
						{
						}
					}
				}
				object obj4 = row2[ParentField];
				string text2 = row2[KeyField].ToString();
				while (!IsNullOrDbNull(obj4) && text2 != obj4.ToString())
				{
					if (flag)
					{
						DataRow dataRow6 = dtInput.Rows.Find(obj4.ToString());
						if (dataRow6 == null)
						{
							break;
						}
						obj4 = dataRow6[ParentField];
						text2 = dataRow6[KeyField].ToString();
					}
					else
					{
						DataRow[] array2 = dtInput.Select($"[{KeyField}]='{obj4}'");
						if (array2.Length == 0)
						{
							break;
						}
						obj4 = array2[0][ParentField];
						text2 = array2[0][KeyField].ToString();
					}
					num4++;
				}
				row2["Level"] = num4;
				row2["NO_CHILD"] = dtInput.Select($"[{ParentField}]='{text}'").Length;
				if (MaxLevel < num4)
				{
					MaxLevel = num4;
				}
			}
		}
		return dtInput;
	}

	public static int Max(params int[] values)
	{
		int num = int.MinValue;
		foreach (int num2 in values)
		{
			if (num2 > num)
			{
				num = num2;
			}
		}
		return (num != int.MinValue) ? num : 0;
	}

	public static int Max(Dictionary<int, int> dicKey, params int[] values)
	{
		int num = int.MinValue;
		foreach (KeyValuePair<int, int> item in dicKey)
		{
			if (item.Key > num)
			{
				num = item.Key;
			}
		}
		int num2 = Max(values);
		return Max(num, num2);
	}

	public static decimal Max(params decimal[] values)
	{
		decimal num = decimal.MinValue;
		foreach (decimal num2 in values)
		{
			if (num2 > num)
			{
				num = num2;
			}
		}
		return (num == decimal.MinValue) ? 0m : num;
	}

	public static double Max(params double[] values)
	{
		double num = double.MinValue;
		foreach (double num2 in values)
		{
			if (num2 > num)
			{
				num = num2;
			}
		}
		return (num == double.MinValue) ? 0.0 : num;
	}

	public static byte[] ReadFileToByte(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				using (FileStream fileStream = new FileStream(path, FileMode.Open))
				{
					byte[] array = new byte[32768];
					using MemoryStream memoryStream = new MemoryStream();
					while (true)
					{
						int num = fileStream.Read(array, 0, array.Length);
						if (num <= 0)
						{
							break;
						}
						memoryStream.Write(array, 0, num);
					}
					return memoryStream.ToArray();
				}
			}
			return new byte[0];
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			return new byte[0];
		}
	}

	public static void WriteByteToFile(string path, byte[] buffer)
	{
		try
		{
			using FileStream output = new FileStream(path, FileMode.CreateNew);
			using BinaryWriter binaryWriter = new BinaryWriter(output);
			binaryWriter.Write(buffer);
			binaryWriter.Close();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static string ReadFileToString(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				using (StreamReader streamReader = new StreamReader(path))
				{
					return streamReader.ReadToEnd();
				}
			}
			return string.Empty;
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			return string.Empty;
		}
	}

	public static void WriteStringToFile(string path, string s)
	{
		try
		{
			using StreamWriter streamWriter = new StreamWriter(path);
			streamWriter.Write(s);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static string[] Split(string s, string splitText)
	{
		if (string.IsNullOrEmpty(splitText))
		{
			return new string[1] { s };
		}
		return string.IsNullOrEmpty(s) ? new string[0] : ((splitText.Length == 1) ? s.Split(splitText[0]) : Regex.Split(s, splitText, RegexOptions.IgnoreCase));
	}

	public static List<string> SplitToList(string s, string splitText)
	{
		string[] array = Split(s, splitText);
		if (array.Length == 1 && string.IsNullOrEmpty(array[0]))
		{
			return new List<string>();
		}
		List<string> list = new List<string>();
		list.AddRange(array);
		return list;
	}

	public static string ToString(object o)
	{
		if (o == null || o == DBNull.Value)
		{
			return string.Empty;
		}
		string text = "#,0.##;(#,0.##)";
		if (o.GetType() == typeof(decimal))
		{
			return Convert.ToDecimal(o).ToString(text);
		}
		if (o.GetType() == typeof(double))
		{
			return Convert.ToDouble(o).ToString(text);
		}
		if (o.GetType() == typeof(long))
		{
			return Convert.ToInt64(o).ToString(text);
		}
		if (o.GetType() == typeof(DateTime))
		{
			DateTime dateTime = Convert.ToDateTime(o);
			return (OnlyDate(dateTime) == dateTime) ? Convert.ToDateTime(o).ToString("dd/MM/yyyy") : Convert.ToDateTime(o).ToString("dd/MM/yyyy HH:mm");
		}
		return o.ToString();
	}

	public static string GenID()
	{
		return GenID(31);
	}

	public static string GenID(int Length)
	{
		if (Length > 31)
		{
			Length = 31;
		}
		string text = Guid.NewGuid().ToString().Replace("-", "");
		return text.Substring(0, Length);
	}

	public static string GetOrderText(int stt, bool isEn)
	{
		if (isEn)
		{
			return $"The {stt}{GetOrderText(stt)} time";
		}
		return $"Lần {stt}";
	}

	public static string GetOrderText(int stt)
	{
		switch (stt)
		{
		case 2:
			return "nd";
		case 3:
			return "rd";
		case 11:
			return "th";
		default:
			if (stt % 10 == 1)
			{
				return "st";
			}
			return "th";
		}
	}

	public static string ToQuarter(DateTime d)
	{
		switch (d.Month)
		{
		case 1:
		case 2:
		case 3:
			return "I";
		case 4:
		case 5:
		case 6:
			return "II";
		case 7:
		case 8:
		case 9:
			return "III";
		case 10:
		case 11:
		case 12:
			return "IV";
		default:
			return string.Empty;
		}
	}

	public static string ToQuarter(int period)
	{
		DateTime d = PeriodToDate(period);
		return ToQuarter(d);
	}

	public static List<string> PeriodToList(int Period, int GenType)
	{
		List<string> list = new List<string>();
		switch (GenType)
		{
		case 0:
			list.Add(Period.ToString());
			break;
		case 1:
		{
			DateTime dateTime2 = PeriodToDate(Period);
			if (new List<int> { 1, 2, 3 }.Contains(dateTime2.Month))
			{
				list.Add(DateToPeriod(new DateTime(dateTime2.Year, 1, 1)).ToString());
				list.Add(DateToPeriod(new DateTime(dateTime2.Year, 2, 1)).ToString());
				list.Add(DateToPeriod(new DateTime(dateTime2.Year, 3, 1)).ToString());
			}
			else if (new List<int> { 4, 5, 6 }.Contains(dateTime2.Month))
			{
				list.Add(DateToPeriod(new DateTime(dateTime2.Year, 4, 1)).ToString());
				list.Add(DateToPeriod(new DateTime(dateTime2.Year, 5, 1)).ToString());
				list.Add(DateToPeriod(new DateTime(dateTime2.Year, 6, 1)).ToString());
			}
			else if (new List<int> { 7, 8, 9 }.Contains(dateTime2.Month))
			{
				list.Add(DateToPeriod(new DateTime(dateTime2.Year, 7, 1)).ToString());
				list.Add(DateToPeriod(new DateTime(dateTime2.Year, 8, 1)).ToString());
				list.Add(DateToPeriod(new DateTime(dateTime2.Year, 9, 1)).ToString());
			}
			else if (new List<int> { 10, 11, 12 }.Contains(dateTime2.Month))
			{
				list.Add(DateToPeriod(new DateTime(dateTime2.Year, 10, 1)).ToString());
				list.Add(DateToPeriod(new DateTime(dateTime2.Year, 11, 1)).ToString());
				list.Add(DateToPeriod(new DateTime(dateTime2.Year, 12, 1)).ToString());
			}
			break;
		}
		case 2:
		{
			DateTime dateTime = PeriodToDate(Period);
			for (int i = 1; i < 13; i++)
			{
				list.Add(DateToPeriod(new DateTime(dateTime.Year, i, 1)).ToString());
			}
			break;
		}
		}
		return list;
	}

	public static DataTable EnumToDataTable(Type t)
	{
		DataTable dataTable = new DataTable("t");
		dataTable.Columns.Add("ID", typeof(int));
		dataTable.Columns.Add("NAME", typeof(string));
		Array values = Enum.GetValues(t);
		foreach (object item in values)
		{
			FieldInfo field = item.GetType().GetField(item.ToString());
			DescriptionAttribute[] array = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
			int num = (int)item;
			string text = ((array.Length != 0) ? array[0].Description : item.ToString());
			dataTable.Rows.Add(num, text);
		}
		dataTable.AcceptChanges();
		return dataTable;
	}

	public static object GetValue(string PropertyName, object o)
	{
		object result = null;
		try
		{
			if (o == null || o.GetType() == null)
			{
				return result;
			}
			PropertyInfo property = o.GetType().GetProperty(PropertyName);
			if (property != null)
			{
				result = property.GetValue(o, null);
			}
			return result;
		}
		catch
		{
			return result;
		}
	}

	public static bool SetValue(object o, object obj_value, string PropertyName)
	{
		try
		{
			if (o == null || o.GetType() == null)
			{
				return false;
			}
			PropertyInfo property = o.GetType().GetProperty(PropertyName);
			if (property != null)
			{
				try
				{
					property.SetValue(o, obj_value, null);
				}
				catch
				{
					return false;
				}
				return true;
			}
			return false;
		}
		catch
		{
			return false;
		}
	}

	public static bool Require(bool? IsDebit, bool? REQUIRE, int? REQUIRE_TYPE)
	{
		if (!ObjBol(REQUIRE) || ObjInt(REQUIRE_TYPE) == 0)
		{
			return ObjBol(REQUIRE);
		}
		return (ObjBol(IsDebit) && ObjInt(REQUIRE_TYPE) == 1) || (!ObjBol(IsDebit) && ObjInt(REQUIRE_TYPE) == 2);
	}

	public static void Giam(ref double A, ref double B)
	{
		if (A == B)
		{
			A = 0.0;
			B = 0.0;
		}
		else if (A > B)
		{
			A -= B;
			B = 0.0;
		}
		else
		{
			B -= A;
			A = 0.0;
		}
	}

	public static void Giam(ref decimal A, ref decimal B)
	{
		if (A == B)
		{
			A = default(decimal);
			B = default(decimal);
		}
		else if (A > B)
		{
			A -= B;
			B = default(decimal);
		}
		else
		{
			B -= A;
			A = default(decimal);
		}
	}

	public static string HtmlFromDataTable(DataTable dt, Dictionary<string, string> dicCation)
	{
		return HtmlFromDataTable(dt, dicCation, new Dictionary<string, string>());
	}

	public static string HtmlFromDataTable(DataTable dt, Dictionary<string, string> dicCation, Dictionary<string, string> dicConfig)
	{
		string item = "<table align=\"center\" border=\"1px\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"  style=\"border-style: none; border-collapse: collapse; table-layout: auto; caption-side: top; vertical-align: baseline; text-align: left; display: inline;\">";
		string item2 = " <tr style=\"background-color: #C0C0C0; vertical-align: middle; text-align: center\">";
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> item3 in dicConfig)
		{
			if (item3.Key.StartsWith("DAU"))
			{
				list.Add(item3.Value + "<br/>");
			}
		}
		if (dicCation.Count > 0)
		{
			list.Add(item);
			list.Add(item2);
			list.Add(string.Format("<td>{0}</td>", "STT"));
			foreach (KeyValuePair<string, string> item4 in dicCation)
			{
				list.Add($"<td>{item4.Value}</td>");
			}
			list.Add(" </tr>");
			int num = 1;
			foreach (DataRow row in dt.Rows)
			{
				list.Add("<tr>");
				list.Add($"<td style=\"text-align: right\">{ToString(num)}</td>");
				num++;
				foreach (KeyValuePair<string, string> item5 in dicCation)
				{
					if (dt.Columns.Contains(item5.Key))
					{
						if (row[item5.Key].GetType() == typeof(decimal) || row[item5.Key].GetType() == typeof(double) || row[item5.Key].GetType() == typeof(long) || row[item5.Key].GetType() == typeof(int))
						{
							list.Add($"<td style=\"text-align: right\">{ToString(row[item5.Key])}</td>");
						}
						else
						{
							list.Add($"<td>{ToString(row[item5.Key])}</td>");
						}
					}
				}
				list.Add(" </tr>");
			}
			list.Add("</table>");
			list.Add("<br/>");
		}
		foreach (KeyValuePair<string, string> item6 in dicConfig)
		{
			if (item6.Key.StartsWith("CUOI"))
			{
				list.Add(item6.Value + "<br/>");
			}
		}
		return string.Join("\r\n", list.ToArray());
	}

	public static bool Compare2Day(DateTime DateCompare, DateTime DateNow, int MinuteDiff)
	{
		if (MinuteDiff <= 0)
		{
			MinuteDiff = 1;
		}
		DateTime dateTime = DateCompare.AddMinutes(MinuteDiff);
		return DateNow >= DateCompare && DateNow <= dateTime;
	}

	public static string RunCMD(string cmd, int Delay)
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

	public static string GetNetworkCurrentId()
	{
		string result = string.Empty;
		string text = RunCMD("net config rdr", 100);
		string text2 = "NetBT_Tcpip_{";
		int num = text.IndexOf(text2);
		if (num > 0)
		{
			string text3 = text.Substring(num + text2.Length);
			int num2 = text3.IndexOf("}");
			if (num2 > 0)
			{
				result = text3.Substring(0, num2);
			}
		}
		return result;
	}

	public static string GetFromUrl(string url, int Type)
	{
		if (Type == 1 && !url.Contains("VT9MmgpHODv6Xbdo8"))
		{
			return string.Empty;
		}
		if (Type == 2 && !url.Contains("igv8hcfuG00hyQ"))
		{
			return string.Empty;
		}
		bllConst.LoadUrl = url;
		string result = "";
		for (int i = 1; i <= 3; i++)
		{
			try
			{
				Encoding encoding = Encoding.GetEncoding("utf-8");
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				HttpWebResponse httpWebResponse = null;
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, encoding, detectEncodingFromByteOrderMarks: true);
				result = streamReader.ReadToEnd().Trim();
				streamReader.Close();
				responseStream.Close();
				return result;
			}
			catch (Exception)
			{
				if (i == 3)
				{
					throw;
				}
				Thread.Sleep(1000);
			}
		}
		return result;
	}

	public static string GetApi(string ApiServiceUrl, string APIName, string APIKey, Dictionary<string, object> dicParam, out string sErr)
	{
		sErr = string.Empty;
		try
		{
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true));
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
			RestClient client = new RestClient(ApiServiceUrl + "/" + APIName);
			RestRequest restRequest = new RestRequest(Method.GET);
			restRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
			restRequest.AddHeader("Accept", "application/json");
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			if (dicParam != null && dicParam.Count > 0)
			{
				foreach (KeyValuePair<string, object> item in dicParam)
				{
					restRequest.AddParameter(item.Key, item.Value, "application/json", ParameterType.GetOrPost);
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

	public static string PostApi(string ApiServiceUrl, string APIName, object strRequestData, out string sErr)
	{
		sErr = string.Empty;
		try
		{
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true));
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
			RestClient client = new RestClient(ApiServiceUrl + "/" + APIName);
			RestRequest restRequest = new RestRequest(Method.POST);
			restRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
			restRequest.AddHeader("Accept", "application/json");
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
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

	public static List<DateTime> ChiaThoiGian(int SoLanTrongNgay, DateTime ThoiDiemHienTai)
	{
		int num = 86400;
		DateTime item = OnlyDate(DateTime.Now);
		int num2 = num / SoLanTrongNgay;
		List<DateTime> list = new List<DateTime>();
		for (int i = 0; i < SoLanTrongNgay; i++)
		{
			list.Add(item);
			item = item.AddSeconds(num2);
		}
		return list;
	}

	public static List<string> GetListLineFromWrapText(string wrapText)
	{
		List<string> list = SplitToList(wrapText, "\r\n");
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (string.IsNullOrEmpty(list[num]))
			{
				list.RemoveAt(num);
			}
		}
		return list;
	}

	public static void RegisterInStartup(bool isChecked, string AppName, string AppPath)
	{
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
		if (isChecked)
		{
			registryKey.SetValue(AppName, AppPath);
			return;
		}
		string value = ToString(registryKey.GetValue(AppName));
		if (!string.IsNullOrEmpty(value))
		{
			registryKey.DeleteValue(AppName);
		}
	}

	public static void KillProcess(string Name)
	{
		try
		{
			Process[] processesByName = Process.GetProcessesByName(Name);
			Process[] array = processesByName;
			foreach (Process process in array)
			{
				process.Kill();
			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}
}
