using System;
using System.Text;

namespace NATechProxy;

public class TextEncoding
{
	private string[] vn1258;

	private string[] vn6909;

	private string[] vnCode;

	private char[] TCVN3_char;

	private readonly char[] Unicode_char2;

	private readonly string[] TCVN3_cap;

	private readonly string[] Unicode_cap;

	private readonly string[] VNI_char;

	private readonly string[] Unicode_char;

	public TextEncoding()
	{
		vn1258 = new string[134]
		{
			"đ", "Đ", "a\u0300", "a\u0309", "a\u0303", "a\u0301", "a\u0323", "ă", "ă\u0300", "ă\u0309",
			"ă\u0303", "ă\u0301", "ă\u0323", "â", "â\u0300", "â\u0309", "â\u0303", "â\u0301", "â\u0323", "e\u0300",
			"e\u0309", "e\u0303", "e\u0301", "e\u0323", "ê", "ê\u0300", "ê\u0309", "ê\u0303", "ê\u0301", "ê\u0323",
			"o\u0300", "o\u0309", "o\u0303", "o\u0301", "o\u0323", "ô", "ô\u0300", "ô\u0309", "ô\u0303", "ô\u0301",
			"ô\u0323", "ơ", "ơ\u0300", "ơ\u0309", "ơ\u0303", "ơ\u0301", "ơ\u0323", "u\u0300", "u\u0309", "u\u0303",
			"u\u0301", "u\u0323", "ư", "ư\u0300", "ư\u0309", "ư\u0303", "ư\u0301", "ư\u0323", "i\u0300", "i\u0309",
			"i\u0303", "i\u0301", "i\u0323", "y\u0300", "y\u0309", "y\u0303", "y\u0301", "y\u0323", "A\u0300", "A\u0309",
			"A\u0303", "A\u0301", "A\u0323", "Ă", "Ă\u0300", "Ă\u0309", "Ă\u0303", "Ă\u0301", "Ă\u0323", "Â",
			"Â\u0300", "Â\u0309", "Â\u0303", "Â\u0301", "Â\u0323", "E\u0300", "E\u0309", "E\u0303", "E\u0301", "E\u0323",
			"Ê", "Ê\u0300", "Ê\u0309", "Ê\u0303", "Ê\u0301", "Ê\u0323", "O\u0300", "O\u0309", "O\u0303", "O\u0301",
			"O\u0323", "Ô", "Ô\u0300", "Ô\u0309", "Ô\u0303", "Ô\u0301", "Ô\u0323", "Ơ", "Ơ\u0300", "Ơ\u0309",
			"Ơ\u0303", "Ơ\u0301", "Ơ\u0323", "U\u0300", "U\u0309", "U\u0303", "U\u0301", "U\u0323", "Ư", "Ư\u0300",
			"Ư\u0309", "Ư\u0303", "Ư\u0301", "Ư\u0323", "I\u0300", "I\u0309", "I\u0303", "I\u0301", "I\u0323", "Y\u0300",
			"Y\u0309", "Y\u0303", "Y\u0301", "Y\u0323"
		};
		vn6909 = new string[134]
		{
			"đ", "Đ", "à", "ả", "ã", "á", "ạ", "ă", "ằ", "ẳ",
			"ẵ", "ắ", "ặ", "â", "ầ", "ẩ", "ẫ", "ấ", "ậ", "è",
			"ẻ", "ẽ", "é", "ẹ", "ê", "ề", "ể", "ễ", "ế", "ệ",
			"ò", "ỏ", "õ", "ó", "ọ", "ô", "ồ", "ổ", "ỗ", "ố",
			"ộ", "ơ", "ờ", "ở", "ỡ", "ớ", "ợ", "ù", "ủ", "ũ",
			"ú", "ụ", "ư", "ừ", "ử", "ữ", "ứ", "ự", "ì", "ỉ",
			"ĩ", "í", "ị", "ỳ", "ỷ", "ỹ", "ý", "ỵ", "À", "Ả",
			"Ã", "Á", "Ạ", "Ă", "Ằ", "Ẳ", "Ẵ", "Ắ", "Ặ", "Â",
			"Ầ", "Ẩ", "Ẫ", "Ấ", "Ậ", "È", "Ẻ", "Ẽ", "É", "Ẹ",
			"Ê", "Ề", "Ể", "Ễ", "Ế", "Ệ", "Ò", "Ỏ", "Õ", "Ó",
			"Ọ", "Ô", "Ồ", "Ổ", "Ỗ", "Ố", "Ộ", "Ơ", "Ờ", "Ở",
			"Ỡ", "Ớ", "Ợ", "Ù", "Ủ", "Ũ", "Ú", "Ụ", "Ư", "Ừ",
			"Ử", "Ữ", "Ứ", "Ự", "Ì", "Ỉ", "Ĩ", "Í", "Ị", "Ỳ",
			"Ỷ", "Ỹ", "Ý", "Ỵ"
		};
		vnCode = new string[134]
		{
			"d", "D", "a", "a", "a", "a", "a", "a", "a", "a",
			"a", "a", "a", "a", "a", "a", "a", "a", "a", "e",
			"e", "e", "e", "e", "e", "e", "e", "e", "e", "e",
			"o", "o", "o", "o", "o", "o", "o", "o", "o", "o",
			"o", "o", "o", "o", "o", "o", "o", "u", "u", "u",
			"u", "u", "u", "u", "u", "u", "u", "u", "i", "i",
			"i", "i", "i", "y", "y", "y", "y", "y", "A", "A",
			"A", "A", "A", "A", "A", "A", "A", "A", "A", "A",
			"A", "A", "A", "A", "A", "E", "E", "E", "E", "E",
			"E", "E", "E", "E", "E", "E", "O", "O", "O", "O",
			"O", "O", "O", "O", "O", "O", "O", "O", "O", "O",
			"O", "O", "O", "U", "U", "U", "U", "U", "U", "U",
			"U", "U", "U", "U", "I", "I", "I", "I", "I", "Y",
			"Y", "Y", "Y", "Y"
		};
		TCVN3_char = new char[73]
		{
			'ü', 'û', 'þ', 'ú', 'ù', '÷', 'ö', 'õ', 'ø', 'ñ',
			'ô', 'î', 'ì', 'ë', 'ê', 'í', 'é', 'ç', 'æ', 'å',
			'è', 'á', 'ä', 'Þ', 'Ø', 'Ö', 'Ô', 'Ó', 'Ò', 'Õ',
			'Ï', 'Î', 'Ñ', 'Æ', '½', '¼', '«', '¾', 'Ë', 'É',
			'È', 'Ç', 'Ê', '¶', '¹', '\u00ad', '¦', '¬', '¥', 'ò',
			'Ü', '®', '\u00a8', '¡', 'ó', 'ï', 'â', '»', 'ã', 'ß',
			'Ý', '×', 'ª', 'Ð', 'Ì', '·', '©', '\u00b8', 'µ', '¤',
			'§', '£', '¢'
		};
		Unicode_char2 = new char[73]
		{
			'ỹ', 'ỷ', 'ỵ', 'ỳ', 'ự', 'ữ', 'ử', 'ừ', 'ứ', 'ủ',
			'ụ', 'ợ', 'ỡ', 'ở', 'ờ', 'ớ', 'ộ', 'ỗ', 'ổ', 'ồ',
			'ố', 'ỏ', 'ọ', 'ị', 'ỉ', 'ệ', 'ễ', 'ể', 'ề', 'ế',
			'ẽ', 'ẻ', 'ẹ', 'ặ', 'ẵ', 'ẳ', 'ô', 'ắ', 'ậ', 'ẫ',
			'ẩ', 'ầ', 'ấ', 'ả', 'ạ', 'ư', 'Ư', 'ơ', 'Ơ', 'ũ',
			'ĩ', 'đ', 'ă', 'Ă', 'ú', 'ù', 'õ', 'ằ', 'ó', 'ò',
			'í', 'ì', 'ê', 'é', 'è', 'ã', 'â', 'á', 'à', 'Ô',
			'Đ', 'Ê', 'Â'
		};
		TCVN3_cap = new string[60]
		{
			"Aà", "Aả", "Aã", "Aá", "Aạ", "Eè", "Eẻ", "Eẽ", "Eé", "Eẹ",
			"Iì", "Iỉ", "Iĩ", "Ií", "Iị", "Oò", "Oỏ", "Oõ", "Oó", "Oọ",
			"Uù", "Uủ", "Uũ", "Uú", "Uụ", "Yỳ", "Yỷ", "Yỹ", "Yý", "Yỵ",
			"Ăằ", "Ăẳ", "Ăẵ", "Ăắ", "Ăặ", "Âầ", "Âẩ", "Âẫ", "Âấ", "Âậ",
			"Êề", "Êể", "Êễ", "Êế", "Êệ", "Ôồ", "Ôổ", "Ôỗ", "Ôố", "Ôộ",
			"Ơờ", "Ơở", "Ơỡ", "Ơớ", "Ơợ", "Ưừ", "Ưử", "Ưữ", "Ưứ", "Ưự"
		};
		Unicode_cap = new string[60]
		{
			"À", "Ả", "Ã", "Á", "Ạ", "È", "Ẻ", "Ẽ", "É", "Ẹ",
			"Ì", "Ỉ", "Ĩ", "Í", "Ị", "Ò", "Ỏ", "Õ", "Ó", "Ọ",
			"Ù", "Ủ", "Ũ", "Ú", "Ụ", "Ỳ", "Ỷ", "Ỹ", "Ý", "Ỵ",
			"Ằ", "Ẳ", "Ẵ", "Ắ", "Ặ", "Ầ", "Ẩ", "Ẫ", "Ấ", "Ậ",
			"Ề", "Ể", "Ễ", "Ế", "Ệ", "Ồ", "Ổ", "Ỗ", "Ố", "Ộ",
			"Ờ", "Ở", "Ỡ", "Ớ", "Ợ", "Ừ", "Ử", "Ữ", "Ứ", "Ự"
		};
		VNI_char = new string[116]
		{
			"OÂ", "oâ", "yõ", "YÕ", "yû", "YÛ", "yø", "YØ", "öï", "ÖÏ",
			"öõ", "ÖÕ", "öû", "ÖÛ", "öø", "ÖØ", "öù", "ÖÙ", "uû", "UÛ",
			"uï", "UÏ", "ôï", "ÔÏ", "ôõ", "ÔÕ", "ôû", "ÔÛ", "ôø", "ÔØ",
			"ôù", "ÔÙ", "oä", "OÄ", "oã", "OÃ", "oå", "OÅ", "oà", "OÀ",
			"oá", "OÁ", "oû", "OÛ", "oï", "OÏ", "eä", "EÄ", "eã", "EÃ",
			"eå", "EÅ", "eà", "EÀ", "eá", "EÁ", "eõ", "EÕ", "eû", "EÛ",
			"eï", "EÏ", "aë", "AË", "aü", "AÜ", "aú", "AÚ", "aè", "AÈ",
			"aé", "AÉ", "aä", "AÄ", "aã", "AÃ", "aå", "AÅ", "aà", "AÀ",
			"aá", "AÁ", "aû", "AÛ", "aï", "AÏ", "uõ", "UÕ", "aê", "AÊ",
			"yù", "uù", "uø", "oõ", "où", "oø", "eâ", "eù", "eø", "aõ",
			"aâ", "aù", "aø", "YÙ", "UÙ", "UØ", "OÕ", "OÙ", "OØ", "EÂ",
			"EÙ", "EØ", "AÕ", "AÂ", "AÙ", "AØ"
		};
		Unicode_char = new string[116]
		{
			"Æ", "æ", "ỹ", "Ỹ", "ỷ", "Ỷ", "ỳ", "Ỳ", "ự", "Ự",
			"ữ", "Ữ", "ử", "Ử", "ừ", "Ừ", "ứ", "Ứ", "ủ", "Ủ",
			"ụ", "Ụ", "ợ", "Ợ", "ỡ", "Ỡ", "ở", "Ở", "ờ", "Ờ",
			"ớ", "Ớ", "ộ", "Ộ", "ỗ", "Ỗ", "ổ", "Ổ", "ồ", "Ồ",
			"ố", "Ố", "ỏ", "Ỏ", "ọ", "Ọ", "ệ", "Ệ", "ễ", "Ễ",
			"ể", "Ể", "ề", "Ề", "ế", "Ế", "ẽ", "Ẽ", "ẻ", "Ẻ",
			"ẹ", "Ẹ", "ặ", "Ặ", "ẵ", "Ẵ", "ẳ", "Ẳ", "ằ", "Ằ",
			"ắ", "Ắ", "ậ", "Ậ", "ẫ", "Ẫ", "ẩ", "Ẩ", "ầ", "Ầ",
			"ấ", "Ấ", "ả", "Ả", "ạ", "Ạ", "ũ", "Ũ", "ă", "Ă",
			"ý", "ú", "ù", "õ", "ó", "ò", "ê", "é", "è", "ã",
			"â", "á", "à", "Ý", "Ú", "Ù", "Õ", "Ó", "Ò", "Ê",
			"É", "È", "Ã", "Â", "Á", "À"
		};
	}

	public string RemoveVietnameseCharacter(string strName)
	{
		return RemoveVietnameseCharacter(strName, bUpperCase: true);
	}

	public string RemoveVietnameseCharacter(string strName, bool bUpperCase)
	{
		if (string.IsNullOrEmpty(strName))
		{
			return string.Empty;
		}
		string text = strName;
		for (int i = 0; i < vnCode.Length; i++)
		{
			text = text.Replace(vn1258[i], vnCode[i]);
			text = text.Replace(vn6909[i], vnCode[i]);
		}
		text = text.Trim();
		if (bUpperCase)
		{
			text = text.Replace(" ", "-").Replace("\t", "-").Replace("\n", "-");
			string text2 = "`~!@#$+%^&*()+=*.,/\\?][}{|\r;:\"'";
			for (int j = 0; j < text2.Length; j++)
			{
				text = text.Replace(text2[j].ToString(), string.Empty);
			}
			return text.ToUpper();
		}
		return text;
	}

	public object ToTCVN6909(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return value;
		}
		if (value.GetType() == typeof(string))
		{
			string text = (string)value;
			for (int i = 0; i <= vn1258.Length - 1; i++)
			{
				text = text.Replace(vn1258[i], vn6909[i]);
			}
			return text;
		}
		return value;
	}

	public object ToTCVN1258(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return value;
		}
		if (value.GetType() == typeof(string))
		{
			string text = (string)value;
			for (int i = 0; i <= vn6909.Length - 1; i++)
			{
				text = text.Replace(vn6909[i], vn1258[i]);
			}
			return text;
		}
		return value;
	}

	public string GetTag(string VietnameseContent)
	{
		VietnameseContent = Misc.ToString(new TextEncoding().ToTCVN6909(VietnameseContent));
		VietnameseContent = new TextEncoding().RemoveVietnameseCharacter(VietnameseContent, bUpperCase: true);
		VietnameseContent = VietnameseContent.ToLower().Replace(" ", "-").Replace("\"", "");
		return VietnameseContent;
	}

	public string ToBase32String(byte[] bytes)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 5;
		int num2 = 0;
		string text = "AQZ2WSX3DEC4RFV5TGB6YNH7UJM8KL9P";
		while (num2 < bytes.Length)
		{
			byte b;
			if (num > 8)
			{
				b = (byte)((uint)bytes[num2++] >> num - 5);
				if (num2 != bytes.Length)
				{
					b = (byte)(((uint)(byte)(bytes[num2] << 16 - num) >> 3) | b);
				}
				num -= 3;
			}
			else if (num == 8)
			{
				b = (byte)((uint)bytes[num2++] >> 3);
				num -= 3;
			}
			else
			{
				b = (byte)((uint)(byte)(bytes[num2] << 8 - num) >> 3);
				num += 5;
			}
			stringBuilder.Append(text[b]);
		}
		return stringBuilder.ToString();
	}

	public byte[] FromBase32String(string str)
	{
		byte[] array = new byte[str.Length * 5 / 8];
		str = str.ToUpper();
		string text = "AQZ2WSX3DEC4RFV5TGB6YNH7UJM8KL9P";
		if (str.Length < 3)
		{
			array[0] = (byte)(text.IndexOf(str[0]) | (text.IndexOf(str[1]) << 5));
			return array;
		}
		int num = text.IndexOf(str[0]) | (text.IndexOf(str[1]) << 5);
		int i = 10;
		int num2 = 2;
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = (byte)num;
			num >>= 8;
			for (i -= 8; i < 8; i += 5)
			{
				if (num2 >= str.Length)
				{
					break;
				}
				num |= text.IndexOf(str[num2++]) << i;
			}
		}
		return array;
	}

	public string TCVN3ToUniCode(string str)
	{
		StringBuilder stringBuilder = new StringBuilder(str);
		for (int i = 0; i < TCVN3_char.Length; i++)
		{
			stringBuilder.Replace(TCVN3_char[i], Unicode_char2[i]);
		}
		for (int j = 0; j < TCVN3_cap.Length; j++)
		{
			stringBuilder.Replace(TCVN3_cap[j], Unicode_cap[j]);
		}
		str = stringBuilder.ToString();
		return str;
	}

	public string VietWareToUniCode(string str)
	{
		StringBuilder stringBuilder = new StringBuilder(str);
		stringBuilder.Replace('Ñ', 'Đ');
		stringBuilder.Replace('ñ', 'đ');
		stringBuilder.Replace('Ó', 'Ĩ');
		stringBuilder.Replace('ó', 'ĩ');
		stringBuilder.Replace('Ò', 'Ị');
		stringBuilder.Replace('ò', 'ị');
		stringBuilder.Replace('Æ', 'Ỉ');
		stringBuilder.Replace('æ', 'ỉ');
		stringBuilder.Replace('Î', 'Ỵ');
		stringBuilder.Replace('î', 'ỵ');
		for (int i = 0; i < VNI_char.Length; i++)
		{
			stringBuilder.Replace(VNI_char[i], Unicode_char[i]);
		}
		stringBuilder.Replace('Ô', 'Ơ');
		stringBuilder.Replace('ô', 'ơ');
		stringBuilder.Replace('Ö', 'Ư');
		stringBuilder.Replace('ö', 'ư');
		stringBuilder.Replace('Æ', 'Ô');
		stringBuilder.Replace('æ', 'ô');
		stringBuilder.Replace("ḍ", "dị");
		str = stringBuilder.ToString();
		return str;
	}
}
