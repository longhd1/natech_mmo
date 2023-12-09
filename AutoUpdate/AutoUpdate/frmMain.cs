using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace AutoUpdate;

public class frmMain : Form
{
	private int wait = 5;

	private IContainer components = null;

	private System.Windows.Forms.Timer tmMain;

	private Label lbeStatus;

	public string PathEXE { get; set; }

	public string SoftwareZipFilePath { get; set; }

	public string SoftwareUnzipFolder { get; set; }

	public string ZipPassword { get; set; }

	public frmMain()
	{
		InitializeComponent();
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

	public void ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
	{
		ZipFile zipFile = null;
		try
		{
			FileStream file = File.OpenRead(archiveFilenameIn);
			zipFile = new ZipFile(file);
			if (!string.IsNullOrEmpty(password))
			{
				zipFile.Password = password;
			}
			foreach (ZipEntry item in zipFile)
			{
				if (item.IsFile)
				{
					string name = item.Name;
					byte[] buffer = new byte[4096];
					Stream inputStream = zipFile.GetInputStream(item);
					string path = Path.Combine(outFolder, name);
					string directoryName = Path.GetDirectoryName(path);
					if (directoryName.Length > 0)
					{
						Directory.CreateDirectory(directoryName);
					}
					using FileStream destination = File.Create(path);
					StreamUtils.Copy(inputStream, destination, buffer);
				}
			}
		}
		finally
		{
			if (zipFile != null)
			{
				zipFile.IsStreamOwner = true;
				zipFile.Close();
			}
		}
	}

	private void RunTimer(string Type, int Interval)
	{
		tmMain.Tag = Type;
		tmMain.Interval = Interval;
		tmMain.Stop();
		tmMain.Start();
	}

	private string ToString(object o)
	{
		if (o != null)
		{
			return o.ToString();
		}
		return string.Empty;
	}

	private void frmMain_Load(object sender, EventArgs e)
	{
		try
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (commandLineArgs.Length < 2)
			{
				return;
			}
			string text = ToString(commandLineArgs[1]);
			string text2 = text;
			string text3 = text2;
			if (!(text3 == "unzip"))
			{
				if (text3 == "copy")
				{
				}
			}
			else if (commandLineArgs.Length >= 5)
			{
				Thread.Sleep(3000);
				SoftwareZipFilePath = Normalize(new Sec().Decrypt(ToString(commandLineArgs[2])));
				SoftwareUnzipFolder = Normalize(new Sec().Decrypt(ToString(commandLineArgs[3])));
				PathEXE = Normalize(new Sec().Decrypt(ToString(commandLineArgs[4])));
				if (commandLineArgs.Length >= 6)
				{
					ZipPassword = ToString(commandLineArgs[5]);
				}
				if (!File.Exists(SoftwareZipFilePath))
				{
					MessageBox.Show(this, "Không tìm thấy file " + SoftwareZipFilePath, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					Application.Exit();
				}
				if (string.IsNullOrEmpty(PathEXE))
				{
					MessageBox.Show(this, "Chưa cấu hình Tập tin ứng dụng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					Application.Exit();
				}
				if (!File.Exists(PathEXE))
				{
					MessageBox.Show(this, "Không tìm thấy file ứng dụng [" + PathEXE + "] hoặc đường dẫn chứa ứng dụng có chứa khoảng trắng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					Application.Exit();
				}
				if (!Directory.Exists(SoftwareUnzipFolder))
				{
					Directory.CreateDirectory(SoftwareUnzipFolder);
				}
				lbeStatus.Text = "Đang giải nén bộ cài...";
				RunTimer("Unzip", 5000);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			MessageBox.Show(this, "Nếu gặp lỗi cập nhật thì vui lòng vào Trang chủ tải lại bản đầy đủ.", "Cập nhật thủ công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	private string Normalize(string EncryptData)
	{
		if (string.IsNullOrEmpty(EncryptData))
		{
			return EncryptData;
		}
		return EncryptData.Replace("NATechstandard", " ");
	}

	private void tmMain_Tick(object sender, EventArgs e)
	{
		try
		{
			if (tmMain.Tag == null)
			{
				lbeStatus.Text = "Chức năng cập nhật tự động của NATECH";
				tmMain.Stop();
				return;
			}
			string text = tmMain.Tag.ToString();
			string text2 = text;
			if (!(text2 == "Restart"))
			{
				if (text2 == "Unzip")
				{
					tmMain.Stop();
					ExtractZipFile(SoftwareZipFilePath, ZipPassword, SoftwareUnzipFolder);
					RunTimer("Restart", 1000);
				}
				return;
			}
			wait--;
			if (wait <= 0)
			{
				Process.Start(PathEXE);
				tmMain.Stop();
				Application.Exit();
			}
			else
			{
				lbeStatus.Text = $"Cập nhật thành công! Tự khởi động chương trình sau {wait} giây";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			MessageBox.Show(this, "Nếu gặp lỗi cập nhật thì vui lòng vào Trang chủ tải lại bản đầy đủ.", "Cập nhật thủ công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoUpdate.frmMain));
		this.tmMain = new System.Windows.Forms.Timer();
		this.lbeStatus = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.tmMain.Enabled = true;
		this.tmMain.Interval = 1000;
		this.tmMain.Tick += new System.EventHandler(tmMain_Tick);
		this.lbeStatus.AutoSize = true;
		this.lbeStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 163);
		this.lbeStatus.ForeColor = System.Drawing.Color.Blue;
		this.lbeStatus.Location = new System.Drawing.Point(124, 93);
		this.lbeStatus.Name = "lbeStatus";
		this.lbeStatus.Size = new System.Drawing.Size(100, 25);
		this.lbeStatus.TabIndex = 0;
		this.lbeStatus.Text = "Tình trạng";
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(703, 246);
		base.Controls.Add(this.lbeStatus);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmMain";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Cập nhật tự động";
		base.Load += new System.EventHandler(frmMain_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
