namespace NATechProxy;

public class DECIMALS
{
	public static double RoundD(double d, int Decimals)
	{
		return Misc.ObjDbl(Round(Misc.ObjDec(d), Decimals));
	}

	public static decimal Round(decimal d, int Decimals)
	{
		if (Decimals < 0)
		{
			return d;
		}
		string text = decimal.Remainder(d, 1m).ToString();
		if (text.EndsWith("0"))
		{
			text = text.TrimEnd('0');
		}
		if (text.Length == Decimals + 3 && text.EndsWith("5"))
		{
			decimal num = 1m;
			for (int i = 0; i <= Decimals; i++)
			{
				num /= 10m;
			}
			d += num;
		}
		return decimal.Round(d, Decimals);
	}

	public static decimal? Round(decimal? d)
	{
		if (d.HasValue)
		{
			return Round(d.Value);
		}
		return d;
	}
}
