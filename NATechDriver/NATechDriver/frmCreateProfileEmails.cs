using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NATechApi.Models;
using NATechDriver.Properties;
using NATechProxy;
using NATechProxy.Class;

namespace NATechDriver;

public class frmCreateProfileEmails : Form
{
	private Random rand = new Random();

	private int Delay = 3;

	private int DelayPass = 3;

	private DataTable dtProfile;

	private string DataSourceFileName = "DataSource.xml";

	private List<string> lstAccount;

	private readonly string login_gmail = "https://accounts.google.com/ServiceLogin/identifier?hl=vi&flowName=GlifWebSignIn&flowEntry=AddSession";

	private IContainer components = null;

	private GroupBox groupBox1;

	private RichTextBox memGmail;

	private Button btnCreate;

	private BackgroundWorker wkMain;

	private Button btnStop;

	private Button btnClose;

	private ProgressBar prbMain;

	private Label label1;

	private Label label3;

	private Label label2;

	private Label label4;

	private Label label6;

	private Label label5;

	private NumericUpDown speDelay;

	private Label label7;

	private NumericUpDown speDelayPass;

	private Label label8;

	public List<string> lstUserAgent { get; set; }

	public string FolderPath { get; set; }

	public string BrowserPath { get; set; }

	public string BrowserLanguage { get; set; }

	public bool IsMain { get; set; }

	public string ProfileDefaultPath { get; set; }

	public frmCreateProfileEmails()
	{
		InitializeComponent();
	}

	private void frmCreateProfileEmails_Load(object sender, EventArgs e)
	{
		try
		{
			LoadDataBase();
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCreate_Click(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrEmpty(memGmail.Text))
			{
				MessageBox.Show(this, "Chưa nhập thông tin Email & proxy", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			Delay = Misc.ObjInt(speDelay.Value);
			DelayPass = Misc.ObjInt(speDelayPass.Value);
			lstAccount = Misc.SplitToList(memGmail.Text, "\r\n");
			if (memGmail.Text.Contains("\r\n"))
			{
				lstAccount = Misc.SplitToList(memGmail.Text, "\r\n");
			}
			else
			{
				lstAccount = Misc.SplitToList(memGmail.Text, "\n");
			}
			prbMain.Minimum = 0;
			prbMain.Maximum = lstAccount.Count;
			prbMain.Step = 1;
			prbMain.ResetText();
			btnCreate.Enabled = false;
			btnStop.Enabled = true;
			wkMain.RunWorkerAsync();
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void wkMain_DoWork(object sender, DoWorkEventArgs e)
	{
		try
		{
			int num = 0;
			while (num < lstAccount.Count && !wkMain.CancellationPending)
			{
				int GmailLoginStatus = 0;
				List<string> list = Misc.SplitToList(lstAccount[num], "|");
				if (list.Count >= 3)
				{
					EmailItem emailItem = new EmailItem(list[0], list[1], list[2]);
					ProxyNATech proxyNATech = null;
					if (list.Count == 2 || list.Count == 4)
					{
						emailItem = null;
						proxyNATech = new ProxyNATech();
						proxyNATech.Ip = list[0];
						proxyNATech.Port = list[1];
						if (list.Count == 4)
						{
							proxyNATech.UserName = list[2];
							proxyNATech.Password = list[3];
						}
					}
					if (list.Count >= 5)
					{
						proxyNATech = new ProxyNATech();
						proxyNATech.Ip = list[3];
						proxyNATech.Port = list[4];
						if (list.Count >= 7)
						{
							proxyNATech.UserName = list[5];
							proxyNATech.Password = list[6];
						}
					}
					string uA = lstUserAgent[rand.Next(lstUserAgent.Count)];
					string sErr = string.Empty;
					string ProfileName = string.Empty;
					if (emailItem != null && !string.IsNullOrEmpty(emailItem.Email))
					{
						ProfileName = emailItem.Email;
					}
					NATechDriver.CreateProfile(ProfileDefaultPath, FolderPath, ref ProfileName, uA, proxyNATech, BrowserLanguage, ref sErr);
					string text = FolderPath + "\\" + ProfileName;
					ChromeDriver driver = null;
					InitDriverChrome(ref driver, text, proxyNATech);
					if (proxyNATech != null && !string.IsNullOrEmpty(proxyNATech.UserName) && !string.IsNullOrEmpty(proxyNATech.Password))
					{
						NetworkAuthenticationHandler handler = new NetworkAuthenticationHandler
						{
							UriMatcher = (Uri d) => true,
							Credentials = new PasswordCredentials(proxyNATech.UserName, proxyNATech.Password)
						};
						INetwork network = driver.Manage().Network;
						network.AddAuthenticationHandler(handler);
						network.StartMonitoring();
					}
					if (emailItem != null)
					{
						LoginGmail(driver, emailItem, ref sErr, out GmailLoginStatus);
					}
					DongTrinhDuyet(ref driver);
					if (!string.IsNullOrEmpty(sErr))
					{
						DeleteProfile(ProfileName);
						num--;
					}
					SaveData(ProfileName, text, uA, proxyNATech, lstAccount[num], GmailLoginStatus);
				}
				num++;
				wkMain.ReportProgress(num);
				if (Delay > 0 && num < lstAccount.Count)
				{
					Thread.Sleep(Delay * 1000);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DeleteProfile(string ProfileName)
	{
		if (string.IsNullOrEmpty(BrowserPath) || dtProfile == null)
		{
			return;
		}
		DataRow[] array = dtProfile.Select("ProfileName='" + ProfileName + "'");
		if (array == null || array.Length == 0)
		{
			return;
		}
		try
		{
			if (Directory.Exists(FolderPath + "\\" + ProfileName))
			{
				Directory.Delete(FolderPath + "\\" + ProfileName, recursive: true);
			}
			array[0].Delete();
			dtProfile.AcceptChanges();
			string fileName = FolderPath + "\\" + DataSourceFileName;
			dtProfile.WriteXml(fileName, XmlWriteMode.WriteSchema);
		}
		catch (Exception ex)
		{
			string message = ex.Message;
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
		}
	}

	private void DongTrinhDuyet(ref ChromeDriver driver)
	{
		try
		{
			if (driver != null)
			{
				driver.Close();
				driver.Quit();
				driver.Dispose();
				driver = null;
			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}

	private void LoginGmail(ChromeDriver driver, EmailItem email, ref string sErr, out int GmailLoginStatus)
	{
		try
		{
			GmailLoginStatus = 0;
			if (email == null)
			{
				return;
			}
			driver.Navigate().GoToUrl(login_gmail);
			IWebElement webElement = driver.FindElement(By.Id("identifierId"));
			webElement.SendKeys(email.Email);
			IWebElement webElement2 = driver.FindElement(By.Id("identifierNext"));
			webElement2.Click();
			Thread.Sleep(3000);
			if (driver.Url.Contains("/rejected?"))
			{
				sErr = "User Agent cũ quá nên không đăng nhập Gmail được.";
				return;
			}
			if (DelayPass > 0)
			{
				Thread.Sleep(DelayPass * 1000);
			}
			IWebElement webElement3 = driver.FindElement(By.XPath("//*[@id=\"password\"]/div[1]/div/div[1]/input"));
			webElement3.SendKeys(email.Password);
			IWebElement webElement4 = driver.FindElement(By.Id("passwordNext"));
			webElement4.Click();
			Thread.Sleep(3000);
			try
			{
				try
				{
					List<IWebElement> list = driver.FindElements(By.TagName("li")).ToList();
					foreach (IWebElement item in list)
					{
						string text = Misc.ToString(item.GetAttribute("outerHTML")).ToLower();
						if (text.Contains("xác nhận email khôi phục của bạn") || text.Contains("confirm your recovery email"))
						{
							item.Click();
						}
					}
				}
				catch (Exception ex)
				{
					string message = ex.Message;
				}
				try
				{
					IWebElement webElement5 = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div[2]/div/div/div[2]/div/div[1]/div/form/span/section/div/div/div[2]/div[1]/div/div/div/div/div[1]/div/div[1]/input"));
					webElement5.SendKeys(email.Password);
					IWebElement webElement6 = driver.FindElement(By.Id("passwordNext"));
					try
					{
						webElement6.Click();
					}
					catch (Exception)
					{
					}
				}
				catch (Exception ex3)
				{
					string message2 = ex3.Message;
				}
				try
				{
					driver.FindElement(By.Name("knowledgePreregisteredEmailResponse"))?.SendKeys(email.RecoveryEmail);
					driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div[2]/div/div/div[2]/div/div[2]/div/div[1]/div/div/button"))?.Click();
					Thread.Sleep(1000);
				}
				catch (Exception)
				{
				}
			}
			catch (Exception)
			{
			}
			try
			{
				List<IWebElement> list2 = driver.FindElements(By.XPath("//a[contains(@href,'https://accounts.google.com/SignOutOptions')]")).ToList();
				GmailLoginStatus = ((list2 != null && list2.Count > 0) ? 1 : 0);
			}
			catch (Exception)
			{
			}
		}
		catch (Exception)
		{
			GmailLoginStatus = 0;
		}
	}

	private void InitDriverChrome(ref ChromeDriver driver, string profileFolder, ProxyNATech proxy)
	{
		ChromeOptions chromeOptions = new ChromeOptions();
		chromeOptions.AddArgument("--lang=en");
		chromeOptions.AddArgument("--mute-audio");
		chromeOptions.AddArgument("--disable-dev-shm-usage");
		chromeOptions.AddArgument("--window-size=500,700");
		chromeOptions.AddArguments("--disable-popup-blocking", "--disable-extensions");
		if (proxy != null && !string.IsNullOrEmpty(proxy.Ip) && !string.IsNullOrEmpty(proxy.Port))
		{
			chromeOptions.AddArguments("--proxy-server=" + proxy.Ip + ":" + proxy.Port);
		}
		string browserExecutablePath = Directory.GetCurrentDirectory() + "\\browser\\orbita-browser\\chrome.exe";
		string userDataDir = string.Empty;
		if (Directory.Exists(profileFolder))
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(profileFolder);
			userDataDir = profileFolder;
		}
		string driverExecutablePath = Directory.GetCurrentDirectory() + "\\chromedriver.exe";
		driver = NATechDriver.Create(chromeOptions, userDataDir, driverExecutablePath, browserExecutablePath);
		driver.Manage().Window.Maximize();
		driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60.0);
		driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.0);
	}

	private void SaveData(string profileName, string profilePath, string uA, ProxyNATech proxy, string GmailInfo, int? GmailLoginStatus)
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
		dataRow["GmailInfo"] = GmailInfo;
		if (GmailLoginStatus.HasValue)
		{
			dataRow["GmailLoginStatus"] = GmailLoginStatus.Value;
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

	private void wkMain_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		try
		{
			prbMain.Value = e.ProgressPercentage;
			prbMain.Refresh();
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}

	private void wkMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		try
		{
			MessageBox.Show(this, "Hoàn thành tạo profile.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			btnCreate.Enabled = true;
			btnClose.Enabled = true;
			btnStop.Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnStop_Click(object sender, EventArgs e)
	{
		try
		{
			wkMain.CancelAsync();
			btnStop.Enabled = false;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateProfileEmails));
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.memGmail = new System.Windows.Forms.RichTextBox();
		this.btnCreate = new System.Windows.Forms.Button();
		this.wkMain = new System.ComponentModel.BackgroundWorker();
		this.btnStop = new System.Windows.Forms.Button();
		this.btnClose = new System.Windows.Forms.Button();
		this.prbMain = new System.Windows.Forms.ProgressBar();
		this.speDelay = new System.Windows.Forms.NumericUpDown();
		this.label7 = new System.Windows.Forms.Label();
		this.speDelayPass = new System.Windows.Forms.NumericUpDown();
		this.label8 = new System.Windows.Forms.Label();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.speDelay).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.speDelayPass).BeginInit();
		base.SuspendLayout();
		this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.memGmail);
		this.groupBox1.Location = new System.Drawing.Point(12, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1231, 548);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Danh sách Gmail và Proxy";
		this.label4.AutoSize = true;
		this.label4.ForeColor = System.Drawing.Color.Blue;
		this.label4.Location = new System.Drawing.Point(41, 510);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(640, 20);
		this.label4.TabIndex = 1;
		this.label4.Text = "- Mẫu 3 (Có Email + Có Sock 5 proxy): Email|Password|Email khôi phục|IP|Port|User|Pass";
		this.label3.AutoSize = true;
		this.label3.ForeColor = System.Drawing.Color.Blue;
		this.label3.Location = new System.Drawing.Point(41, 485);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(543, 20);
		this.label3.TabIndex = 1;
		this.label3.Text = "- Mẫu 2 (Có Email + Có Http proxy): Email|Password|Email khôi phục|IP|Port";
		this.label6.AutoSize = true;
		this.label6.ForeColor = System.Drawing.Color.Blue;
		this.label6.Location = new System.Drawing.Point(711, 485);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(350, 20);
		this.label6.TabIndex = 1;
		this.label6.Text = "- Mẫu 5 (Chỉ có Sock 5 proxy): IP|Port|User|Pass";
		this.label5.AutoSize = true;
		this.label5.ForeColor = System.Drawing.Color.Blue;
		this.label5.Location = new System.Drawing.Point(711, 461);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(253, 20);
		this.label5.TabIndex = 1;
		this.label5.Text = "- Mẫu 4 (Chỉ có Http proxy): IP|Port";
		this.label2.AutoSize = true;
		this.label2.ForeColor = System.Drawing.Color.Blue;
		this.label2.Location = new System.Drawing.Point(41, 461);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(501, 20);
		this.label2.TabIndex = 1;
		this.label2.Text = "- Mẫu 1 (Có Email + Không có proxy): Email|Password|Email khôi phục";
		this.label1.AutoSize = true;
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(16, 433);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(460, 20);
		this.label1.TabIndex = 1;
		this.label1.Text = "Mỗi dòng 1 bộ Gmail và Proxy. Dữ liệu có thể có 5 dạng như sau:";
		this.memGmail.Location = new System.Drawing.Point(6, 25);
		this.memGmail.Name = "memGmail";
		this.memGmail.Size = new System.Drawing.Size(1212, 405);
		this.memGmail.TabIndex = 0;
		this.memGmail.Text = "";
		this.btnCreate.Image = Properties.Resources.play;
		this.btnCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnCreate.Location = new System.Drawing.Point(689, 552);
		this.btnCreate.Name = "btnCreate";
		this.btnCreate.Size = new System.Drawing.Size(151, 63);
		this.btnCreate.TabIndex = 1;
		this.btnCreate.Text = "Thực hiện";
		this.btnCreate.UseVisualStyleBackColor = true;
		this.btnCreate.Click += new System.EventHandler(btnCreate_Click);
		this.wkMain.WorkerReportsProgress = true;
		this.wkMain.WorkerSupportsCancellation = true;
		this.wkMain.DoWork += new System.ComponentModel.DoWorkEventHandler(wkMain_DoWork);
		this.wkMain.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(wkMain_ProgressChanged);
		this.wkMain.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(wkMain_RunWorkerCompleted);
		this.btnStop.Image = Properties.Resources.stop1;
		this.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnStop.Location = new System.Drawing.Point(846, 552);
		this.btnStop.Name = "btnStop";
		this.btnStop.Size = new System.Drawing.Size(131, 63);
		this.btnStop.TabIndex = 2;
		this.btnStop.Text = "Dừng";
		this.btnStop.UseVisualStyleBackColor = true;
		this.btnStop.Click += new System.EventHandler(btnStop_Click);
		this.btnClose.Image = Properties.Resources.exit1;
		this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnClose.Location = new System.Drawing.Point(983, 552);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(118, 63);
		this.btnClose.TabIndex = 3;
		this.btnClose.Text = "Đóng";
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		this.prbMain.Location = new System.Drawing.Point(18, 621);
		this.prbMain.Name = "prbMain";
		this.prbMain.Size = new System.Drawing.Size(1212, 17);
		this.prbMain.TabIndex = 4;
		this.speDelay.Location = new System.Drawing.Point(254, 569);
		this.speDelay.Name = "speDelay";
		this.speDelay.Size = new System.Drawing.Size(120, 26);
		this.speDelay.TabIndex = 5;
		this.speDelay.Value = new decimal(new int[4] { 3, 0, 0, 0 });
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(102, 571);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(137, 20);
		this.label7.TabIndex = 6;
		this.label7.Text = "Chờ giữa 2 lượt (s)";
		this.speDelayPass.Location = new System.Drawing.Point(544, 569);
		this.speDelayPass.Name = "speDelayPass";
		this.speDelayPass.Size = new System.Drawing.Size(120, 26);
		this.speDelayPass.TabIndex = 5;
		this.speDelayPass.Value = new decimal(new int[4] { 3, 0, 0, 0 });
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(390, 571);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(138, 20);
		this.label8.TabIndex = 6;
		this.label8.Text = "Chờ nhập pass (s)";
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1255, 654);
		base.Controls.Add(this.label8);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.speDelayPass);
		base.Controls.Add(this.speDelay);
		base.Controls.Add(this.prbMain);
		base.Controls.Add(this.btnClose);
		base.Controls.Add(this.btnStop);
		base.Controls.Add(this.btnCreate);
		base.Controls.Add(this.groupBox1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmCreateProfileEmails";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Tạo profile theo danh sách Gmail và proxy";
		base.Load += new System.EventHandler(frmCreateProfileEmails_Load);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.speDelay).EndInit();
		((System.ComponentModel.ISupportInitialize)this.speDelayPass).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
