using System.Collections.Generic;
using System.Windows.Forms;

namespace NATechDriver;

public class DriverRec
{
	public int X { get; set; }

	public int Y { get; set; }

	public int Width { get; set; }

	public int Height { get; set; }

	public DriverRec()
	{
	}

	public DriverRec(int x, int y, int width, int height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}

	public Dictionary<int, DriverRec> SplitScreen(int DisplayMode, int SoCuaSo, int SoProxy, int TypeIp, int TypeProxy)
	{
		if (DisplayMode != 2)
		{
			return null;
		}
		switch (TypeIp)
		{
		case 0:
		case 1:
			return SplitScreen(SoCuaSo);
		case 2:
			switch (TypeProxy)
			{
			case 1:
				return SplitScreen(SoCuaSo);
			case 2:
			case 7:
				return SplitScreen(SoCuaSo * SoProxy);
			case 3:
			case 4:
			case 5:
			case 6:
				return SplitScreen(SoProxy);
			}
			break;
		}
		return SplitScreen(SoCuaSo * SoProxy);
	}

	public Dictionary<int, DriverRec> SplitScreen(int SoCuaSo)
	{
		int num = SoCuaSo / 2;
		if (SoCuaSo <= 2)
		{
			num = 2;
		}
		int width = Screen.PrimaryScreen.Bounds.Width;
		int height = Screen.PrimaryScreen.Bounds.Height;
		int num2 = width / num;
		int num3 = height / 2;
		int num4 = 0;
		int num5 = 0;
		Dictionary<int, DriverRec> dictionary = new Dictionary<int, DriverRec>();
		for (int i = 0; i < SoCuaSo; i++)
		{
			DriverRec value = new DriverRec(num4, num5, num2, num3);
			dictionary.Add(i, value);
			num4 += num2;
			if (num4 >= width - 5)
			{
				num4 = 0;
				num5 += num3;
			}
		}
		return dictionary;
	}
}
