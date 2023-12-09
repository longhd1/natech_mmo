using System;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;

namespace NATech;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		try
		{
			SkinManager.EnableFormSkins();
			BonusSkins.Register();
			UserLookAndFeel.Default.SetSkinStyle("iMaginary");
			frmMain frmMain2 = new frmMain();
			frmMain2.Height = 800;
			Application.Run(frmMain2);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
