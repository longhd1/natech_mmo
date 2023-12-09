using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NATechProxy;

namespace NATechApi;

public class frmUpdate : Form
{
	private string tempFile = string.Empty;

	private IContainer components = null;

	private Label lbeCurrentVersion;

	private Label lbeServerVersion;

	private Button btnUpdate;

	private Button btnClose;

	private Label lbeStatus;

	private Label label1;

	private HyperlinkLabelControl hplDownload;

	private LabelControl labelControl1;

	public string CurrentVersion { get; set; }

	public frmUpdate()
	{
		InitializeComponent();
	}

	private void frmUpdate_Load(object sender, EventArgs e)
	{
		try
		{
			lbeCurrentVersion.Text = "Phiên bản hiện tại: " + CurrentVersion;
			if (ApiClientNATech.loginResponse != null)
			{
				lbeServerVersion.Text = "Phiên bản máy chủ: " + ApiClientNATech.loginResponse.SoftVersion;
			}
			btnUpdate.Enabled = CurrentVersion != ApiClientNATech.loginResponse.SoftVersion;
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

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			btnUpdate.Enabled = false;
			tempFile = $"{Path.GetTempPath().TrimEnd('\\')}\\{ApiClientNATech.SoftwareZipFileName}";
			if (string.IsNullOrEmpty(ApiClientNATech.LinkDownloadSoft))
			{
				MessageBox.Show(this, "Chưa cấu hình link download phần mềm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			WebClient webClient = new WebClient();
			webClient.DownloadProgressChanged += wc_DownloadProgressChanged;
			webClient.DownloadFileCompleted += wc_DownloadFileCompleted;
			webClient.DownloadFileAsync(new Uri(ApiClientNATech.LinkDownloadSoft), tempFile);
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			btnUpdate.Enabled = true;
		}
	}

	private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
	{
		try
		{
			lbeStatus.Text = $"Đã tải {e.BytesReceived / 1024}K/{e.TotalBytesToReceive / 1024}K. Hoàn thành: {e.ProgressPercentage}%";
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
	{
		btnUpdate.Enabled = true;
		lbeStatus.Text = "Tải hoàn tất. Tiến hành giải nén.";
		if (File.Exists(tempFile))
		{
			string text = Standardize(new Sec().Encrypt(tempFile));
			string text2 = Standardize(new Sec().Encrypt(Directory.GetCurrentDirectory()));
			string text3 = Standardize(new Sec().Encrypt(Assembly.GetEntryAssembly().Location));
			string arguments = "unzip " + text + " " + text2 + " " + text3;
			Process.Start(Directory.GetCurrentDirectory() + "\\AutoUpdate.exe", arguments);
			Application.Exit();
		}
	}

	private string Standardize(string EncryptData)
	{
		if (string.IsNullOrEmpty(EncryptData))
		{
			return EncryptData;
		}
		return EncryptData.Replace(" ", "NATechstandard");
	}

	private string Normalize(string EncryptData)
	{
		if (string.IsNullOrEmpty(EncryptData))
		{
			return EncryptData;
		}
		return EncryptData.Replace("NATechstandard", " ");
	}

	private void hplDownload_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start(ApiClientNATech.LinkDownloadSoft);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NATechApi.frmUpdate));
		this.lbeCurrentVersion = new System.Windows.Forms.Label();
		this.lbeServerVersion = new System.Windows.Forms.Label();
		this.btnUpdate = new System.Windows.Forms.Button();
		this.btnClose = new System.Windows.Forms.Button();
		this.lbeStatus = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.hplDownload = new DevExpress.XtraEditors.HyperlinkLabelControl();
		this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
		base.SuspendLayout();
		this.lbeCurrentVersion.AutoSize = true;
		this.lbeCurrentVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 163);
		this.lbeCurrentVersion.ForeColor = System.Drawing.Color.DodgerBlue;
		this.lbeCurrentVersion.Location = new System.Drawing.Point(269, 29);
		this.lbeCurrentVersion.Name = "lbeCurrentVersion";
		this.lbeCurrentVersion.Size = new System.Drawing.Size(204, 29);
		this.lbeCurrentVersion.TabIndex = 0;
		this.lbeCurrentVersion.Text = "Phiên bản hiện tại";
		this.lbeServerVersion.AutoSize = true;
		this.lbeServerVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 163);
		this.lbeServerVersion.ForeColor = System.Drawing.Color.DodgerBlue;
		this.lbeServerVersion.Location = new System.Drawing.Point(269, 69);
		this.lbeServerVersion.Name = "lbeServerVersion";
		this.lbeServerVersion.Size = new System.Drawing.Size(167, 29);
		this.lbeServerVersion.TabIndex = 0;
		this.lbeServerVersion.Text = "Phiên bản mới";
		this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 163);
		this.btnUpdate.ForeColor = System.Drawing.Color.MediumBlue;
		this.btnUpdate.Location = new System.Drawing.Point(255, 121);
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(128, 42);
		this.btnUpdate.TabIndex = 1;
		this.btnUpdate.Text = "Cập nhật";
		this.btnUpdate.UseVisualStyleBackColor = true;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 163);
		this.btnClose.ForeColor = System.Drawing.Color.MediumBlue;
		this.btnClose.Location = new System.Drawing.Point(389, 121);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(125, 42);
		this.btnClose.TabIndex = 1;
		this.btnClose.Text = "Đóng";
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Visible = false;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		this.lbeStatus.AutoSize = true;
		this.lbeStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 163);
		this.lbeStatus.ForeColor = System.Drawing.Color.Blue;
		this.lbeStatus.Location = new System.Drawing.Point(190, 267);
		this.lbeStatus.Name = "lbeStatus";
		this.lbeStatus.Size = new System.Drawing.Size(0, 25);
		this.lbeStatus.TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 163);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(74, 210);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(677, 25);
		this.label1.TabIndex = 3;
		this.label1.Text = "Nếu quá trình cập nhật bị lỗi thì vui lòng khởi động lại Máy tính rồi Cập nhật lại";
		this.hplDownload.Cursor = System.Windows.Forms.Cursors.Hand;
		this.hplDownload.Location = new System.Drawing.Point(221, 339);
		this.hplDownload.Name = "hplDownload";
		this.hplDownload.Size = new System.Drawing.Size(324, 19);
		this.hplDownload.TabIndex = 4;
		this.hplDownload.Text = "Hoặc cập nhật thủ công tại đây => Download";
		this.hplDownload.Click += new System.EventHandler(hplDownload_Click);
		this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Italic);
		this.labelControl1.Appearance.Options.UseFont = true;
		this.labelControl1.Location = new System.Drawing.Point(169, 379);
		this.labelControl1.Name = "labelControl1";
		this.labelControl1.Size = new System.Drawing.Size(442, 19);
		this.labelControl1.TabIndex = 5;
		this.labelControl1.Text = "Tải về sau đó giải nén và chép đè vào thư mục của Phần mềm";
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(840, 410);
		base.Controls.Add(this.labelControl1);
		base.Controls.Add(this.hplDownload);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.lbeStatus);
		base.Controls.Add(this.btnClose);
		base.Controls.Add(this.btnUpdate);
		base.Controls.Add(this.lbeServerVersion);
		base.Controls.Add(this.lbeCurrentVersion);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmUpdate";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Cập nhật phiên bản mới";
		base.Load += new System.EventHandler(frmUpdate_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
