using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace AutoUpdate;

public class Sec
{
	public object ConvertErp(string Value, Type t, string DecimalSpace, string RoundOrFormat)
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
				return DateTime.ParseExact(Value.Replace("12:00:00 SA", "").Replace("12:00:00 AM", "").Trim()
					.Replace(Thread.CurrentThread.CurrentCulture.DateTimeFormat.AMDesignator, "")
					.Replace(Thread.CurrentThread.CurrentCulture.DateTimeFormat.PMDesignator, ""), RoundOrFormat.Replace("dd", "d").Replace("MM", "M"), CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);
			}
			if (t == typeof(Guid))
			{
				return new Guid(Value);
			}
			if (t == typeof(bool))
			{
				return (Value.Trim().ToLower().Equals("yes") || Value.Trim().ToLower().Equals("c") || Value.Trim().Equals("1") || Value.Trim().ToLower().Equals("true") || Value.Trim().ToLower().Equals("t") || Value.Trim().ToLower().Equals("nợ")) ? true : false;
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
				return (decimal)num * Convert.ToDecimal(Value);
			}
			if (t == typeof(double))
			{
				return (double)num * Convert.ToDouble(Convert.ToDecimal(Value));
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

	public Guid GetMD5Guid(string s)
	{
		int num = 0;
		int num2 = ((num != 1) ? 7 : 0);
		int num3 = ((num != 1) ? 7 : 0);
		int num4 = ((num != 1) ? 7 : 0);
		try
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			byte[] b = mD5CryptoServiceProvider.ComputeHash(bytes, 0, bytes.Length);
			return new Guid(b);
		}
		catch
		{
			return Guid.Empty;
		}
	}

	public string Encrypt(string data)
	{
		int num = 0;
		int num2 = ((num != 1) ? 7 : 0);
		int num3 = ((num != 1) ? 7 : 0);
		int num4 = ((num != 1) ? 7 : 0);
		try
		{
			byte[] bytes = Convert.FromBase64String("Z2Fz");
			byte[] bytes2 = Convert.FromBase64String("Y2Fv");
			string @string = Encoding.UTF8.GetString(bytes);
			string string2 = Encoding.UTF8.GetString(bytes2);
			byte[] bytes3 = Encoding.ASCII.GetBytes(string2);
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(@string, bytes3, "MD5", 2);
			byte[] bytes4 = passwordDeriveBytes.GetBytes(16);
			byte[] bytes5 = passwordDeriveBytes.GetBytes(32);
			return a4(bytes5, bytes4, data);
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
		finally
		{
		}
		return string.Empty;
	}

	public string Encrypt(object o)
	{
		int num = 0;
		int num2 = ((num != 1) ? 7 : 0);
		int num3 = ((num != 1) ? 7 : 0);
		int num4 = ((num != 1) ? 7 : 0);
		return Encrypt(o.ToString());
	}

	private string a4(byte[] s7, byte[] s6, string s8)
	{
		int num = 0;
		int num2 = ((num != 1) ? 7 : 0);
		int num3 = ((num != 1) ? 7 : 0);
		int num4 = ((num != 1) ? 7 : 0);
		MemoryStream memoryStream = new MemoryStream();
		byte[] bytes = Encoding.UTF8.GetBytes(s8);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		ICryptoTransform transform = rijndaelManaged.CreateEncryptor(s7, s6);
		CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
		cryptoStream.Write(bytes, 0, bytes.Length);
		cryptoStream.Close();
		memoryStream.Close();
		return Convert.ToBase64String(memoryStream.ToArray());
	}

	public string Decrypt(string data)
	{
		int num = 0;
		int num2 = ((num != 1) ? 7 : 0);
		int num3 = ((num != 1) ? 7 : 0);
		int num4 = ((num != 1) ? 7 : 0);
		try
		{
			byte[] bytes = Convert.FromBase64String("Z2Fz");
			byte[] bytes2 = Convert.FromBase64String("Y2Fv");
			string @string = Encoding.UTF8.GetString(bytes);
			string string2 = Encoding.UTF8.GetString(bytes2);
			byte[] bytes3 = Encoding.ASCII.GetBytes(string2);
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(@string, bytes3, "MD5", 2);
			byte[] bytes4 = passwordDeriveBytes.GetBytes(16);
			byte[] bytes5 = passwordDeriveBytes.GetBytes(32);
			string text = a2(data, bytes5, bytes4);
			return (!string.IsNullOrEmpty(data) && string.IsNullOrEmpty(text)) ? data : text;
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
		finally
		{
		}
		return string.Empty;
	}

	public string Decrypt(object o)
	{
		int num = 0;
		int num2 = ((num != 1) ? 7 : 0);
		int num3 = ((num != 1) ? 7 : 0);
		int num4 = ((num != 1) ? 7 : 0);
		return Decrypt(o.ToString());
	}

	private string a2(string s7, byte[] s3, byte[] s4)
	{
		int num = 0;
		int num2 = ((num != 1) ? 7 : 0);
		int num3 = ((num != 1) ? 7 : 0);
		int num4 = ((num != 1) ? 7 : 0);
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = Convert.FromBase64String(s7);
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(s3, s4), CryptoStreamMode.Write);
			cryptoStream.Write(array, 0, array.Length);
			cryptoStream.Close();
			memoryStream.Close();
			return Encoding.UTF8.GetString(memoryStream.ToArray());
		}
		catch
		{
			return string.Empty;
		}
	}
}
