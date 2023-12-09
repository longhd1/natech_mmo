using System;
using System.IO;
using System.Linq;
using System.Text;

namespace NATechDriver;

public class Patcher
{
	private string _driverExecutablePath;

	public Patcher(string driverExecutablePath = null)
	{
		_driverExecutablePath = driverExecutablePath;
	}

	public void Auto()
	{
		if (!isBinaryPatched())
		{
			patchExe();
		}
	}

	private bool isBinaryPatched()
	{
		if (_driverExecutablePath == null)
		{
			throw new Exception("driverExecutablePath is required.");
		}
		using FileStream stream = new FileStream(_driverExecutablePath, FileMode.Open, FileAccess.Read);
		using StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("ISO-8859-1"));
		while (true)
		{
			string text = streamReader.ReadLine();
			if (text == null)
			{
				break;
			}
			if (text.Contains("cdc_"))
			{
				return false;
			}
		}
		return true;
	}

	private int patchExe()
	{
		int num = 0;
		string text = genRandomCdc();
		Console.WriteLine(text);
		using (FileStream fileStream = new FileStream(_driverExecutablePath, FileMode.Open, FileAccess.ReadWrite))
		{
			byte[] array = new byte[1];
			StringBuilder stringBuilder = new StringBuilder("....");
			int num2 = 0;
			while (fileStream.Read(array, 0, array.Length) != 0)
			{
				stringBuilder.Remove(0, 1);
				stringBuilder.Append((char)array[0]);
				if (stringBuilder.ToString() == "cdc_")
				{
					fileStream.Seek(-4L, SeekOrigin.Current);
					byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(text);
					fileStream.Write(bytes, 0, bytes.Length);
					num++;
				}
			}
		}
		return num;
	}

	private string genRandomCdc()
	{
		string element = "abcdefghijklmnopqrstuvwxyz";
		Random random = new Random();
		char[] array = (from s in Enumerable.Repeat(element, 26)
			select s[random.Next(s.Length)]).ToArray();
		for (int i = 4; i <= 6; i++)
		{
			array[array.Length - i] = char.ToUpper(array[array.Length - i]);
		}
		array[2] = array[0];
		array[3] = '_';
		return new string(array);
	}
}
