using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using NATechApi.Models;

namespace NATechApi;

public class frmChangePassword : Form
{
	private IContainer components = null;

	private Label lbeCurrentPassword;

	private Label label1;

	private Label label2;

	private TextBox txtCurentPassword;

	private TextBox txtNewPass;

	private TextBox txtNewPassConfim;

	private Button btnSubmit;

	public frmChangePassword()
	{
		InitializeComponent();
	}

	private void frmChangePassword_Load(object sender, EventArgs e)
	{
		try
		{
			if (ApiClientNATech.loginResponse == null && MessageBox.Show("Chưa đăng nhập.", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand) == DialogResult.OK)
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSubmit_Click(object sender, EventArgs e)
	{
		try
		{
			ChangePasswordRequest strRequestData = new ChangePasswordRequest(ApiClientNATech.loginResponse.UserName, txtCurentPassword.Text.Trim(), txtNewPass.Text.Trim(), txtNewPassConfim.Text.Trim(), ApiClientNATech.SoftId);
			string sErr = string.Empty;
			string value = new ApiClientNATech().PostApi("Users/ChangePassword", strRequestData, ApiClientNATech.loginResponse.Token, out sErr);
			ChangePasswordResponse changePasswordResponse = JsonConvert.DeserializeObject<ChangePasswordResponse>(value);
			if (changePasswordResponse != null && changePasswordResponse.ChangeOk)
			{
				MessageBox.Show(this, "Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (changePasswordResponse != null && changePasswordResponse.sErr != null)
			{
				MessageBox.Show(this, changePasswordResponse.sErr.ErrorMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NATechApi.frmChangePassword));
		this.lbeCurrentPassword = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtCurentPassword = new System.Windows.Forms.TextBox();
		this.txtNewPass = new System.Windows.Forms.TextBox();
		this.txtNewPassConfim = new System.Windows.Forms.TextBox();
		this.btnSubmit = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.lbeCurrentPassword.AutoSize = true;
		this.lbeCurrentPassword.Location = new System.Drawing.Point(35, 69);
		this.lbeCurrentPassword.Name = "lbeCurrentPassword";
		this.lbeCurrentPassword.Size = new System.Drawing.Size(96, 20);
		this.lbeCurrentPassword.TabIndex = 0;
		this.lbeCurrentPassword.Text = "Mật khẩu cũ";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(35, 111);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(104, 20);
		this.label1.TabIndex = 0;
		this.label1.Text = "Mật khẩu mới";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(35, 150);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(165, 20);
		this.label2.TabIndex = 0;
		this.label2.Text = "Nhập lại mật khẩu mới";
		this.txtCurentPassword.Location = new System.Drawing.Point(231, 66);
		this.txtCurentPassword.Name = "txtCurentPassword";
		this.txtCurentPassword.PasswordChar = '*';
		this.txtCurentPassword.Size = new System.Drawing.Size(332, 26);
		this.txtCurentPassword.TabIndex = 0;
		this.txtNewPass.Location = new System.Drawing.Point(231, 108);
		this.txtNewPass.Name = "txtNewPass";
		this.txtNewPass.PasswordChar = '*';
		this.txtNewPass.Size = new System.Drawing.Size(332, 26);
		this.txtNewPass.TabIndex = 1;
		this.txtNewPassConfim.Location = new System.Drawing.Point(231, 147);
		this.txtNewPassConfim.Name = "txtNewPassConfim";
		this.txtNewPassConfim.PasswordChar = '*';
		this.txtNewPassConfim.Size = new System.Drawing.Size(332, 26);
		this.txtNewPassConfim.TabIndex = 2;
		this.btnSubmit.Location = new System.Drawing.Point(403, 210);
		this.btnSubmit.Name = "btnSubmit";
		this.btnSubmit.Size = new System.Drawing.Size(160, 36);
		this.btnSubmit.TabIndex = 3;
		this.btnSubmit.Text = "Cập nhật";
		this.btnSubmit.UseVisualStyleBackColor = true;
		this.btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(663, 320);
		base.Controls.Add(this.btnSubmit);
		base.Controls.Add(this.txtNewPassConfim);
		base.Controls.Add(this.txtNewPass);
		base.Controls.Add(this.txtCurentPassword);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.lbeCurrentPassword);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmChangePassword";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Đổi mật khẩu";
		base.Load += new System.EventHandler(frmChangePassword_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
