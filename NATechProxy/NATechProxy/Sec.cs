using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace NATechProxy;

public class Sec
{
	internal class PubLic
	{
		public string[] Pub = new string[18]
		{
			"<BitStrength>744</BitStrength><RSAKeyValue><Modulus>wL/pc+XqKPlge6+fnO3Hz0faB35qkiJTpeGhAvFHWtrqrhcY5er2VUWGLSMSiNTk7guJDH0wx5RLMkW4Iy5+p5PV8GCBg5O9FxxD8rjaBRuqqOFYWkXWZzaGDgjp</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>sO8lVgcR2aUVtkr7iim43DiKIemZ1WGg3HSqusY0fmTYRmAmYckfllSHZhoFkTZDQQ2efWKPmBhOINsbG/7sTOinzg9Ga2GEFubPGXGdIhuBs3u1qKdT39/edxUH</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>0lEvNPm2K6dA5Tlr0ahuk4MFxtZ1jx2oq0UtmhjlnUQMx3l3xzfCaZVhXuKYZxhmXrx5iZzedGPzCJiTY4POx5RbJxKIZym5hVtaMvI8pqvNvGjNECoo9uzdAv0x</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>r6AH+FLlItVXfndKEdm/dQyT2S88BHsW5H2vfyib3y3cxLz18qIbLSKCAP547ki25dwEulyYNbJQHJHAFOR897i+EC5KbLkNqwXc1aDp04DBgfk4qui91SSEQOy1</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>ncF1Tc4cFRXlYi9/8niy0XAVmO5Z4mF82MzBd9RiloO3PFTjBJ+65JZ3eRu0cWG1KKIoWUEFXs7RDX4DfxZPgHDLGmLjtvkJAin9Orfair0WDLV0AMIckaGqDsFP</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>25VXiI65hOS0quOkzT0WOd3EED1GKI//eMp3k9xfPaw7QFAHgBuKcN/V/lm76srIVZSmer+lxcoxEQVVU2DRJD8gpo1sx4HVEg9xhjSQWQ/hqJNNQHrWhw2av5Qd</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>rTUBlTz0s+8ddeRVlR3Ys6+ujS2z7DCP+hEFiJRNb5ZqUzRiADsHamMzmYnjnSGKAJlYrkOZE+7J7/0fIBfKsMUikyiSAb8VfGxUs6iiAmGt3z6+yIoheczxawoV</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>skDWSkOpEyQTdshDxhwRLNnaVtL4A2bzje8lieiBlxo52yxo1TWwrwLI1f0sSwcpl3Uqlo9bzlXZCXBFTaKksJ6ZduiS5zgBd06U8qr9XLTe/TvfAaa83zHOg1QX</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>w/SIT8xPznGRP7AR83hwNgyED/uy9X20LBUJYrJR62BUxM0pwAhQU8ta/s4Gjg08fV/cYuYTVgrR+hwpB69tUkVnHaHTYmecy8mUt9OXsUiQAsMNglgjzGFLh673</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>2oZ2Ba+qqZto7eWkOOp1bVO7/0Sl1lOydI7z0uctJqZfYOQSVH8w6z/WC0pjkogtlVSsNTXGVNdgkwgOy19AteO7crbVB8rNCUjVWnp4pLFciwdLH/9uMemlnKFh</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>",
			"<BitStrength>744</BitStrength><RSAKeyValue><Modulus>tQb9yxXr0XoosrwV3pv6abvBQwy3PJjB/AaDHIxYJWFdc4F7aFD76dvVOW0uiu+l4XyRp6qZMg0f6MR8UA2VNq+Ry0s6o6nRYwe+dd6COHIieAbJc2thNBRXL+BN</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>xMYMbXft5lbV9P78tXZf3d8dyOURZHft2SGB26axFDUtphDu4EbmUF6f/52aBkokP/RGZuMIWlGoTogWhYi2w7B2EuEeoYo6IhavMhYFf48r3uG3Tv2ijQnou36j</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>wnxzvGuN04oo6TAYYM0e1oPRuoaKtm+Q29hFmJ5YCtTUIY9yoxcyMRL5/KSc8WukFnDwRr60M24R/FIxDVScdDiKrdfkbyTH9VPcxuPICf5RRi2kHghE5Dmcs7Gn</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>uLm+GJI/OYdgY6BIjGgd8SK6ryplndlPqarUTwnRrydPyDV5+GVlgEs3eyUQkgCkCMCafQJQA4PT1yB2CqQLZ0BZ5Qao4oXO6EeB2uTmOr/8/2dnTNfkx7EWkwUD</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>wwabtKxdH9N2bEe7ziw+SnHvLyJpOfK1ecA54zJC5Ix75xNQo2WktFvbyxle09AYAWwRyuci2KgtawZSuKmWnNAPtB1Zy7Rh126rtJ847eUnIowcKCnlTMPlALn5</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>s89kwIMyG58dYFYbJJlIA02uesa+uTuWU3rPePSkgAz335Vp6R6NGXV24h/YGEKIZZqxc3nr6erljApnRlHpY67Q4Z5IX3b0s8V79izjI2YcpuTrDBqPt/ADNwpX</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>2+klXSb7By87/yENLWZ9XrRc19y2vRiNyX6LMOX6Ll9Wc+aE4N4Bfk5U/K4uAbIkH4w1jXsq6G9WckggS8xmMDIMdSw2W9iuxvsDvAmLMxgDaC1coXdEVgCL8txt</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "<BitStrength>744</BitStrength><RSAKeyValue><Modulus>p1R3jro6vFKwypEtbceloObughco5kF2kcs6K6Jv1S9uyuNlVhDbHYAYf0nP5rDmfVXy1Z8VyfblNnI9FRKtGjLil6k48sSTlMsTvuf7Im+uigllTlGi+UA5MyFv</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
		};

		public string Period = "j3se7nASPUWr3vMgM/yI99FGMrHK26ZPLnS9XaerT0VULy44CIP4mzkv6hvnaC3IJFB4hx08N8+VLhhe/TCM3bt7ls+b6rKZaRYM+UIANrSz+wQJZrAnSCMi7PfdV9raxQCHj/2MWCTqBiusmYEzkfF4BvhG4+HYpaK1p3Duc5xCxcRvdEKagW/8g+gvKVk6MeLGS1SxH/XAKKbj2EXLaRuI7fKHL0nCWjkD4IWPqgfjhgqKAg8DKVPkLz8nR172JbR5436ehJN7f4Oev6Gb5AZxTl9MXglvjKaD+wlejZRKdr80xGOrO4C5J+m4ismsXkIPdR1TnaxNwCXSVzEbDI+SjghouprNzh+7Eu+SS/Bwck8xPAgT5ABHykVptEAX";
	}

	public class AES
	{
		public static string Encrypt(string clearText)
		{
			string password = "MAKV2SPBNI99212";
			byte[] bytes = Encoding.Unicode.GetBytes(clearText);
			using (Aes aes = Aes.Create())
			{
				Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, new byte[13]
				{
					73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
					100, 101, 118
				});
				aes.Key = rfc2898DeriveBytes.GetBytes(32);
				aes.IV = rfc2898DeriveBytes.GetBytes(16);
				using MemoryStream memoryStream = new MemoryStream();
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
				{
					cryptoStream.Write(bytes, 0, bytes.Length);
					cryptoStream.Close();
				}
				clearText = Convert.ToBase64String(memoryStream.ToArray());
			}
			return clearText;
		}

		public static string Decrypt(string cipherText)
		{
			string password = "MAKV2SPBNI99212";
			cipherText = cipherText.Replace(" ", "+");
			byte[] array = Convert.FromBase64String(cipherText);
			using (Aes aes = Aes.Create())
			{
				Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, new byte[13]
				{
					73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
					100, 101, 118
				});
				aes.Key = rfc2898DeriveBytes.GetBytes(32);
				aes.IV = rfc2898DeriveBytes.GetBytes(16);
				using MemoryStream memoryStream = new MemoryStream();
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
				{
					cryptoStream.Write(array, 0, array.Length);
					cryptoStream.Close();
				}
				cipherText = Encoding.Unicode.GetString(memoryStream.ToArray());
			}
			return cipherText;
		}
	}

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

	public string CaculatePass(int NO_OBJ, string Pass)
	{
		string mD = GetMD5(NO_OBJ.ToString());
		return GetMD5(GetMD5(Pass) + mD);
	}

	public bool CheckLogin(int NO_OBJ, string dbPass, string RawPass, string Pass1, string Email)
	{
		bool flag = false;
		Sec sec = new Sec();
		string text = sec.CaculatePass(NO_OBJ, RawPass);
		if (text == dbPass)
		{
			flag = true;
		}
		if (!flag && dbPass == Pass1)
		{
			flag = true;
		}
		return flag;
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

	public string GetMD5(byte[] bs)
	{
		int num = 0;
		int num2 = ((num != 1) ? 7 : 0);
		int num3 = ((num != 1) ? 7 : 0);
		int num4 = ((num != 1) ? 7 : 0);
		try
		{
			byte[] array = new MD5CryptoServiceProvider().ComputeHash(bs, 0, bs.Length);
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

	public bool GetFileWritePermition(string FilePath)
	{
		try
		{
			foreach (FileSystemAccessRule accessRule in File.GetAccessControl(FilePath).GetAccessRules(includeExplicit: true, includeInherited: true, typeof(NTAccount)))
			{
				if ((accessRule.FileSystemRights & FileSystemRights.Write) == FileSystemRights.Write && (accessRule.FileSystemRights & FileSystemRights.Modify) == FileSystemRights.Modify)
				{
					return true;
				}
			}
		}
		catch
		{
		}
		return false;
	}

	public string DecryptST(string st)
	{
		if (string.IsNullOrEmpty(st) || !st.StartsWith("¿"))
		{
			return st;
		}
		st = st.Substring(1);
		int num = 18;
		string data = "eKuxwWTUJogtM6iAFNnlzaeHRdwKd61rrGaNG+oHc8TTtu73qXnEy9YME40AyMzjxKwut/xpXczn0k2FwMMbftIJA9ALWNMpX0oXPhzEXONb8SlRvKVo2TsXfgxfQ+qhfxiOQKYts9oyiGPDR9eQne1WxUeJr5n5ZETf47Hb7hKbMmlFiRxW+dZwSgCy8WLyT9IKTyGfJeQcfZ4G8x1zwaCAM6InPfc6Cdt6MRSvRr3V5BA6P2uHNRCU/Nwu+54eQzMavRV1FVxB/hQ0Boc23O9OEBwKbioD5WiicmKf7Jj0G0FdOZbD/Clj5y2aXl+wrl5etMr2xmNLdummnd9Ua0pPNV+gLUKMMifoHDOSWIIxIu7pc/t4OASseLdRpLbhvYBcy0vkjgrQB7L3D6l3gqQg9gVu2W8eW/ZXGZA/TwaclHYADRBgU3D7R5LGZQwVYgaejqMsCRCUkF+sT6hz45UNagWsZwZLGedlkibhhChfqWSpoT/5f8KGxVfVh7WO+v90NUqjAOmLI7EVEsc/84saGDPTaILx9m5VbicKPxZMNRv+fm9p1y30BlN/r4v0fto2XwWXEguYg25tgB7aLX+wXw0yusMENtdCasb2LMI=";
		data = Decrypt(data);
		int num2 = data.Length - 1;
		string text = string.Empty;
		for (int i = 0; i < st.Length; i++)
		{
			int num3 = data.IndexOf(new string(st[i], 1));
			if (num3 > -1)
			{
				int index = ((num3 - num < 0) ? (num2 + (num3 - num) + 1) : (num3 - num));
				text += data[index];
			}
			else
			{
				text += new string(st[i], 1);
			}
		}
		return text;
	}

	public string EncryptST(string st)
	{
		if (string.IsNullOrEmpty(st) || st.StartsWith("¿"))
		{
			return st;
		}
		TextEncoding textEncoding = new TextEncoding();
		st = textEncoding.ToTCVN6909(st) as string;
		int num = 18;
		string data = "eKuxwWTUJogtM6iAFNnlzaeHRdwKd61rrGaNG+oHc8TTtu73qXnEy9YME40AyMzjxKwut/xpXczn0k2FwMMbftIJA9ALWNMpX0oXPhzEXONb8SlRvKVo2TsXfgxfQ+qhfxiOQKYts9oyiGPDR9eQne1WxUeJr5n5ZETf47Hb7hKbMmlFiRxW+dZwSgCy8WLyT9IKTyGfJeQcfZ4G8x1zwaCAM6InPfc6Cdt6MRSvRr3V5BA6P2uHNRCU/Nwu+54eQzMavRV1FVxB/hQ0Boc23O9OEBwKbioD5WiicmKf7Jj0G0FdOZbD/Clj5y2aXl+wrl5etMr2xmNLdummnd9Ua0pPNV+gLUKMMifoHDOSWIIxIu7pc/t4OASseLdRpLbhvYBcy0vkjgrQB7L3D6l3gqQg9gVu2W8eW/ZXGZA/TwaclHYADRBgU3D7R5LGZQwVYgaejqMsCRCUkF+sT6hz45UNagWsZwZLGedlkibhhChfqWSpoT/5f8KGxVfVh7WO+v90NUqjAOmLI7EVEsc/84saGDPTaILx9m5VbicKPxZMNRv+fm9p1y30BlN/r4v0fto2XwWXEguYg25tgB7aLX+wXw0yusMENtdCasb2LMI=";
		data = Decrypt(data);
		int num2 = data.Length - 1;
		string text = string.Empty;
		for (int i = 0; i < st.Length; i++)
		{
			int num3 = data.IndexOf(new string(st[i], 1));
			if (num3 > -1)
			{
				int index = ((num3 + num > num2) ? (num3 + num - 1 - num2) : (num3 + num));
				text += data[index];
			}
			else
			{
				text += new string(st[i], 1);
			}
		}
		return "¿" + text;
	}

	public DataTable EnCryptSTObject(DataTable dt)
	{
		if (dt == null || !dt.Columns.Contains("ID_OTY"))
		{
			return dt;
		}
		foreach (DataRow row in dt.Rows)
		{
			if (row.RowState == DataRowState.Deleted)
			{
				continue;
			}
			int key = Misc.ObjInt(row["ID_OTY"]);
			if (!Const.dicEncryptObject.ContainsKey(key))
			{
				continue;
			}
			foreach (string item in Const.dicEncryptObject[key])
			{
				if (row.Table.Columns.Contains(item))
				{
					row[item] = EncryptST(row[item].ToString());
				}
			}
		}
		return dt;
	}

	public DataRow EnCryptSTObject(DataRow dr)
	{
		if (dr == null || dr.RowState == DataRowState.Deleted || !dr.Table.Columns.Contains("ID_OTY"))
		{
			return dr;
		}
		int key = Misc.ObjInt(dr["ID_OTY"]);
		if (Const.dicEncryptObject.ContainsKey(key))
		{
			foreach (string item in Const.dicEncryptObject[key])
			{
				if (dr.Table.Columns.Contains(item))
				{
					dr[item] = EncryptST(dr[item].ToString());
				}
			}
		}
		return dr;
	}

	public DataTable DecryptSTObject(DataTable dt)
	{
		if (dt == null || !dt.Columns.Contains("ID_OTY"))
		{
			return dt;
		}
		foreach (DataRow row in dt.Rows)
		{
			if (row.RowState == DataRowState.Deleted)
			{
				continue;
			}
			int key = Misc.ObjInt(row["ID_OTY"]);
			if (!Const.dicEncryptObject.ContainsKey(key))
			{
				continue;
			}
			foreach (string item in Const.dicEncryptObject[key])
			{
				if (row.Table.Columns.Contains(item))
				{
					row[item] = DecryptST(row[item].ToString());
				}
			}
		}
		return dt;
	}

	public void DecryptSTObject(DataRow dr)
	{
		if (dr == null || dr.RowState == DataRowState.Deleted || !dr.Table.Columns.Contains("ID_OTY"))
		{
			return;
		}
		int key = Misc.ObjInt(dr["ID_OTY"]);
		if (!Const.dicEncryptObject.ContainsKey(key))
		{
			return;
		}
		foreach (string item in Const.dicEncryptObject[key])
		{
			if (dr.Table.Columns.Contains(item))
			{
				dr[item] = DecryptST(dr[item].ToString());
			}
		}
	}

	public DataTable DecryptSTTable(DataTable dt)
	{
		if (dt == null)
		{
			return dt;
		}
		foreach (DataColumn column in dt.Columns)
		{
			if (!(column.DataType == typeof(string)))
			{
				continue;
			}
			List<string> list = Misc.SplitToList(column.ColumnName, "_");
			if (list.Count < 3 || !(list[0] == "EO") || !Misc.IsDigit(list[1]))
			{
				continue;
			}
			int key = Convert.ToInt32(list[1]);
			if (!Const.dicEncryptObject.ContainsKey(key))
			{
				continue;
			}
			foreach (DataRow row in dt.Rows)
			{
				if (row.RowState != DataRowState.Deleted)
				{
					row[column.ColumnName] = DecryptST(row[column.ColumnName].ToString());
				}
			}
		}
		return dt;
	}

	public DataTable DecryptSTMisc(DataTable dt, params string[] Fields)
	{
		if (dt == null || Const.dicEncryptObject.Count == 0)
		{
			return dt;
		}
		foreach (string text in Fields)
		{
			if (!dt.Columns.Contains(text) || !(dt.Columns[text].DataType == typeof(string)))
			{
				continue;
			}
			foreach (DataRow row in dt.Rows)
			{
				if (row.RowState != DataRowState.Deleted)
				{
					row[text] = DecryptST(row[text].ToString());
				}
			}
		}
		return dt;
	}

	public bool ValidPeriod(int Period, string ID, string sig)
	{
		try
		{
			return true;
		}
		catch
		{
			return false;
		}
	}

	public void RightDecode(ref object En, string KeyFieldValue)
	{
		bool saveRightEn = Const.SaveRightEn;
		RightDecode(ref En, KeyFieldValue, saveRightEn);
	}

	public void RightDecode(ref object En, string KeyFieldValue, bool SaveRightEn)
	{
		if (SaveRightEn)
		{
			List<string> list = Misc.SplitToList(Decrypt(CBO.GetProperty(En, "HTML").ToString()), ",");
			CBO.SetProperty(En, "RIGHT_SELECT", false);
			CBO.SetProperty(En, "RIGHT_INSERT", false);
			CBO.SetProperty(En, "RIGHT_UPDATE", false);
			CBO.SetProperty(En, "RIGHT_DELETE", false);
			CBO.SetProperty(En, "RIGHT_PRICE_VIEW", false);
			CBO.SetProperty(En, "RIGHT_PRICE", false);
			CBO.SetProperty(En, "RIGHT_LOCK", false);
			CBO.SetProperty(En, "RIGHT_UNLOCK", false);
			CBO.SetProperty(En, "RIGHT_ALL", false);
			CBO.SetProperty(En, "RIGHT_UNLOCK_R", false);
			CBO.SetProperty(En, "RIGHT_PRINT", false);
			CBO.SetProperty(En, "RIGHT_EXECUTE", false);
			if (list.Count == 14 && list[1] == KeyFieldValue)
			{
				CBO.SetProperty(En, "RIGHT_SELECT", (list[2] == "1") ? true : false);
				CBO.SetProperty(En, "RIGHT_INSERT", (list[3] == "1") ? true : false);
				CBO.SetProperty(En, "RIGHT_UPDATE", (list[4] == "1") ? true : false);
				CBO.SetProperty(En, "RIGHT_DELETE", (list[5] == "1") ? true : false);
				CBO.SetProperty(En, "RIGHT_PRICE_VIEW", (list[6] == "1") ? true : false);
				CBO.SetProperty(En, "RIGHT_PRICE", (list[7] == "1") ? true : false);
				CBO.SetProperty(En, "RIGHT_LOCK", (list[8] == "1") ? true : false);
				CBO.SetProperty(En, "RIGHT_UNLOCK", (list[9] == "1") ? true : false);
				CBO.SetProperty(En, "RIGHT_ALL", (list[10] == "1") ? true : false);
				CBO.SetProperty(En, "RIGHT_UNLOCK_R", (list[11] == "1") ? true : false);
				CBO.SetProperty(En, "RIGHT_PRINT", (list[12] == "1") ? true : false);
				CBO.SetProperty(En, "RIGHT_EXECUTE", (list[13] == "1") ? true : false);
			}
		}
	}

	public void RightEncode(ref object En, string KeyFieldValue)
	{
		bool saveRightEn = Const.SaveRightEn;
		RightEncode(ref En, KeyFieldValue, saveRightEn);
	}

	public void RightEncode(ref object En, string KeyFieldValue, bool SaveRightEn)
	{
		if (SaveRightEn)
		{
			bool flag = Misc.ObjBol(CBO.GetProperty(En, "RIGHT_SELECT"));
			bool flag2 = Misc.ObjBol(CBO.GetProperty(En, "RIGHT_INSERT"));
			bool flag3 = Misc.ObjBol(CBO.GetProperty(En, "RIGHT_UPDATE"));
			bool flag4 = Misc.ObjBol(CBO.GetProperty(En, "RIGHT_DELETE"));
			bool flag5 = Misc.ObjBol(CBO.GetProperty(En, "RIGHT_PRICE_VIEW"));
			bool flag6 = Misc.ObjBol(CBO.GetProperty(En, "RIGHT_PRICE"));
			bool flag7 = Misc.ObjBol(CBO.GetProperty(En, "RIGHT_LOCK"));
			bool flag8 = Misc.ObjBol(CBO.GetProperty(En, "RIGHT_UNLOCK"));
			bool flag9 = Misc.ObjBol(CBO.GetProperty(En, "RIGHT_ALL"));
			bool flag10 = Misc.ObjBol(CBO.GetProperty(En, "RIGHT_UNLOCK_R"));
			bool flag11 = Misc.ObjBol(CBO.GetProperty(En, "RIGHT_PRINT"));
			bool flag12 = Misc.ObjBol(CBO.GetProperty(En, "RIGHT_EXECUTE"));
			CBO.SetProperty(En, "HTML", Encrypt(string.Join(",", new List<string>
			{
				Misc.GenID(4),
				KeyFieldValue,
				flag ? "1" : "0",
				flag2 ? "1" : "0",
				flag3 ? "1" : "0",
				flag4 ? "1" : "0",
				flag5 ? "1" : "0",
				flag6 ? "1" : "0",
				flag7 ? "1" : "0",
				flag8 ? "1" : "0",
				flag9 ? "1" : "0",
				flag10 ? "1" : "0",
				flag11 ? "1" : "0",
				flag12 ? "1" : "0"
			}.ToArray())));
		}
	}

	public void RightDecode(ref DataTable dt, string KeyField, bool SaveRightEn)
	{
		if (!SaveRightEn || !dt.Columns.Contains("HTML") || !dt.Columns.Contains(KeyField))
		{
			return;
		}
		bool flag = dt.Columns.Contains("RIGHT_SELECT");
		bool flag2 = dt.Columns.Contains("RIGHT_INSERT");
		bool flag3 = dt.Columns.Contains("RIGHT_UPDATE");
		bool flag4 = dt.Columns.Contains("RIGHT_DELETE");
		bool flag5 = dt.Columns.Contains("RIGHT_PRICE_VIEW");
		bool flag6 = dt.Columns.Contains("RIGHT_PRICE");
		bool flag7 = dt.Columns.Contains("RIGHT_LOCK");
		bool flag8 = dt.Columns.Contains("RIGHT_UNLOCK");
		bool flag9 = dt.Columns.Contains("RIGHT_ALL");
		bool flag10 = dt.Columns.Contains("RIGHT_UNLOCK_R");
		bool flag11 = dt.Columns.Contains("RIGHT_PRINT");
		bool flag12 = dt.Columns.Contains("RIGHT_EXECUTE");
		foreach (DataRow row in dt.Rows)
		{
			if (row.RowState == DataRowState.Deleted)
			{
				continue;
			}
			List<string> list = Misc.SplitToList(Decrypt(row["HTML"].ToString()), ",");
			if (list.Count == 14)
			{
				if (flag)
				{
					row["RIGHT_SELECT"] = ((list[2] == "1" && list[1] == row[KeyField].ToString()) ? true : false);
				}
				if (flag2)
				{
					row["RIGHT_INSERT"] = ((list[3] == "1" && list[1] == row[KeyField].ToString()) ? true : false);
				}
				if (flag3)
				{
					row["RIGHT_UPDATE"] = ((list[4] == "1" && list[1] == row[KeyField].ToString()) ? true : false);
				}
				if (flag4)
				{
					row["RIGHT_DELETE"] = ((list[5] == "1" && list[1] == row[KeyField].ToString()) ? true : false);
				}
				if (flag5)
				{
					row["RIGHT_PRICE_VIEW"] = ((list[6] == "1" && list[1] == row[KeyField].ToString()) ? true : false);
				}
				if (flag6)
				{
					row["RIGHT_PRICE"] = ((list[7] == "1" && list[1] == row[KeyField].ToString()) ? true : false);
				}
				if (flag7)
				{
					row["RIGHT_LOCK"] = ((list[8] == "1" && list[1] == row[KeyField].ToString()) ? true : false);
				}
				if (flag8)
				{
					row["RIGHT_UNLOCK"] = ((list[9] == "1" && list[1] == row[KeyField].ToString()) ? true : false);
				}
				if (flag9)
				{
					row["RIGHT_ALL"] = ((list[10] == "1" && list[1] == row[KeyField].ToString()) ? true : false);
				}
				if (flag10)
				{
					row["RIGHT_UNLOCK_R"] = ((list[11] == "1" && list[1] == row[KeyField].ToString()) ? true : false);
				}
				if (flag11)
				{
					row["RIGHT_PRINT"] = ((list[12] == "1" && list[1] == row[KeyField].ToString()) ? true : false);
				}
				if (flag12)
				{
					row["RIGHT_EXECUTE"] = ((list[13] == "1" && list[1] == row[KeyField].ToString()) ? true : false);
				}
			}
			else
			{
				if (flag)
				{
					row["RIGHT_SELECT"] = false;
				}
				if (flag2)
				{
					row["RIGHT_INSERT"] = false;
				}
				if (flag3)
				{
					row["RIGHT_UPDATE"] = false;
				}
				if (flag4)
				{
					row["RIGHT_DELETE"] = false;
				}
				if (flag5)
				{
					row["RIGHT_PRICE_VIEW"] = false;
				}
				if (flag6)
				{
					row["RIGHT_PRICE"] = false;
				}
				if (flag7)
				{
					row["RIGHT_LOCK"] = false;
				}
				if (flag8)
				{
					row["RIGHT_UNLOCK"] = false;
				}
				if (flag9)
				{
					row["RIGHT_ALL"] = false;
				}
				if (flag10)
				{
					row["RIGHT_UNLOCK_R"] = false;
				}
				if (flag11)
				{
					row["RIGHT_PRINT"] = false;
				}
				if (flag12)
				{
					row["RIGHT_EXECUTE"] = false;
				}
			}
		}
		dt.AcceptChanges();
	}

	public void RightDecode(ref DataTable dt, string KeyField)
	{
		bool saveRightEn = Const.SaveRightEn;
		RightDecode(ref dt, KeyField, saveRightEn);
	}
}
