using System.Data;
using System.IO;

namespace NATechProxy;

public class bllMultiProxy
{
	public const string MultiDataFile = "MultiProxy.xml";

	public void InitMultiProxyType(ref DataTable dtData)
	{
		if (dtData == null)
		{
			dtData = new DataTable("MultiProxy");
		}
		if (File.Exists("MultiProxy.xml"))
		{
			dtData.ReadXml("MultiProxy.xml");
		}
		if (!dtData.Columns.Contains("Type"))
		{
			dtData.Columns.Add("Type", typeof(int));
		}
		if (!dtData.Columns.Contains("ServiceUrl"))
		{
			dtData.Columns.Add("ServiceUrl", typeof(string));
		}
	}

	public DataTable GetProxyType()
	{
		DataTable dataTable = new DataTable("ProxyType");
		if (!dataTable.Columns.Contains("ID"))
		{
			dataTable.Columns.Add("ID", typeof(int));
		}
		if (!dataTable.Columns.Contains("NAME"))
		{
			dataTable.Columns.Add("NAME", typeof(string));
		}
		dataTable.Rows.Add(3, "X Proxy");
		dataTable.Rows.Add(4, "OBC Proxy");
		return dataTable;
	}
}
