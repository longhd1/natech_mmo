using System.Data;

namespace NATechProxy;

public class bllConst
{
	public const string AdsUrl = "Tu7nDLdDvPQE9w9pavOW63BzCEUFsJGHp8eyhr8A5GERvwf8LNaUL9+hN2zkmJfbbpfEXBGYKVeXAIK+53cQsW7e4JCmML7A3WqFGc0B1zzqKfAMB834SZj03iLAuZ5Ps9Vyf8en8viSsf9nPPlST4azJ0QMTIGIqDPRQWcLRBzpII1namKZjs+JriZFjkBYAdVPFJ96G3YXRQrWl50rcw==";

	public const string SEOUrl = "Tu7nDLdDvPQE9w9pavOW63BzCEUFsJGHp8eyhr8A5GGRyoZr0Ruqzj7tqUEnnAivyAA4k83yiN7D1EYGjlT7d+a0zsprCmkVp2zhjWgw+giapJVf1i2q/G4TaZ1c1WuzGwdN67BcFozg9NswUMn7X2NknT2t5d9k4x2ceJdeDXzxm8y7zCn1kH8/9gjScxrh88XAlfU8u8i5UHYHoXkHyA==";

	public static string LoadUrl = string.Empty;

	public DataTable GetOption()
	{
		DataTable dataTable = new DataTable("Option");
		dataTable.Columns.Add("ID", typeof(int));
		dataTable.Columns.Add("NAME", typeof(string));
		dataTable.Rows.Add(1, "SEO Google");
		dataTable.Rows.Add(2, "SEO Google Video");
		dataTable.Rows.Add(3, "Direct Traffic");
		dataTable.Rows.Add(4, "SEO Youtube");
		dataTable.Rows.Add(5, "SEO Youtube Live");
		dataTable.Rows.Add(6, "View Youtube");
		dataTable.Rows.Add(7, "SEO Youtube (Upload date)");
		dataTable.Rows.Add(8, "SEO Backlink Youtube");
		dataTable.Rows.Add(9, "SEO Google Maps(Comming soon)");
		dataTable.Rows.Add(10, "SEO Google Image(Comming soon)");
		return dataTable;
	}

	public DataTable GetDisplayMode()
	{
		DataTable dataTable = new DataTable("DisplayMode");
		dataTable.Columns.Add("ID", typeof(int));
		dataTable.Columns.Add("NAME", typeof(string));
		dataTable.Rows.Add(1, "Full screen");
		dataTable.Rows.Add(2, "Chia cửa sổ (Chạy từ 10 luồng trở xuống)");
		dataTable.Rows.Add(3, "Ẩn trình duyệt");
		return dataTable;
	}
}
