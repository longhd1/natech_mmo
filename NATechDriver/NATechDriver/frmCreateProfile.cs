using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using NATechApi;
using NATechApi.Models;
using NATechProxy;
using NATechProxy.Class;
using NATechProxy.Class.TMProxy;

namespace NATechDriver;

public class frmCreateProfile : Form
{
	private string sErr = string.Empty;

	private Random rand = new Random();

	private DataTable dtProfile;

	private DataTable dtTinsoftLocation;

	private DataTable dtTMLocation;

	private DataTable dtTinsoftType;

	private DataTable dtTMType;

	private string DataSourceFileName = "DataSource.xml";

	private const string Version = "2.0.0";

	private Dictionary<string, TinsoftProxy.TinsoftData> TinsoftData = new Dictionary<string, TinsoftProxy.TinsoftData>();

	private Dictionary<string, bllTMProxy.TMProxyData> TMProxyData = new Dictionary<string, bllTMProxy.TMProxyData>();

	private ProxyAllType currentFocusProxy = null;

	private int CurrentIndex = -1;

	private int SoLuong = 1;

	private ProxyNATech proxySLL = null;

	private ProxyRotation proxyRotationSLL = null;

	private IContainer components = null;

	private DataGridView grdProfile;

	private ContextMenuStrip mnuProfile;

	private ToolStripMenuItem mniOpenProfile;

	private ToolStripMenuItem mniDeleteProfile;

	private ToolStripMenuItem mniEditProfile;

	private BackgroundWorker wkCreateProfile;

	private LayoutControl locMain;

	private LayoutControlGroup layoutControlGroup1;

	private TextEdit txtProfileName;

	private LayoutControlItem lciProfileName;

	private EmptySpaceItem emptySpaceItem1;

	private SimpleButton btnCreate;

	private LayoutControlItem layoutControlItem2;

	private ComboBoxEdit cbeProxySupplier;

	private LayoutControlItem lciProxySupplier;

	private TextEdit txtIP;

	private LayoutControlItem lciIP;

	private CheckedComboBoxEdit ccbTinhTP;

	private LayoutControlItem lciTinhTP;

	private LayoutControlGroup layoutControlGroup2;

	private SpinEdit speSoLuong;

	private LayoutControlItem layoutControlItem1;

	private LayoutControlGroup layoutControlGroup3;

	private EmptySpaceItem emptySpaceItem3;

	private LabelControl lbeStatusCreateSLL;

	private SimpleButton btnCreateProfileSLL;

	private LayoutControlItem layoutControlItem3;

	private LayoutControlItem layoutControlItem4;

	private EmptySpaceItem emptySpaceItem2;

	private SimpleButton btnCreateProfileEmail;

	private SimpleButton btnUpdate;

	private LayoutControlItem layoutControlItem5;

	private LayoutControlItem layoutControlItem6;

	private EmptySpaceItem emptySpaceItem4;

	private MemoEdit memUserAgent;

	private LayoutControlItem layoutControlItem7;

	private LayoutControlItem layoutControlItem8;

	private EmptySpaceItem emptySpaceItem5;

	private EmptySpaceItem emptySpaceItem6;

	private SplitterItem splitterItem1;

	private GridLookUpEdit lueProxyType;

	private GridView gridLookUpEdit1View;

	private LayoutControlItem lciProxyType;

	private GridColumn gridColumn1;

	private DataGridViewTextBoxColumn ProfileName;

	private DataGridViewTextBoxColumn UA;

	private DataGridViewTextBoxColumn Proxy;

	private DataGridViewTextBoxColumn ProfilePath;

	private DataGridViewTextBoxColumn grdNCCProxy;

	private DataGridViewTextBoxColumn KeyProxy;

	private DataGridViewTextBoxColumn LoaiProxy;

	private DataGridViewTextBoxColumn TinhTP;

	private DataGridViewTextBoxColumn Status;

	private DataGridViewTextBoxColumn GmailInfo;

	private DataGridViewTextBoxColumn GmailLoginStatus;

	public List<string> lstUserAgent { get; set; }

	public string FolderPath { get; set; }

	public string BrowserPath { get; set; }

	public bool IsMain { get; set; }

	public string ProfileDefaultPath { get; set; }

	public string BrowserLanguage { get; set; }

	public frmCreateProfile()
	{
		InitializeComponent();
	}

	private void frmCreateProfile_Load(object sender, EventArgs e)
	{
		try
		{
			if (IsMain && e != null)
			{
				LoginApi();
				string text = Directory.GetCurrentDirectory() + "\\Data";
				if (File.Exists(text + "\\UA.txt"))
				{
					string text2 = File.ReadAllText(text + "\\UA.txt");
					lstUserAgent = SplitToList(text2, "\r\n");
					memUserAgent.Text = text2;
				}
			}
			else if (lstUserAgent != null && lstUserAgent.Count > 0)
			{
				memUserAgent.Text = string.Join("\r\n", lstUserAgent.ToArray());
			}
			if (string.IsNullOrEmpty(FolderPath))
			{
				MessageBox.Show(this, "Chưa có đường dẫn lưu trữ Profile", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				LoadDataBase();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SetRight(bool isRight)
	{
		btnCreate.Enabled = isRight;
		btnUpdate.Enabled = isRight;
		mniEditProfile.Enabled = isRight;
		mniOpenProfile.Enabled = isRight;
		mniDeleteProfile.Enabled = isRight;
	}

	private void LoginApi()
	{
		frmLogin frmLogin = new frmLogin();
		frmLogin.ShowDialog(this);
		if (ApiClientNATech.loginResponse != null && !string.IsNullOrEmpty(ApiClientNATech.loginResponse.Token) && ApiClientNATech.loginResponse.DateTo.HasValue)
		{
			if (ApiClientNATech.loginResponse.DateTo < ApiClientNATech.loginResponse.CurrentTime)
			{
				SetRight(isRight: false);
				Text = "NATECH Profile " + ApiClientNATech.loginResponse.SoftVersion + ". Hạn dùng đến: " + ApiClientNATech.loginResponse.DateTo.Value.ToString("dd/MM/yyyy") + ". Tài khoản [" + ApiClientNATech.loginResponse.UserName + "]. Thiết kế bởi NATECH (na.com.vn). ";
			}
			else
			{
				SetRight(isRight: true);
				Text = "NATECH Profile 2.0.0. Hạn dùng đến: " + ApiClientNATech.loginResponse.DateTo.Value.ToString("dd/MM/yyyy") + ". Tài khoản [" + ApiClientNATech.loginResponse.UserName + "]. Thiết kế bởi NATECH (na.com.vn).";
			}
			if (!string.IsNullOrEmpty(ApiClientNATech.loginResponse.SoftVersion) && "2.0.0" != ApiClientNATech.loginResponse.SoftVersion)
			{
				frmUpdate frmUpdate = new frmUpdate();
				frmUpdate.CurrentVersion = "2.0.0";
				frmUpdate.ShowDialog(this);
			}
		}
		else
		{
			SetRight(isRight: false);
			Text = "NATECH Profile 2.0.0. Chưa đăng nhập. Thiết kế bởi NATECH (na.com.vn). ";
		}
	}

	private void SaveData(string profileName, string profilePath, string uA, ProxyNATech proxy)
	{
		SaveData(profileName, profilePath, uA, proxy, null);
	}

	private void SaveData(string profileName, string profilePath, string uA, ProxyNATech proxy, ProxyRotation proxyRotation)
	{
		string fileName = FolderPath + "\\" + DataSourceFileName;
		InitDataSource(ref dtProfile);
		DataRow dataRow = dtProfile.NewRow();
		dataRow["ProfileName"] = profileName;
		dataRow["ProfilePath"] = profilePath;
		dataRow["Status"] = "Sẵn sàng";
		dataRow["UA"] = uA;
		if (proxy != null)
		{
			dataRow["Proxy"] = ((!string.IsNullOrEmpty(proxy.UserName)) ? (proxy.Ip + ":" + proxy.Port + ":" + proxy.UserName + ":" + proxy.Password) : (proxy.Ip + ":" + proxy.Port));
		}
		if (proxyRotation != null)
		{
			dataRow["NCCProxy"] = proxyRotation.Supplier;
			dataRow["LoaiProxy"] = proxyRotation.TypeProxy;
			dataRow["TinhTP"] = proxyRotation.Location;
			dataRow["Key"] = proxyRotation.Key;
			dataRow["Proxy"] = string.Empty;
		}
		dtProfile.Rows.Add(dataRow);
		dtProfile.WriteXml(fileName, XmlWriteMode.WriteSchema);
	}

	private void InitDataSource(ref DataTable dtProfile)
	{
		if (dtProfile == null)
		{
			dtProfile = new DataTable("NATech");
		}
		if (!dtProfile.Columns.Contains("ProfileName"))
		{
			dtProfile.Columns.Add("ProfileName", typeof(string));
		}
		if (!dtProfile.Columns.Contains("Status"))
		{
			dtProfile.Columns.Add("Status", typeof(string));
		}
		if (!dtProfile.Columns.Contains("ProfilePath"))
		{
			dtProfile.Columns.Add("ProfilePath", typeof(string));
		}
		if (!dtProfile.Columns.Contains("UA"))
		{
			dtProfile.Columns.Add("UA", typeof(string));
		}
		if (!dtProfile.Columns.Contains("Proxy"))
		{
			dtProfile.Columns.Add("Proxy", typeof(string));
		}
		if (!dtProfile.Columns.Contains("Key"))
		{
			dtProfile.Columns.Add("Key", typeof(string));
		}
		if (!dtProfile.Columns.Contains("NCCProxy"))
		{
			dtProfile.Columns.Add("NCCProxy", typeof(int));
		}
		if (!dtProfile.Columns.Contains("LoaiProxy"))
		{
			dtProfile.Columns.Add("LoaiProxy", typeof(int));
		}
		if (!dtProfile.Columns.Contains("TinhTP"))
		{
			dtProfile.Columns.Add("TinhTP", typeof(string));
		}
		if (!dtProfile.Columns.Contains("GmailInfo"))
		{
			dtProfile.Columns.Add("GmailInfo", typeof(string));
		}
		if (!dtProfile.Columns.Contains("GmailLoginStatus"))
		{
			dtProfile.Columns.Add("GmailLoginStatus", typeof(int));
		}
	}

	private void LoadDataBase()
	{
		string text = FolderPath + "\\DataSource.xml";
		if (File.Exists(text))
		{
			dtProfile = new DataTable();
			dtProfile.ReadXml(text);
			InitDataSource(ref dtProfile);
			grdProfile.DataSource = dtProfile;
		}
	}

	private void DirectoryCopy(string sourceDir, string destDir)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(sourceDir);
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		if (!directoryInfo.Exists)
		{
			throw new DirectoryNotFoundException("Không tìm thấy thư mục: " + sourceDir);
		}
		if (!Directory.Exists(destDir))
		{
			Directory.CreateDirectory(destDir);
		}
		FileInfo[] files = directoryInfo.GetFiles();
		FileInfo[] array = files;
		FileInfo[] array2 = array;
		foreach (FileInfo fileInfo in array2)
		{
			string destFileName = Path.Combine(destDir, fileInfo.Name);
			try
			{
				fileInfo.CopyTo(destFileName, overwrite: true);
			}
			catch
			{
			}
		}
		DirectoryInfo[] array3 = directories;
		DirectoryInfo[] array4 = array3;
		foreach (DirectoryInfo directoryInfo2 in array4)
		{
			string destDir2 = Path.Combine(destDir, directoryInfo2.Name);
			DirectoryCopy(directoryInfo2.FullName, destDir2);
		}
	}

	private void SaveUserAgent()
	{
		string text = Directory.GetCurrentDirectory() + "\\Data";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		File.WriteAllText(text + "\\UA.txt", memUserAgent.Text);
	}

	private string ToString(object o)
	{
		if (o != null)
		{
			return o.ToString();
		}
		return string.Empty;
	}

	public string[] Split(string s, string splitText)
	{
		if (string.IsNullOrEmpty(splitText))
		{
			return new string[1] { s };
		}
		return string.IsNullOrEmpty(s) ? new string[0] : ((splitText.Length == 1) ? s.Split(splitText[0]) : Regex.Split(s, splitText, RegexOptions.IgnoreCase));
	}

	public List<string> SplitToList(string s, string splitText)
	{
		string[] array = Split(s, splitText);
		if (array.Length == 1 && string.IsNullOrEmpty(array[0]))
		{
			return new List<string>();
		}
		List<string> list = new List<string>();
		list.AddRange(array);
		return list;
	}

	private void OpenProfile(int rowIndex)
	{
		if (string.IsNullOrEmpty(BrowserPath))
		{
			MessageBox.Show(this, "Chưa cấu hình đường dẫn trình duyệt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		string idprofile = ToString(dtProfile.Rows[rowIndex]["ProfileName"]);
		string proxy = ToString(dtProfile.Rows[rowIndex]["Proxy"]);
		string text = ToString(dtProfile.Rows[rowIndex]["Key"]);
		string text2 = "";
		string proxyfree = "";
		string proxyPort = "";
		switch (Misc.ObjInt(dtProfile.Rows[rowIndex]["NCCProxy"]))
		{
		case 0:
			if (!string.IsNullOrEmpty(proxy))
			{
				text2 = proxy.Split(':')[0];
				proxyPort = proxy.Split(':')[0] + ":" + proxy.Split(':')[1];
				proxyfree = " --host-resolver-rules=\"MAP * 0.0.0.0 , EXCLUDE " + text2;
			}
			break;
		case 1:
		{
			TinsoftProxy tinsoftProxy = new TinsoftProxy(text);
			int num3 = Misc.ObjInt(dtProfile.Rows[rowIndex]["LoaiProxy"]);
			if (!TinsoftData.ContainsKey(text))
			{
				TinsoftProxy.TinsoftData tinsoftData = new TinsoftProxy.TinsoftData();
				tinsoftData.DangLay = false;
				tinsoftData.ThoiGianCho = new TinsoftProxy().LayThoiGianCho(num3);
				tinsoftData.LastChange = DateTime.Now.AddDays(-1.0);
				TinsoftData.Add(text, tinsoftData);
			}
			List<int> list2 = new bllProxy().LayTinhTP(Misc.ToString(dtProfile.Rows[rowIndex]["TinhTP"]));
			if (list2 != null && list2.Count > 0)
			{
				tinsoftProxy.location = GetRandomValue(list2);
			}
			else
			{
				tinsoftProxy.location = 0;
			}
			int num4 = Misc.ObjInt((DateTime.Now - TinsoftData[text].LastChange).TotalSeconds);
			if (num4 > TinsoftData[text].ThoiGianCho)
			{
				TinsoftData[text].DangLay = true;
				bool flag2 = false;
				try
				{
					flag2 = tinsoftProxy.changeProxy();
				}
				catch (Exception ex2)
				{
					string message = ex2.Message;
				}
				TinsoftData[text].DangLay = false;
				if (flag2)
				{
					TinsoftData[text].LastChange = DateTime.Now;
					text2 = tinsoftProxy.ip;
					proxyPort = tinsoftProxy.proxy;
					proxyfree = " --host-resolver-rules=\"MAP * 0.0.0.0 , EXCLUDE " + text2;
					ProxyItems proxyItems2 = new ProxyItems();
					proxyItems2.fullProxy = tinsoftProxy.proxy;
					proxyItems2.ip = tinsoftProxy.ip;
					proxyItems2.port = Misc.ToString(tinsoftProxy.port);
					proxyItems2.proxyPort = bllProxy.ProxyPort.HttpIPv4;
					proxyItems2.TypeProxy = "TinSoftProxy";
					TinsoftData[text].LastProxy = proxyItems2;
					ProxyAllType proxyAllType2 = new ProxyAllType();
					proxyAllType2.Supplier = 1;
					proxyAllType2.ip = proxyItems2.ip;
					proxyAllType2.port = proxyItems2.port;
					proxyAllType2.Key = text;
					proxyAllType2.Location = Misc.ToString(tinsoftProxy.location);
					proxyAllType2.TypeProxy = num3;
					ChangeProxyProfile(rowIndex, proxyAllType2);
				}
				else
				{
					string errorCode2 = tinsoftProxy.errorCode;
					MessageBox.Show(errorCode2);
				}
			}
			else
			{
				ProxyItems lastProxy2 = TinsoftData[text].LastProxy;
				text2 = lastProxy2.ip;
				proxyPort = lastProxy2.fullProxy;
				proxyfree = " --host-resolver-rules=\"MAP * 0.0.0.0 , EXCLUDE " + text2;
			}
			break;
		}
		case 2:
		{
			bllTMProxy bllTMProxy = new bllTMProxy();
			int num = Misc.ObjInt(dtProfile.Rows[rowIndex]["LoaiProxy"]);
			if (!TMProxyData.ContainsKey(text))
			{
				bllTMProxy.TMProxyData tMProxyData = new bllTMProxy.TMProxyData();
				tMProxyData.DangLay = false;
				tMProxyData.ThoiGianCho = new bllTMProxy().LayThoiGianCho(num);
				tMProxyData.LastChange = DateTime.Now.AddDays(-1.0);
				TMProxyData.Add(text, tMProxyData);
			}
			bllTMProxy.api_key = text;
			List<int> list = new bllProxy().LayTinhTP(Misc.ToString(dtProfile.Rows[rowIndex]["TinhTP"]));
			if (list != null && list.Count > 0)
			{
				bllTMProxy.location = GetRandomValue(list);
			}
			else
			{
				bllTMProxy.location = 1;
			}
			int num2 = Misc.ObjInt((DateTime.Now - TMProxyData[text].LastChange).TotalSeconds);
			if (num2 > TMProxyData[text].ThoiGianCho)
			{
				TMProxyData[text].DangLay = true;
				bool flag = false;
				try
				{
					flag = bllTMProxy.changeProxy();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					return;
				}
				TMProxyData[text].DangLay = false;
				if (flag)
				{
					TMProxyData[text].LastChange = DateTime.Now;
					ProxyItems proxyItems = new ProxyItems();
					proxyItems.fullProxy = bllTMProxy.sock5;
					proxyItems.ip = bllTMProxy.ip;
					proxyItems.port = Misc.ToString(bllTMProxy.socks5_port);
					proxyItems.proxyPort = bllProxy.ProxyPort.SocksIPv4;
					proxyItems.TypeProxy = "TMProxy (Socks5)";
					TMProxyData[text].LastProxy = proxyItems;
					text2 = bllTMProxy.ip;
					proxyPort = bllTMProxy.sock5;
					proxyfree = " --host-resolver-rules=\"MAP * 0.0.0.0 , EXCLUDE " + text2;
					ProxyAllType proxyAllType = new ProxyAllType();
					proxyAllType.Supplier = 2;
					proxyAllType.ip = proxyItems.ip;
					proxyAllType.port = proxyItems.port;
					proxyAllType.Key = text;
					proxyAllType.Location = Misc.ToString(bllTMProxy.location);
					proxyAllType.TypeProxy = num;
					ChangeProxyProfile(rowIndex, proxyAllType);
				}
				else
				{
					string errorCode = bllTMProxy.errorCode;
					MessageBox.Show(errorCode);
				}
			}
			else
			{
				ProxyItems lastProxy = TMProxyData[text].LastProxy;
				text2 = lastProxy.ip;
				proxyPort = lastProxy.fullProxy;
				proxyfree = " --host-resolver-rules=\"MAP * 0.0.0.0 , EXCLUDE " + text2;
			}
			break;
		}
		}
		string IDorbita = "";
		if (!Directory.Exists(FolderPath + "\\" + idprofile))
		{
			return;
		}
		Thread thread = new Thread((ThreadStart)delegate
		{
			if (!string.IsNullOrEmpty(proxy))
			{
				Process process3 = new Process();
				process3.StartInfo = new ProcessStartInfo(BrowserPath ?? "");
				process3.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
				process3.StartInfo.Arguments = " --user-data-dir=\"" + FolderPath + "\\" + idprofile + "\" --proxy-server=" + proxyPort + proxyfree;
				process3.Start();
				try
				{
					IDorbita = process3.Id.ToString();
				}
				catch
				{
					IDorbita = "";
				}
				grdProfile.Invoke((MethodInvoker)delegate
				{
					UpdateStatus(rowIndex, "Đang mở");
				});
				Thread thread2 = new Thread((ThreadStart)delegate
				{
					process3.WaitForExit();
					process3.Close();
					try
					{
						grdProfile.Invoke((MethodInvoker)delegate
						{
							UpdateStatus(rowIndex, "Sẵn sàng");
						});
					}
					catch
					{
					}
				});
				thread2.Start();
			}
			else
			{
				Process process2 = new Process();
				process2.StartInfo = new ProcessStartInfo(BrowserPath ?? "");
				process2.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
				process2.StartInfo.Arguments = " --user-data-dir=\"" + FolderPath + "\\" + idprofile;
				process2.Start();
				try
				{
					IDorbita = process2.Id.ToString();
				}
				catch (Exception)
				{
					IDorbita = "";
				}
				grdProfile.Invoke((MethodInvoker)delegate
				{
					UpdateStatus(rowIndex, "Đang mở");
				});
				Thread thread3 = new Thread((ThreadStart)delegate
				{
					process2.WaitForExit();
					process2.Close();
					try
					{
						grdProfile.Invoke((MethodInvoker)delegate
						{
							UpdateStatus(rowIndex, "Sẵn sàng");
						});
					}
					catch
					{
					}
				});
				thread3.Start();
			}
		});
		thread.Start();
	}

	private int GetRandomValue(List<int> lstKeyword)
	{
		if (lstKeyword == null || lstKeyword.Count == 0)
		{
			return 0;
		}
		int index = rand.Next(0, lstKeyword.Count);
		return lstKeyword[index];
	}

	private void UpdateStatus(int rowIndex, string Status)
	{
		if (dtProfile != null && dtProfile.Rows.Count >= rowIndex + 1)
		{
			dtProfile.Rows[rowIndex]["Status"] = Status;
		}
	}

	private void DeleteProfile(int rowIndex)
	{
		if (string.IsNullOrEmpty(BrowserPath))
		{
			MessageBox.Show(this, "Chưa cấu hình đường dẫn trình duyệt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		string text = ToString(dtProfile.Rows[rowIndex]["ProfileName"]);
		try
		{
			if (Directory.Exists(FolderPath + "\\" + text))
			{
				Directory.Delete(FolderPath + "\\" + text, recursive: true);
			}
			if (dtProfile.Rows.Count > rowIndex)
			{
				dtProfile.Rows[rowIndex].Delete();
				dtProfile.AcceptChanges();
				string fileName = FolderPath + "\\" + DataSourceFileName;
				dtProfile.WriteXml(fileName, XmlWriteMode.WriteSchema);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdProfile_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdProfile_MouseClick(object sender, MouseEventArgs e)
	{
	}

	private void grdProfile_MouseDown(object sender, MouseEventArgs e)
	{
		try
		{
			if (grdProfile != null)
			{
				DataGridView.HitTestInfo hitTestInfo = grdProfile.HitTest(e.X, e.Y);
				if (e.Button == MouseButtons.Right && hitTestInfo != null && dtProfile != null && dtProfile.Rows.Count > 0)
				{
					mniOpenProfile.Tag = hitTestInfo.RowIndex;
					mniDeleteProfile.Tag = hitTestInfo.RowIndex;
					mniEditProfile.Tag = hitTestInfo.RowIndex;
					mnuProfile.Show(Cursor.Position);
					mniDeleteProfile.Visible = ToString(dtProfile.Rows[hitTestInfo.RowIndex]["Status"]) == "Sẵn sàng";
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void mniOpenProfile_Click(object sender, EventArgs e)
	{
		try
		{
			int rowIndex = int.Parse(ToString(mniOpenProfile.Tag));
			OpenProfile(rowIndex);
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void mniDeleteProfile_Click(object sender, EventArgs e)
	{
		try
		{
			int rowIndex = int.Parse(ToString(mniDeleteProfile.Tag));
			DeleteProfile(rowIndex);
			frmCreateProfile_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void mniEditProfile_Click(object sender, EventArgs e)
	{
		try
		{
			int rowIndex = int.Parse(ToString(mniEditProfile.Tag));
			LoadProfileToControl(rowIndex);
			btnUpdate.Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void LoadProfileToControl(int rowIndex)
	{
		if (string.IsNullOrEmpty(BrowserPath))
		{
			MessageBox.Show(this, "Chưa cấu hình đường dẫn trình duyệt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		CurrentIndex = rowIndex;
		string text = ToString(dtProfile.Rows[rowIndex]["ProfileName"]);
		currentFocusProxy = new bllProxy().GetProxyFromData(dtProfile.Rows[rowIndex]);
		txtProfileName.Text = text;
		if (currentFocusProxy != null)
		{
			cbeProxySupplier.EditValue = currentFocusProxy.Supplier;
			lueProxyType.EditValue = currentFocusProxy.TypeProxy;
			if (currentFocusProxy.Supplier == 0)
			{
				txtIP.Text = currentFocusProxy.ip;
			}
			else
			{
				txtIP.Text = currentFocusProxy.Key;
			}
			ccbTinhTP.EditValue = currentFocusProxy.Location;
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (CurrentIndex < 0 || dtProfile.Rows.Count <= CurrentIndex)
			{
				MessageBox.Show(this, "Chưa chọn Profile để chỉnh sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			ProxyAllType proxyFromControl = GetProxyFromControl();
			ChangeProxyProfile(CurrentIndex, proxyFromControl);
			MessageBox.Show(this, "Đã cập nhật xong!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			btnUpdate.Enabled = false;
			frmCreateProfile_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ChangeProxyProfile(int CurrentIndex, ProxyAllType newProxy)
	{
		if (CurrentIndex < 0 || dtProfile.Rows.Count <= CurrentIndex)
		{
			MessageBox.Show(this, "Chưa chọn Profile để chỉnh sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		if (newProxy == null)
		{
			MessageBox.Show(this, "Chưa có thông tin proxy mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		string text = ToString(dtProfile.Rows[CurrentIndex]["ProfileName"]);
		string text2 = FolderPath + "\\" + text;
		if (!Directory.Exists(text2))
		{
			MessageBox.Show(this, "Profile này đã bị xóa hoặc không tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		string text3 = File.ReadAllText(text2 + "\\Default\\Preferences");
		ProxyAllType proxyFromData = new bllProxy().GetProxyFromData(dtProfile.Rows[CurrentIndex]);
		if (proxyFromData != null)
		{
			if (!string.IsNullOrEmpty(newProxy.ip))
			{
				text3 = text3.Replace("\"public_ip\": \"" + proxyFromData.ip + "\"", "\"public_ip\": \"" + newProxy.ip + "\"");
			}
			if (!string.IsNullOrEmpty(newProxy.userName))
			{
				text3 = text3.Replace("\"username\": \"" + proxyFromData.userName + "\"", "\"username\": \"" + newProxy.userName + "\"");
			}
			if (!string.IsNullOrEmpty(newProxy.passWord))
			{
				text3 = text3.Replace("\"password\": \"" + proxyFromData.passWord + "\"", "\"password\": \"" + newProxy.passWord + "\"");
			}
		}
		else
		{
			text3 = text3.Replace("\"public_ip\": \"\"", "\"public_ip\": \"" + newProxy.ip + "\"");
			text3 = text3.Replace("\"username\": \"\"", "\"username\": \"" + newProxy.userName + "\"");
			text3 = text3.Replace("\"password\": \"\"", "\"password\": \"" + newProxy.passWord + "\"");
		}
		File.WriteAllText(text2 + "\\Default\\Preferences", text3);
		switch (newProxy.Supplier)
		{
		case 0:
			dtProfile.Rows[CurrentIndex]["Proxy"] = (string.IsNullOrEmpty(newProxy.userName) ? (newProxy.ip + ":" + newProxy.port) : (newProxy.ip + ":" + newProxy.port + ":" + newProxy.userName + ":" + newProxy.passWord));
			break;
		case 1:
		case 2:
			dtProfile.Rows[CurrentIndex]["Key"] = newProxy.Key;
			dtProfile.Rows[CurrentIndex]["NCCProxy"] = newProxy.Supplier;
			dtProfile.Rows[CurrentIndex]["LoaiProxy"] = newProxy.TypeProxy;
			dtProfile.Rows[CurrentIndex]["TinhTP"] = newProxy.Location;
			dtProfile.Rows[CurrentIndex]["proxy"] = newProxy.ip + ":" + newProxy.port;
			break;
		}
		dtProfile.AcceptChanges();
		string fileName = FolderPath + "\\" + DataSourceFileName;
		dtProfile.WriteXml(fileName, XmlWriteMode.WriteSchema);
	}

	private void btnCreateProfileSLL_Click(object sender, EventArgs e)
	{
		try
		{
			lbeStatusCreateSLL.Visible = true;
			if (Misc.ToString(btnCreateProfileSLL.Tag).Equals("pause"))
			{
				wkCreateProfile.CancelAsync();
				btnCreateProfileSLL.Tag = "play";
			}
			else
			{
				if (!Misc.ToString(btnCreateProfileSLL.Tag).Equals("play"))
				{
					return;
				}
				SoLuong = Misc.ObjInt(speSoLuong.Value);
				btnCreateProfileSLL.Text = "Tạm dừng";
				btnCreateProfileSLL.Tag = "pause";
				if (!string.IsNullOrEmpty(txtIP.Text))
				{
					switch (cbeProxySupplier.SelectedIndex)
					{
					case 0:
					{
						List<string> list = SplitToList(txtIP.Text, ":");
						proxySLL = new ProxyNATech();
						if (list.Count >= 2)
						{
							proxySLL.Ip = list[0];
							proxySLL.Port = list[1];
							if (list.Count >= 4)
							{
								proxySLL.UserName = list[2];
								proxySLL.Password = list[3];
							}
						}
						break;
					}
					case 1:
						proxyRotationSLL = new ProxyRotation(1, Misc.ObjInt(lueProxyType.EditValue), txtIP.Text.Trim(), Misc.ToString(ccbTinhTP.EditValue));
						break;
					case 2:
						proxyRotationSLL = new ProxyRotation(2, Misc.ObjInt(lueProxyType.EditValue), txtIP.Text.Trim(), Misc.ToString(ccbTinhTP.EditValue));
						break;
					}
				}
				wkCreateProfile.RunWorkerAsync();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void wkCreateProfile_DoWork(object sender, DoWorkEventArgs e)
	{
		try
		{
			for (int i = 0; i < SoLuong; i++)
			{
				if (wkCreateProfile.CancellationPending)
				{
					break;
				}
				string uA = lstUserAgent[rand.Next(lstUserAgent.Count)];
				string empty = string.Empty;
				string ProfileName = string.Empty;
				NATechDriver.CreateProfile(ProfileDefaultPath, FolderPath, ref ProfileName, uA, proxySLL, BrowserLanguage, ref empty);
				if (!string.IsNullOrEmpty(empty))
				{
					MessageBox.Show(this, empty, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					break;
				}
				string profilePath = FolderPath + "\\" + ProfileName;
				SaveData(ProfileName, profilePath, uA, proxySLL, proxyRotationSLL);
				wkCreateProfile.ReportProgress(i);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void wkCreateProfile_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		try
		{
			lbeStatusCreateSLL.Text = $"Đã tạo {e.ProgressPercentage + 1}/{SoLuong} profiles";
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void wkCreateProfile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		try
		{
			btnCreateProfileSLL.Text = "Tạo profile hàng loạt";
			btnCreateProfileSLL.Tag = "play";
			MessageBox.Show(this, "Đã tạo xong!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			frmCreateProfile_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdProfile_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex > -1)
			{
				OpenProfile(e.RowIndex);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCreateProfileEmail_Click(object sender, EventArgs e)
	{
		try
		{
			frmCreateProfileEmails frmCreateProfileEmails2 = new frmCreateProfileEmails();
			frmCreateProfileEmails2.lstUserAgent = lstUserAgent;
			frmCreateProfileEmails2.FolderPath = FolderPath;
			frmCreateProfileEmails2.BrowserPath = BrowserPath;
			frmCreateProfileEmails2.ProfileDefaultPath = ProfileDefaultPath;
			frmCreateProfileEmails2.BrowserLanguage = BrowserLanguage;
			frmCreateProfileEmails2.ShowDialog(this);
			LoadDataBase();
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private ProxyAllType GetProxyFromControl()
	{
		ProxyAllType proxyAllType = null;
		if (!string.IsNullOrEmpty(txtIP.Text))
		{
			switch (cbeProxySupplier.SelectedIndex)
			{
			case 0:
			{
				List<string> list = SplitToList(txtIP.Text, ":");
				proxyAllType = new ProxyAllType();
				if (list.Count >= 2)
				{
					proxyAllType.Supplier = 0;
					proxyAllType.ip = list[0];
					proxyAllType.port = list[1];
					if (list.Count >= 4)
					{
						proxyAllType.userName = list[2];
						proxyAllType.passWord = list[3];
					}
				}
				break;
			}
			case 1:
				proxyAllType = new ProxyAllType();
				proxyAllType.Supplier = 1;
				proxyAllType.Key = txtIP.Text.Trim();
				proxyAllType.TypeProxy = Misc.ObjInt(lueProxyType.EditValue);
				proxyAllType.Location = Misc.ToString(ccbTinhTP.EditValue);
				break;
			case 2:
				proxyAllType = new ProxyAllType();
				proxyAllType.Supplier = 2;
				proxyAllType.Key = txtIP.Text.Trim();
				proxyAllType.TypeProxy = Misc.ObjInt(lueProxyType.EditValue);
				proxyAllType.Location = Misc.ToString(ccbTinhTP.EditValue);
				break;
			}
		}
		return proxyAllType;
	}

	private void btnCreate_Click(object sender, EventArgs e)
	{
		try
		{
			string ProfileName = txtProfileName.Text.Trim();
			if (ApiClientNATech.loginResponse == null || string.IsNullOrEmpty(ApiClientNATech.loginResponse.Token))
			{
				MessageBox.Show(this, "Chưa đăng nhập không dùng được tính năng này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			string path = FolderPath + "\\" + ProfileName;
			if (!string.IsNullOrEmpty(ProfileName) && Directory.Exists(path))
			{
				MessageBox.Show(this, "Đã tồn tại ProfileName này rồi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			lstUserAgent = SplitToList(memUserAgent.Text, "\n");
			if (lstUserAgent == null || lstUserAgent.Count == 0)
			{
				MessageBox.Show(this, "Chưa có danh sách User Agent", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (IsMain)
			{
				SaveUserAgent();
			}
			string uA = lstUserAgent[rand.Next(lstUserAgent.Count)];
			ProxyNATech proxyNATech = null;
			ProxyRotation proxyRotation = null;
			if (!string.IsNullOrEmpty(txtIP.Text))
			{
				switch (cbeProxySupplier.SelectedIndex)
				{
				case 0:
				{
					List<string> list = SplitToList(txtIP.Text, ":");
					proxyNATech = new ProxyNATech();
					if (list.Count >= 2)
					{
						proxyNATech.Ip = list[0];
						proxyNATech.Port = list[1];
						if (list.Count >= 4)
						{
							proxyNATech.UserName = list[2];
							proxyNATech.Password = list[3];
						}
					}
					break;
				}
				case 1:
					proxyRotation = new ProxyRotation(1, Misc.ObjInt(lueProxyType.EditValue), txtIP.Text.Trim(), Misc.ToString(ccbTinhTP.EditValue));
					break;
				case 2:
					proxyRotation = new ProxyRotation(2, Misc.ObjInt(lueProxyType.EditValue), txtIP.Text.Trim(), Misc.ToString(ccbTinhTP.EditValue));
					break;
				}
			}
			string empty = string.Empty;
			NATechDriver.CreateProfile(ProfileDefaultPath, FolderPath, ref ProfileName, uA, proxyNATech, BrowserLanguage, ref empty);
			if (!string.IsNullOrEmpty(empty))
			{
				MessageBox.Show(this, empty, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			path = FolderPath + "\\" + ProfileName;
			SaveData(ProfileName, path, uA, proxyNATech, proxyRotation);
			frmCreateProfile_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cbeProxyType_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			switch (cbeProxySupplier.SelectedIndex)
			{
			case 0:
			{
				LayoutControlItem layoutControlItem3 = lciProxyType;
				LayoutVisibility visibility = (lciTinhTP.Visibility = LayoutVisibility.Always);
				layoutControlItem3.Visibility = visibility;
				LayoutControlItem layoutControlItem4 = lciProxyType;
				visibility = (lciTinhTP.Visibility = LayoutVisibility.Never);
				layoutControlItem4.Visibility = visibility;
				lciProxyType.Text = "Loại proxy";
				lciIP.Text = "Proxy";
				break;
			}
			case 1:
			{
				LayoutControlItem layoutControlItem2 = lciProxyType;
				LayoutVisibility visibility = (lciTinhTP.Visibility = LayoutVisibility.Always);
				layoutControlItem2.Visibility = visibility;
				lciIP.Text = "Key Tinsoft";
				dtTinsoftLocation = new TinsoftProxy().getLocations();
				ccbTinhTP.Properties.DisplayMember = "name";
				ccbTinhTP.Properties.ValueMember = "location";
				ccbTinhTP.Properties.DataSource = dtTinsoftLocation;
				dtTinsoftType = new TinsoftProxy().GetProxyType();
				lueProxyType.Properties.DataSource = dtTinsoftType;
				break;
			}
			case 2:
			{
				LayoutControlItem layoutControlItem = lciProxyType;
				LayoutVisibility visibility = (lciTinhTP.Visibility = LayoutVisibility.Always);
				layoutControlItem.Visibility = visibility;
				lciIP.Text = "Key TMProxy";
				dtTMLocation = new bllTMProxy().getLocations(out sErr);
				ccbTinhTP.Properties.DataSource = dtTMLocation;
				dtTMType = new bllTMProxy().GetProxyType();
				lueProxyType.Properties.DataSource = dtTMType;
				break;
			}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateProfile));
		this.grdProfile = new System.Windows.Forms.DataGridView();
		this.mnuProfile = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.mniOpenProfile = new System.Windows.Forms.ToolStripMenuItem();
		this.mniDeleteProfile = new System.Windows.Forms.ToolStripMenuItem();
		this.mniEditProfile = new System.Windows.Forms.ToolStripMenuItem();
		this.wkCreateProfile = new System.ComponentModel.BackgroundWorker();
		this.locMain = new DevExpress.XtraLayout.LayoutControl();
		this.lueProxyType = new DevExpress.XtraEditors.GridLookUpEdit();
		this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
		this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
		this.memUserAgent = new DevExpress.XtraEditors.MemoEdit();
		this.btnCreateProfileEmail = new DevExpress.XtraEditors.SimpleButton();
		this.btnUpdate = new DevExpress.XtraEditors.SimpleButton();
		this.lbeStatusCreateSLL = new DevExpress.XtraEditors.LabelControl();
		this.btnCreateProfileSLL = new DevExpress.XtraEditors.SimpleButton();
		this.speSoLuong = new DevExpress.XtraEditors.SpinEdit();
		this.ccbTinhTP = new DevExpress.XtraEditors.CheckedComboBoxEdit();
		this.txtIP = new DevExpress.XtraEditors.TextEdit();
		this.cbeProxySupplier = new DevExpress.XtraEditors.ComboBoxEdit();
		this.btnCreate = new DevExpress.XtraEditors.SimpleButton();
		this.txtProfileName = new DevExpress.XtraEditors.TextEdit();
		this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
		this.lciProfileName = new DevExpress.XtraLayout.LayoutControlItem();
		this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
		this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
		this.lciProxySupplier = new DevExpress.XtraLayout.LayoutControlItem();
		this.lciIP = new DevExpress.XtraLayout.LayoutControlItem();
		this.lciTinhTP = new DevExpress.XtraLayout.LayoutControlItem();
		this.lciProxyType = new DevExpress.XtraLayout.LayoutControlItem();
		this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
		this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
		this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
		this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
		this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
		this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
		this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
		this.emptySpaceItem6 = new DevExpress.XtraLayout.EmptySpaceItem();
		this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
		this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
		this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
		this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
		this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
		this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
		this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
		this.ProfileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.UA = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Proxy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProfilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.grdNCCProxy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.KeyProxy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.LoaiProxy = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TinhTP = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.GmailInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.GmailLoginStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
		((System.ComponentModel.ISupportInitialize)this.grdProfile).BeginInit();
		this.mnuProfile.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.locMain).BeginInit();
		this.locMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.lueProxyType.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.memUserAgent.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.speSoLuong.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ccbTinhTP.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtIP.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cbeProxySupplier.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtProfileName.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lciProfileName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlGroup2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lciProxySupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lciIP).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lciTinhTP).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lciProxyType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlGroup3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.emptySpaceItem3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.emptySpaceItem5).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.emptySpaceItem6).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.emptySpaceItem4).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.splitterItem1).BeginInit();
		base.SuspendLayout();
		this.grdProfile.AllowUserToAddRows = false;
		this.grdProfile.AllowUserToDeleteRows = false;
		this.grdProfile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grdProfile.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.grdProfile.BackgroundColor = System.Drawing.Color.White;
		this.grdProfile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.grdProfile.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 224, 192);
		dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 163);
		dataGridViewCellStyle.ForeColor = System.Drawing.Color.Maroon;
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.grdProfile.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
		this.grdProfile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.grdProfile.Columns.AddRange(this.ProfileName, this.UA, this.Proxy, this.ProfilePath, this.grdNCCProxy, this.KeyProxy, this.LoaiProxy, this.TinhTP, this.Status, this.GmailInfo, this.GmailLoginStatus);
		this.grdProfile.GridColor = System.Drawing.Color.SkyBlue;
		this.grdProfile.Location = new System.Drawing.Point(3, 518);
		this.grdProfile.Name = "grdProfile";
		this.grdProfile.ReadOnly = true;
		this.grdProfile.RowHeadersWidth = 62;
		this.grdProfile.RowTemplate.Height = 28;
		this.grdProfile.Size = new System.Drawing.Size(1835, 694);
		this.grdProfile.TabIndex = 4;
		this.grdProfile.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(grdProfile_CellContentClick);
		this.grdProfile.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(grdProfile_CellContentDoubleClick);
		this.grdProfile.MouseClick += new System.Windows.Forms.MouseEventHandler(grdProfile_MouseClick);
		this.grdProfile.MouseDown += new System.Windows.Forms.MouseEventHandler(grdProfile_MouseDown);
		this.mnuProfile.ImageScalingSize = new System.Drawing.Size(24, 24);
		this.mnuProfile.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.mniOpenProfile, this.mniDeleteProfile, this.mniEditProfile });
		this.mnuProfile.Name = "mnuProfile";
		this.mnuProfile.Size = new System.Drawing.Size(218, 100);
		this.mnuProfile.Text = "Profile menu";
		this.mniOpenProfile.Name = "mniOpenProfile";
		this.mniOpenProfile.Size = new System.Drawing.Size(217, 32);
		this.mniOpenProfile.Text = "Mở profile";
		this.mniOpenProfile.Click += new System.EventHandler(mniOpenProfile_Click);
		this.mniDeleteProfile.Name = "mniDeleteProfile";
		this.mniDeleteProfile.Size = new System.Drawing.Size(217, 32);
		this.mniDeleteProfile.Text = "Xóa profile";
		this.mniDeleteProfile.Click += new System.EventHandler(mniDeleteProfile_Click);
		this.mniEditProfile.Name = "mniEditProfile";
		this.mniEditProfile.Size = new System.Drawing.Size(217, 32);
		this.mniEditProfile.Text = "Chỉnh sửa Profile";
		this.mniEditProfile.Click += new System.EventHandler(mniEditProfile_Click);
		this.wkCreateProfile.WorkerReportsProgress = true;
		this.wkCreateProfile.WorkerSupportsCancellation = true;
		this.wkCreateProfile.DoWork += new System.ComponentModel.DoWorkEventHandler(wkCreateProfile_DoWork);
		this.wkCreateProfile.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(wkCreateProfile_ProgressChanged);
		this.wkCreateProfile.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(wkCreateProfile_RunWorkerCompleted);
		this.locMain.Controls.Add(this.lueProxyType);
		this.locMain.Controls.Add(this.memUserAgent);
		this.locMain.Controls.Add(this.btnCreateProfileEmail);
		this.locMain.Controls.Add(this.grdProfile);
		this.locMain.Controls.Add(this.btnUpdate);
		this.locMain.Controls.Add(this.lbeStatusCreateSLL);
		this.locMain.Controls.Add(this.btnCreateProfileSLL);
		this.locMain.Controls.Add(this.speSoLuong);
		this.locMain.Controls.Add(this.ccbTinhTP);
		this.locMain.Controls.Add(this.txtIP);
		this.locMain.Controls.Add(this.cbeProxySupplier);
		this.locMain.Controls.Add(this.btnCreate);
		this.locMain.Controls.Add(this.txtProfileName);
		this.locMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.locMain.Location = new System.Drawing.Point(0, 0);
		this.locMain.Name = "locMain";
		this.locMain.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(601, 524, 675, 600);
		this.locMain.Root = this.layoutControlGroup1;
		this.locMain.Size = new System.Drawing.Size(1841, 1215);
		this.locMain.TabIndex = 12;
		this.locMain.Text = "layoutControl1";
		this.lueProxyType.Location = new System.Drawing.Point(793, 67);
		this.lueProxyType.Name = "lueProxyType";
		this.lueProxyType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.lueProxyType.Properties.DisplayMember = "NAME";
		this.lueProxyType.Properties.NullText = "";
		this.lueProxyType.Properties.ValueMember = "ID";
		this.lueProxyType.Properties.View = this.gridLookUpEdit1View;
		this.lueProxyType.Size = new System.Drawing.Size(612, 26);
		this.lueProxyType.StyleController = this.locMain;
		this.lueProxyType.TabIndex = 17;
		this.gridLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[1] { this.gridColumn1 });
		this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
		this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
		this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
		this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
		this.gridColumn1.Caption = "Loại key";
		this.gridColumn1.FieldName = "NAME";
		this.gridColumn1.Name = "gridColumn1";
		this.gridColumn1.Visible = true;
		this.gridColumn1.VisibleIndex = 0;
		this.memUserAgent.Location = new System.Drawing.Point(3, 198);
		this.memUserAgent.Name = "memUserAgent";
		this.memUserAgent.Size = new System.Drawing.Size(1835, 282);
		this.memUserAgent.StyleController = this.locMain;
		this.memUserAgent.TabIndex = 16;
		this.btnCreateProfileEmail.Location = new System.Drawing.Point(760, 136);
		this.btnCreateProfileEmail.Name = "btnCreateProfileEmail";
		this.btnCreateProfileEmail.Size = new System.Drawing.Size(293, 32);
		this.btnCreateProfileEmail.StyleController = this.locMain;
		this.btnCreateProfileEmail.TabIndex = 15;
		this.btnCreateProfileEmail.Text = "Tạo profile và Login theo Gmail";
		this.btnCreateProfileEmail.Click += new System.EventHandler(btnCreateProfileEmail_Click);
		this.btnUpdate.Location = new System.Drawing.Point(551, 136);
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(203, 32);
		this.btnUpdate.StyleController = this.locMain;
		this.btnUpdate.TabIndex = 14;
		this.btnUpdate.Text = "Cập nhật";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.lbeStatusCreateSLL.Appearance.ForeColor = System.Drawing.Color.Blue;
		this.lbeStatusCreateSLL.Appearance.Options.UseForeColor = true;
		this.lbeStatusCreateSLL.Location = new System.Drawing.Point(1421, 105);
		this.lbeStatusCreateSLL.Name = "lbeStatusCreateSLL";
		this.lbeStatusCreateSLL.Size = new System.Drawing.Size(69, 19);
		this.lbeStatusCreateSLL.StyleController = this.locMain;
		this.lbeStatusCreateSLL.TabIndex = 13;
		this.lbeStatusCreateSLL.Text = "Tiến trình";
		this.btnCreateProfileSLL.Location = new System.Drawing.Point(1555, 67);
		this.btnCreateProfileSLL.Name = "btnCreateProfileSLL";
		this.btnCreateProfileSLL.Size = new System.Drawing.Size(160, 32);
		this.btnCreateProfileSLL.StyleController = this.locMain;
		this.btnCreateProfileSLL.TabIndex = 12;
		this.btnCreateProfileSLL.Tag = "play";
		this.btnCreateProfileSLL.Text = "Tạo hàng loạt";
		this.btnCreateProfileSLL.Click += new System.EventHandler(btnCreateProfileSLL_Click);
		this.speSoLuong.EditValue = new decimal(new int[4]);
		this.speSoLuong.Location = new System.Drawing.Point(1578, 35);
		this.speSoLuong.Name = "speSoLuong";
		this.speSoLuong.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.speSoLuong.Size = new System.Drawing.Size(255, 26);
		this.speSoLuong.StyleController = this.locMain;
		this.speSoLuong.TabIndex = 11;
		this.ccbTinhTP.EditValue = "";
		this.ccbTinhTP.Location = new System.Drawing.Point(792, 99);
		this.ccbTinhTP.Name = "ccbTinhTP";
		this.ccbTinhTP.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.ccbTinhTP.Properties.DisplayMember = "name";
		this.ccbTinhTP.Properties.ValueMember = "location";
		this.ccbTinhTP.Size = new System.Drawing.Size(613, 26);
		this.ccbTinhTP.StyleController = this.locMain;
		this.ccbTinhTP.TabIndex = 10;
		this.txtIP.Location = new System.Drawing.Point(165, 99);
		this.txtIP.Name = "txtIP";
		this.txtIP.Size = new System.Drawing.Size(559, 26);
		this.txtIP.StyleController = this.locMain;
		this.txtIP.TabIndex = 9;
		this.cbeProxySupplier.Location = new System.Drawing.Point(165, 67);
		this.cbeProxySupplier.Name = "cbeProxySupplier";
		this.cbeProxySupplier.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.cbeProxySupplier.Properties.Items.AddRange(new object[3] { "Proxy tĩnh: IP:PORT:USER:PASS", "Tinsoft Proxy", "TMProxy" });
		this.cbeProxySupplier.Size = new System.Drawing.Size(559, 26);
		this.cbeProxySupplier.StyleController = this.locMain;
		this.cbeProxySupplier.TabIndex = 6;
		this.cbeProxySupplier.SelectedIndexChanged += new System.EventHandler(cbeProxyType_SelectedIndexChanged);
		this.btnCreate.Location = new System.Drawing.Point(381, 136);
		this.btnCreate.Name = "btnCreate";
		this.btnCreate.Size = new System.Drawing.Size(164, 32);
		this.btnCreate.StyleController = this.locMain;
		this.btnCreate.TabIndex = 5;
		this.btnCreate.Text = "Tạo profile";
		this.btnCreate.Click += new System.EventHandler(btnCreate_Click);
		this.txtProfileName.Location = new System.Drawing.Point(160, 3);
		this.txtProfileName.Name = "txtProfileName";
		this.txtProfileName.Size = new System.Drawing.Size(1250, 26);
		this.txtProfileName.StyleController = this.locMain;
		this.txtProfileName.TabIndex = 4;
		this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
		this.layoutControlGroup1.GroupBordersVisible = false;
		this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[11]
		{
			this.lciProfileName, this.emptySpaceItem1, this.layoutControlGroup2, this.layoutControlGroup3, this.layoutControlItem2, this.layoutControlItem5, this.layoutControlItem6, this.emptySpaceItem4, this.layoutControlItem7, this.layoutControlItem8,
			this.splitterItem1
		});
		this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
		this.layoutControlGroup1.Name = "Root";
		this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
		this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
		this.layoutControlGroup1.Size = new System.Drawing.Size(1841, 1215);
		this.layoutControlGroup1.TextVisible = false;
		this.lciProfileName.Control = this.txtProfileName;
		this.lciProfileName.Location = new System.Drawing.Point(0, 0);
		this.lciProfileName.Name = "lciProfileName";
		this.lciProfileName.Size = new System.Drawing.Size(1413, 32);
		this.lciProfileName.Text = "Tên profile";
		this.lciProfileName.TextSize = new System.Drawing.Size(152, 19);
		this.emptySpaceItem1.AllowHotTrack = false;
		this.emptySpaceItem1.Location = new System.Drawing.Point(1056, 133);
		this.emptySpaceItem1.Name = "emptySpaceItem1";
		this.emptySpaceItem1.Size = new System.Drawing.Size(357, 38);
		this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
		this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[4] { this.lciProxySupplier, this.lciIP, this.lciTinhTP, this.lciProxyType });
		this.layoutControlGroup2.Location = new System.Drawing.Point(0, 32);
		this.layoutControlGroup2.Name = "layoutControlGroup2";
		this.layoutControlGroup2.OptionsItemText.TextToControlDistance = 5;
		this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
		this.layoutControlGroup2.Size = new System.Drawing.Size(1413, 101);
		this.layoutControlGroup2.Text = "Proxy";
		this.lciProxySupplier.Control = this.cbeProxySupplier;
		this.lciProxySupplier.Location = new System.Drawing.Point(0, 0);
		this.lciProxySupplier.Name = "lciProxySupplier";
		this.lciProxySupplier.Size = new System.Drawing.Size(722, 32);
		this.lciProxySupplier.Text = "NCC Proxy";
		this.lciProxySupplier.TextSize = new System.Drawing.Size(152, 19);
		this.lciIP.Control = this.txtIP;
		this.lciIP.Location = new System.Drawing.Point(0, 32);
		this.lciIP.Name = "lciIP";
		this.lciIP.Size = new System.Drawing.Size(722, 32);
		this.lciIP.Text = "Proxy";
		this.lciIP.TextSize = new System.Drawing.Size(152, 19);
		this.lciTinhTP.Control = this.ccbTinhTP;
		this.lciTinhTP.Location = new System.Drawing.Point(722, 32);
		this.lciTinhTP.Name = "lciTinhTP";
		this.lciTinhTP.Size = new System.Drawing.Size(681, 32);
		this.lciTinhTP.Text = "Tỉnh/TP";
		this.lciTinhTP.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
		this.lciTinhTP.TextSize = new System.Drawing.Size(57, 19);
		this.lciTinhTP.TextToControlDistance = 5;
		this.lciProxyType.Control = this.lueProxyType;
		this.lciProxyType.Location = new System.Drawing.Point(722, 0);
		this.lciProxyType.Name = "lciProxyType";
		this.lciProxyType.Size = new System.Drawing.Size(681, 32);
		this.lciProxyType.Text = "Loại key";
		this.lciProxyType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
		this.lciProxyType.TextSize = new System.Drawing.Size(58, 19);
		this.lciProxyType.TextToControlDistance = 5;
		this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[7] { this.layoutControlItem1, this.emptySpaceItem3, this.layoutControlItem3, this.layoutControlItem4, this.emptySpaceItem2, this.emptySpaceItem5, this.emptySpaceItem6 });
		this.layoutControlGroup3.Location = new System.Drawing.Point(1413, 0);
		this.layoutControlGroup3.Name = "layoutControlGroup3";
		this.layoutControlGroup3.OptionsItemText.TextToControlDistance = 5;
		this.layoutControlGroup3.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
		this.layoutControlGroup3.Size = new System.Drawing.Size(428, 171);
		this.layoutControlGroup3.Text = "Tạo profile hàng loạt";
		this.layoutControlItem1.Control = this.speSoLuong;
		this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
		this.layoutControlItem1.Name = "layoutControlItem1";
		this.layoutControlItem1.Size = new System.Drawing.Size(418, 32);
		this.layoutControlItem1.Text = "Số lượng";
		this.layoutControlItem1.TextSize = new System.Drawing.Size(152, 19);
		this.emptySpaceItem3.AllowHotTrack = false;
		this.emptySpaceItem3.Location = new System.Drawing.Point(0, 95);
		this.emptySpaceItem3.Name = "emptySpaceItem3";
		this.emptySpaceItem3.Size = new System.Drawing.Size(418, 39);
		this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
		this.layoutControlItem3.Control = this.btnCreateProfileSLL;
		this.layoutControlItem3.Location = new System.Drawing.Point(134, 32);
		this.layoutControlItem3.Name = "layoutControlItem3";
		this.layoutControlItem3.Size = new System.Drawing.Size(166, 38);
		this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
		this.layoutControlItem3.TextVisible = false;
		this.layoutControlItem4.Control = this.lbeStatusCreateSLL;
		this.layoutControlItem4.Location = new System.Drawing.Point(0, 70);
		this.layoutControlItem4.Name = "layoutControlItem4";
		this.layoutControlItem4.Size = new System.Drawing.Size(75, 25);
		this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
		this.layoutControlItem4.TextVisible = false;
		this.emptySpaceItem2.AllowHotTrack = false;
		this.emptySpaceItem2.Location = new System.Drawing.Point(75, 70);
		this.emptySpaceItem2.Name = "emptySpaceItem2";
		this.emptySpaceItem2.Size = new System.Drawing.Size(343, 25);
		this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
		this.emptySpaceItem5.AllowHotTrack = false;
		this.emptySpaceItem5.Location = new System.Drawing.Point(0, 32);
		this.emptySpaceItem5.Name = "emptySpaceItem5";
		this.emptySpaceItem5.Size = new System.Drawing.Size(134, 38);
		this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
		this.emptySpaceItem6.AllowHotTrack = false;
		this.emptySpaceItem6.Location = new System.Drawing.Point(300, 32);
		this.emptySpaceItem6.Name = "emptySpaceItem6";
		this.emptySpaceItem6.Size = new System.Drawing.Size(118, 38);
		this.emptySpaceItem6.TextSize = new System.Drawing.Size(0, 0);
		this.layoutControlItem2.Control = this.btnCreate;
		this.layoutControlItem2.Location = new System.Drawing.Point(378, 133);
		this.layoutControlItem2.MinSize = new System.Drawing.Size(94, 38);
		this.layoutControlItem2.Name = "layoutControlItem2";
		this.layoutControlItem2.Size = new System.Drawing.Size(170, 38);
		this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
		this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
		this.layoutControlItem2.TextVisible = false;
		this.layoutControlItem5.Control = this.btnUpdate;
		this.layoutControlItem5.Location = new System.Drawing.Point(548, 133);
		this.layoutControlItem5.MinSize = new System.Drawing.Size(118, 38);
		this.layoutControlItem5.Name = "layoutControlItem5";
		this.layoutControlItem5.Size = new System.Drawing.Size(209, 38);
		this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
		this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
		this.layoutControlItem5.TextVisible = false;
		this.layoutControlItem6.Control = this.btnCreateProfileEmail;
		this.layoutControlItem6.Location = new System.Drawing.Point(757, 133);
		this.layoutControlItem6.MinSize = new System.Drawing.Size(118, 38);
		this.layoutControlItem6.Name = "layoutControlItem6";
		this.layoutControlItem6.Size = new System.Drawing.Size(299, 38);
		this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
		this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
		this.layoutControlItem6.TextVisible = false;
		this.emptySpaceItem4.AllowHotTrack = false;
		this.emptySpaceItem4.Location = new System.Drawing.Point(0, 133);
		this.emptySpaceItem4.Name = "emptySpaceItem4";
		this.emptySpaceItem4.Size = new System.Drawing.Size(378, 38);
		this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
		this.layoutControlItem7.Control = this.memUserAgent;
		this.layoutControlItem7.Location = new System.Drawing.Point(0, 171);
		this.layoutControlItem7.Name = "layoutControlItem7";
		this.layoutControlItem7.Size = new System.Drawing.Size(1841, 312);
		this.layoutControlItem7.Text = "Danh sách UserAgent";
		this.layoutControlItem7.TextLocation = DevExpress.Utils.Locations.Top;
		this.layoutControlItem7.TextSize = new System.Drawing.Size(152, 19);
		this.layoutControlItem8.Control = this.grdProfile;
		this.layoutControlItem8.Location = new System.Drawing.Point(0, 491);
		this.layoutControlItem8.Name = "layoutControlItem8";
		this.layoutControlItem8.Size = new System.Drawing.Size(1841, 724);
		this.layoutControlItem8.Text = "Danh sách Profile";
		this.layoutControlItem8.TextLocation = DevExpress.Utils.Locations.Top;
		this.layoutControlItem8.TextSize = new System.Drawing.Size(152, 19);
		this.splitterItem1.AllowHotTrack = true;
		this.splitterItem1.Location = new System.Drawing.Point(0, 483);
		this.splitterItem1.Name = "splitterItem1";
		this.splitterItem1.Size = new System.Drawing.Size(1841, 8);
		this.ProfileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProfileName.DataPropertyName = "ProfileName";
		this.ProfileName.FillWeight = 20f;
		this.ProfileName.HeaderText = "Tên Profile";
		this.ProfileName.MinimumWidth = 8;
		this.ProfileName.Name = "ProfileName";
		this.ProfileName.ReadOnly = true;
		this.UA.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.UA.DataPropertyName = "UA";
		this.UA.FillWeight = 25f;
		this.UA.HeaderText = "User Agent";
		this.UA.MinimumWidth = 8;
		this.UA.Name = "UA";
		this.UA.ReadOnly = true;
		this.Proxy.DataPropertyName = "Proxy";
		this.Proxy.FillWeight = 20f;
		this.Proxy.HeaderText = "Proxy";
		this.Proxy.MinimumWidth = 8;
		this.Proxy.Name = "Proxy";
		this.Proxy.ReadOnly = true;
		this.ProfilePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProfilePath.DataPropertyName = "ProfilePath";
		this.ProfilePath.FillWeight = 38f;
		this.ProfilePath.HeaderText = "Đường dẫn";
		this.ProfilePath.MinimumWidth = 8;
		this.ProfilePath.Name = "ProfilePath";
		this.ProfilePath.ReadOnly = true;
		this.grdNCCProxy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.grdNCCProxy.DataPropertyName = "NCCProxy";
		this.grdNCCProxy.FillWeight = 20f;
		this.grdNCCProxy.HeaderText = "NCC Proxy";
		this.grdNCCProxy.MinimumWidth = 8;
		this.grdNCCProxy.Name = "grdNCCProxy";
		this.grdNCCProxy.ReadOnly = true;
		this.KeyProxy.DataPropertyName = "Key";
		this.KeyProxy.FillWeight = 50f;
		this.KeyProxy.HeaderText = "Key Proxy";
		this.KeyProxy.MinimumWidth = 8;
		this.KeyProxy.Name = "KeyProxy";
		this.KeyProxy.ReadOnly = true;
		this.LoaiProxy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.LoaiProxy.DataPropertyName = "LoaiProxy";
		this.LoaiProxy.FillWeight = 25f;
		this.LoaiProxy.HeaderText = "Loại proxy";
		this.LoaiProxy.MinimumWidth = 8;
		this.LoaiProxy.Name = "LoaiProxy";
		this.LoaiProxy.ReadOnly = true;
		this.TinhTP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TinhTP.DataPropertyName = "TinhTP";
		this.TinhTP.FillWeight = 20f;
		this.TinhTP.HeaderText = "Tỉnh/TP";
		this.TinhTP.MinimumWidth = 8;
		this.TinhTP.Name = "TinhTP";
		this.TinhTP.ReadOnly = true;
		this.Status.DataPropertyName = "Status";
		this.Status.FillWeight = 18f;
		this.Status.HeaderText = "Trạng thái";
		this.Status.MinimumWidth = 8;
		this.Status.Name = "Status";
		this.Status.ReadOnly = true;
		this.GmailInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.GmailInfo.FillWeight = 30f;
		this.GmailInfo.HeaderText = "Gmail Info";
		this.GmailInfo.MinimumWidth = 8;
		this.GmailInfo.Name = "GmailInfo";
		this.GmailInfo.ReadOnly = true;
		this.GmailLoginStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.GmailLoginStatus.FillWeight = 20f;
		this.GmailLoginStatus.HeaderText = "Tình trạng Gmail";
		this.GmailLoginStatus.MinimumWidth = 8;
		this.GmailLoginStatus.Name = "GmailLoginStatus";
		this.GmailLoginStatus.ReadOnly = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1841, 1215);
		base.Controls.Add(this.locMain);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmCreateProfile";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Tạo profile";
		base.Load += new System.EventHandler(frmCreateProfile_Load);
		((System.ComponentModel.ISupportInitialize)this.grdProfile).EndInit();
		this.mnuProfile.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.locMain).EndInit();
		this.locMain.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.lueProxyType.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).EndInit();
		((System.ComponentModel.ISupportInitialize)this.memUserAgent.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.speSoLuong.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ccbTinhTP.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtIP.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cbeProxySupplier.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtProfileName.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lciProfileName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlGroup2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lciProxySupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lciIP).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lciTinhTP).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lciProxyType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlGroup3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.emptySpaceItem3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).EndInit();
		((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.emptySpaceItem5).EndInit();
		((System.ComponentModel.ISupportInitialize)this.emptySpaceItem6).EndInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).EndInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).EndInit();
		((System.ComponentModel.ISupportInitialize)this.emptySpaceItem4).EndInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).EndInit();
		((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).EndInit();
		((System.ComponentModel.ISupportInitialize)this.splitterItem1).EndInit();
		base.ResumeLayout(false);
	}
}
