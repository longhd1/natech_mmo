using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NATechProxy;

public static class Const
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct Notice
	{
		public const string RequireText = "Bắt buộc nhập";
	}

	public enum FormState
	{
		View = 1,
		Edit,
		Add
	}

	public enum ID_APP
	{
		KeToan,
		BanHang
	}

	public const string AppName = "NC";

	public const int MaxObject = 8;

	public const int SysAdmin = -500000;

	public static Dictionary<int, List<string>> dicEncryptObject = new Dictionary<int, List<string>>();

	public static Dictionary<int, List<string>> dicEncryptObjectHM = new Dictionary<int, List<string>>();

	public static IntPtr hWnd_mainFrm;

	public static List<string> lstFormParam;

	public static List<string> lstModun;

	public static bool SaveRightEn;

	public static string SHORTNAME;
}
