using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;
using NATechApi.Models;

namespace NATechApi;

public class frmLogin : Form
{
	private IContainer components = null;

	private Button btnLogin;

	private Label label2;

	private Label label1;

	private TextBox txtPassword;

	private TextBox txtUsername;

	private LinkLabel lbeLossPassword;

	private LinkLabel lbeRegister;

	private Label lbeNotice;

	private CheckBox ceiRememberMe;

	private LinkLabel lbeGUINewVersion;

	private LinkLabel lbeActiveAccount;

	public string Token { get; set; }

	public string LinkGUINewVersion { get; set; }

	public bool IsAutoLogin { get; set; }

	public frmLogin()
	{
		IsAutoLogin = true;
		InitializeComponent();
	}

	public frmLogin(bool isAutoLogin)
	{
		IsAutoLogin = isAutoLogin;
		InitializeComponent();
	}

	private void frmLogin_Load(object sender, EventArgs e)
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(ApiClientNATech.SoftId.ToString());
			lbeGUINewVersion.Visible = !string.IsNullOrEmpty(LinkGUINewVersion);
			if (registryKey == null)
			{
				return;
			}
			string text = ToString(registryKey.GetValue("rememberMe"));
			if (!string.IsNullOrEmpty(text))
			{
				ceiRememberMe.Checked = text == "1";
			}
			if (ceiRememberMe.Checked)
			{
				text = ToString(registryKey.GetValue("username"));
				if (!string.IsNullOrEmpty(text))
				{
					txtUsername.Text = text;
				}
				text = ToString(registryKey.GetValue("password"));
				if (!string.IsNullOrEmpty(text))
				{
					txtPassword.Text = text;
				}
				if (IsAutoLogin)
				{
					btnLogin_Click(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, "frmLogin_Load" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private string ToString(object o)
	{
		if (o == null)
		{
			return string.Empty;
		}
		return o.ToString();
	}

	private void btnLogin_Click(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrEmpty(ApiClientNATech.ApiUrl))
			{
				MessageBox.Show(this, "Chưa cấu hình ApiUrl", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (ApiClientNATech.SoftId == 0)
			{
				MessageBox.Show(this, "Chưa cấu hình SoftId", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (string.IsNullOrEmpty(txtUsername.Text.Trim()))
			{
				MessageBox.Show(this, "Username không được rỗng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
			{
				MessageBox.Show(this, "Password không được rỗng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			LoginRequest strRequestData = new LoginRequest(txtUsername.Text.Trim(), txtPassword.Text.Trim(), rememberMe: true, ApiClientNATech.SoftId, ApiClientNATech.HardKey);
			string sErr = string.Empty;
			string value = new ApiClientNATech().PostApi("Users/Authenticate", strRequestData, string.Empty, out sErr);
			LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(value);
			if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
			{
				ApiClientNATech.loginResponse = loginResponse;
				RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(ApiClientNATech.SoftId.ToString());
				registryKey.SetValue("rememberMe", ceiRememberMe.Checked ? "1" : "0");
				if (ceiRememberMe.Checked)
				{
					registryKey.SetValue("username", txtUsername.Text.Trim());
					registryKey.SetValue("password", txtPassword.Text.Trim());
				}
				registryKey.Close();
				Close();
			}
			else if (loginResponse != null && loginResponse.sErr != null)
			{
				MessageBox.Show(this, loginResponse.Email + "-" + loginResponse.sErr.ErrorMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (!string.IsNullOrEmpty(sErr))
			{
				if (sErr.ToLower().Contains("unable to connect to the remote server"))
				{
					MessageBox.Show(this, "Không thể kết nối đến server của NATECH. Hãy đăng nhập lại sau hoặc liên hệ với NATECH (Telegram @longhoang02363)", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					MessageBox.Show(this, sErr, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, "btnLogin_Click " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtUsername_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyCode == Keys.Return && !string.IsNullOrEmpty(txtUsername.Text))
			{
				txtPassword.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtPassword_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyCode == Keys.Return && !string.IsNullOrEmpty(txtPassword.Text))
			{
				btnLogin_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void lbeLossPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			frmLossPassword frmLossPassword2 = new frmLossPassword();
			frmLossPassword2.ShowDialog(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void lbeRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			frmRegister frmRegister2 = new frmRegister();
			frmRegister2.ShowDialog(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void lbeGUINewVersion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			if (!string.IsNullOrEmpty(LinkGUINewVersion))
			{
				Process.Start(LinkGUINewVersion);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void lbeActiveAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			frmActiveAccount frmActiveAccount2 = new frmActiveAccount();
			frmActiveAccount2.ShowDialog(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NATechApi.frmLogin));
		this.btnLogin = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtPassword = new System.Windows.Forms.TextBox();
		this.txtUsername = new System.Windows.Forms.TextBox();
		this.lbeLossPassword = new System.Windows.Forms.LinkLabel();
		this.lbeRegister = new System.Windows.Forms.LinkLabel();
		this.lbeNotice = new System.Windows.Forms.Label();
		this.ceiRememberMe = new System.Windows.Forms.CheckBox();
		this.lbeGUINewVersion = new System.Windows.Forms.LinkLabel();
		this.lbeActiveAccount = new System.Windows.Forms.LinkLabel();
		base.SuspendLayout();
		this.btnLogin.Location = new System.Drawing.Point(422, 118);
		this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.btnLogin.Name = "btnLogin";
		this.btnLogin.Size = new System.Drawing.Size(112, 35);
		this.btnLogin.TabIndex = 2;
		this.btnLogin.Text = "Đăng nhập";
		this.btnLogin.UseVisualStyleBackColor = true;
		this.btnLogin.Click += new System.EventHandler(btnLogin_Click);
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(83, 86);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(75, 20);
		this.label2.TabIndex = 12;
		this.label2.Text = "Mật khẩu";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(83, 53);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(116, 20);
		this.label1.TabIndex = 11;
		this.label1.Text = "Tên đăng nhập";
		this.txtPassword.Location = new System.Drawing.Point(221, 83);
		this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.PasswordChar = '*';
		this.txtPassword.Size = new System.Drawing.Size(313, 26);
		this.txtPassword.TabIndex = 1;
		this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(txtPassword_KeyDown);
		this.txtUsername.Location = new System.Drawing.Point(221, 47);
		this.txtUsername.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.txtUsername.Name = "txtUsername";
		this.txtUsername.Size = new System.Drawing.Size(313, 26);
		this.txtUsername.TabIndex = 0;
		this.txtUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(txtUsername_KeyDown);
		this.lbeLossPassword.AutoSize = true;
		this.lbeLossPassword.Location = new System.Drawing.Point(179, 157);
		this.lbeLossPassword.Name = "lbeLossPassword";
		this.lbeLossPassword.Size = new System.Drawing.Size(118, 20);
		this.lbeLossPassword.TabIndex = 4;
		this.lbeLossPassword.TabStop = true;
		this.lbeLossPassword.Text = "Quên mật khẩu";
		this.lbeLossPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lbeLossPassword_LinkClicked);
		this.lbeRegister.AutoSize = true;
		this.lbeRegister.Location = new System.Drawing.Point(106, 157);
		this.lbeRegister.Name = "lbeRegister";
		this.lbeRegister.Size = new System.Drawing.Size(67, 20);
		this.lbeRegister.TabIndex = 3;
		this.lbeRegister.TabStop = true;
		this.lbeRegister.Text = "Đăng ký";
		this.lbeRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lbeRegister_LinkClicked);
		this.lbeNotice.AutoSize = true;
		this.lbeNotice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 163);
		this.lbeNotice.ForeColor = System.Drawing.Color.Red;
		this.lbeNotice.Location = new System.Drawing.Point(83, 253);
		this.lbeNotice.Name = "lbeNotice";
		this.lbeNotice.Size = new System.Drawing.Size(451, 20);
		this.lbeNotice.TabIndex = 13;
		this.lbeNotice.Text = "Lưu ý: Cùng 1 user đăng nhập ở máy này thì máy kia sẽ bị dừng";
		this.ceiRememberMe.AutoSize = true;
		this.ceiRememberMe.Location = new System.Drawing.Point(221, 118);
		this.ceiRememberMe.Name = "ceiRememberMe";
		this.ceiRememberMe.Size = new System.Drawing.Size(171, 24);
		this.ceiRememberMe.TabIndex = 14;
		this.ceiRememberMe.Text = "Ghi nhớ đăng nhập";
		this.ceiRememberMe.UseVisualStyleBackColor = true;
		this.lbeGUINewVersion.AutoSize = true;
		this.lbeGUINewVersion.Location = new System.Drawing.Point(145, 200);
		this.lbeGUINewVersion.Name = "lbeGUINewVersion";
		this.lbeGUINewVersion.Size = new System.Drawing.Size(252, 20);
		this.lbeGUINewVersion.TabIndex = 3;
		this.lbeGUINewVersion.TabStop = true;
		this.lbeGUINewVersion.Text = "Hướng dẫn sử dụng phiên bản mới";
		this.lbeGUINewVersion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lbeGUINewVersion_LinkClicked);
		this.lbeActiveAccount.AutoSize = true;
		this.lbeActiveAccount.Location = new System.Drawing.Point(303, 158);
		this.lbeActiveAccount.Name = "lbeActiveAccount";
		this.lbeActiveAccount.Size = new System.Drawing.Size(144, 20);
		this.lbeActiveAccount.TabIndex = 3;
		this.lbeActiveAccount.TabStop = true;
		this.lbeActiveAccount.Text = "Kích hoạt tài khoản";
		this.lbeActiveAccount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lbeActiveAccount_LinkClicked);
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(638, 282);
		base.Controls.Add(this.ceiRememberMe);
		base.Controls.Add(this.lbeNotice);
		base.Controls.Add(this.lbeGUINewVersion);
		base.Controls.Add(this.lbeActiveAccount);
		base.Controls.Add(this.lbeRegister);
		base.Controls.Add(this.lbeLossPassword);
		base.Controls.Add(this.btnLogin);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.txtPassword);
		base.Controls.Add(this.txtUsername);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmLogin";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Đăng nhập hệ thống";
		base.Load += new System.EventHandler(frmLogin_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
