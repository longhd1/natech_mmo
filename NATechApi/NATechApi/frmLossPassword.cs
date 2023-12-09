using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using NATechApi.Models;

namespace NATechApi;

public class frmLossPassword : Form
{
	private IContainer components = null;

	private Label lbeUserName;

	private TextBox txtUserName;

	private Button btnRequire;

	private TextBox txtSubmitCode;

	private Label lbeSubmitCode;

	private Label lbeNotice;

	private Button btnSubmit;

	public frmLossPassword()
	{
		InitializeComponent();
	}

	private void frmLossPassword_Load(object sender, EventArgs e)
	{
	}

	private void btnRequire_Click(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
			{
				MessageBox.Show("Tên đăng nhập/email bắt buộc nhập", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			ForgotPasswordRequireRequest strRequestData = new ForgotPasswordRequireRequest(txtUserName.Text.Trim(), ApiClientNATech.SoftId);
			string sErr = string.Empty;
			string value = new ApiClientNATech().PostApi("Users/ForgotPasswordRequire", strRequestData, string.Empty, out sErr);
			ForgotPasswordResponse forgotPasswordResponse = JsonConvert.DeserializeObject<ForgotPasswordResponse>(value);
			if (forgotPasswordResponse != null && forgotPasswordResponse.ResetOk)
			{
				txtUserName.Enabled = false;
				lbeSubmitCode.Visible = true;
				txtSubmitCode.Visible = true;
				btnSubmit.Visible = true;
				lbeNotice.Visible = true;
				btnRequire.Enabled = false;
			}
			else if (forgotPasswordResponse != null && forgotPasswordResponse.sErr != null)
			{
				MessageBox.Show(this, forgotPasswordResponse.sErr.ErrorMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
			{
				MessageBox.Show("Tên đăng nhập/email bắt buộc nhập", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (string.IsNullOrEmpty(txtSubmitCode.Text.Trim()))
			{
				MessageBox.Show("Mã xác nhận bắt buộc nhập", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			ForgotPasswordSubmitRequest strRequestData = new ForgotPasswordSubmitRequest(txtUserName.Text.Trim(), txtSubmitCode.Text.Trim(), ApiClientNATech.SoftId);
			string sErr = string.Empty;
			string value = new ApiClientNATech().PostApi("Users/ForgotPasswordSubmit", strRequestData, string.Empty, out sErr);
			ForgotPasswordResponse forgotPasswordResponse = JsonConvert.DeserializeObject<ForgotPasswordResponse>(value);
			if (forgotPasswordResponse != null && forgotPasswordResponse.ResetOk)
			{
				lbeNotice.Text = "Đã cấp lại mật khẩu mới thành công! Hãy kiểm tra hộp thư đến/spam để lấy mật khẩu";
				lbeNotice.ForeColor = Color.Blue;
				MessageBox.Show(lbeNotice.Text, "Cấp lại mật khẩu thành công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (forgotPasswordResponse != null && forgotPasswordResponse.sErr != null)
			{
				MessageBox.Show(this, forgotPasswordResponse.sErr.ErrorMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				lbeNotice.Text = forgotPasswordResponse.sErr.ErrorMessage;
				lbeNotice.ForeColor = Color.Red;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtUserName_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyCode == Keys.Return)
			{
				btnRequire_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtSubmitCode_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyCode == Keys.Return)
			{
				btnSubmit_Click(null, null);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NATechApi.frmLossPassword));
		this.lbeUserName = new System.Windows.Forms.Label();
		this.txtUserName = new System.Windows.Forms.TextBox();
		this.btnRequire = new System.Windows.Forms.Button();
		this.txtSubmitCode = new System.Windows.Forms.TextBox();
		this.lbeSubmitCode = new System.Windows.Forms.Label();
		this.lbeNotice = new System.Windows.Forms.Label();
		this.btnSubmit = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.lbeUserName.AutoSize = true;
		this.lbeUserName.Location = new System.Drawing.Point(97, 87);
		this.lbeUserName.Name = "lbeUserName";
		this.lbeUserName.Size = new System.Drawing.Size(159, 20);
		this.lbeUserName.TabIndex = 0;
		this.lbeUserName.Text = "Tên đăng nhập/Email";
		this.txtUserName.Location = new System.Drawing.Point(302, 81);
		this.txtUserName.Name = "txtUserName";
		this.txtUserName.Size = new System.Drawing.Size(295, 26);
		this.txtUserName.TabIndex = 0;
		this.txtUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(txtUserName_KeyDown);
		this.btnRequire.Location = new System.Drawing.Point(445, 123);
		this.btnRequire.Name = "btnRequire";
		this.btnRequire.Size = new System.Drawing.Size(152, 35);
		this.btnRequire.TabIndex = 1;
		this.btnRequire.Text = "Lấy mã xác nhận";
		this.btnRequire.UseVisualStyleBackColor = true;
		this.btnRequire.Click += new System.EventHandler(btnRequire_Click);
		this.txtSubmitCode.Location = new System.Drawing.Point(302, 173);
		this.txtSubmitCode.Name = "txtSubmitCode";
		this.txtSubmitCode.Size = new System.Drawing.Size(295, 26);
		this.txtSubmitCode.TabIndex = 2;
		this.txtSubmitCode.Visible = false;
		this.txtSubmitCode.KeyDown += new System.Windows.Forms.KeyEventHandler(txtSubmitCode_KeyDown);
		this.lbeSubmitCode.AutoSize = true;
		this.lbeSubmitCode.Location = new System.Drawing.Point(97, 179);
		this.lbeSubmitCode.Name = "lbeSubmitCode";
		this.lbeSubmitCode.Size = new System.Drawing.Size(141, 20);
		this.lbeSubmitCode.TabIndex = 0;
		this.lbeSubmitCode.Text = "Nhập mã kích hoạt";
		this.lbeSubmitCode.Visible = false;
		this.lbeNotice.AutoSize = true;
		this.lbeNotice.ForeColor = System.Drawing.Color.MediumBlue;
		this.lbeNotice.Location = new System.Drawing.Point(97, 270);
		this.lbeNotice.Name = "lbeNotice";
		this.lbeNotice.Size = new System.Drawing.Size(574, 20);
		this.lbeNotice.TabIndex = 4;
		this.lbeNotice.Text = "Mã xác nhận đã được gửi vào email. Hãy kiểm tra hộp thư đến/Spam để nhận mã";
		this.lbeNotice.Visible = false;
		this.btnSubmit.Location = new System.Drawing.Point(445, 218);
		this.btnSubmit.Name = "btnSubmit";
		this.btnSubmit.Size = new System.Drawing.Size(152, 35);
		this.btnSubmit.TabIndex = 3;
		this.btnSubmit.Text = "Cấp lại mật khẩu";
		this.btnSubmit.UseVisualStyleBackColor = true;
		this.btnSubmit.Visible = false;
		this.btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(816, 354);
		base.Controls.Add(this.lbeNotice);
		base.Controls.Add(this.txtSubmitCode);
		base.Controls.Add(this.btnSubmit);
		base.Controls.Add(this.btnRequire);
		base.Controls.Add(this.txtUserName);
		base.Controls.Add(this.lbeSubmitCode);
		base.Controls.Add(this.lbeUserName);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmLossPassword";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Quên mật khẩu";
		base.Load += new System.EventHandler(frmLossPassword_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
