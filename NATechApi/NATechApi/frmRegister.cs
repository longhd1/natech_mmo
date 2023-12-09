using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using NATechApi.Models;

namespace NATechApi;

public class frmRegister : Form
{
	private IContainer components = null;

	private Label label1;

	private TextBox txtUserName;

	private Label label2;

	private TextBox txtEmail;

	private Label label3;

	private TextBox txtPassword;

	private Label label4;

	private TextBox txtPasswordConfim;

	private Label label5;

	private TextBox txtPhone;

	private Button btnSubmit;

	private Button btnClose;

	public frmRegister()
	{
		InitializeComponent();
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnSubmit_Click(object sender, EventArgs e)
	{
		try
		{
			RegisterRequest strRequestData = new RegisterRequest(txtUserName.Text.Trim(), txtEmail.Text.Trim(), txtPassword.Text.Trim(), txtPasswordConfim.Text.Trim(), txtPhone.Text.Trim(), ApiClientNATech.HardKey, ApiClientNATech.SoftId);
			string sErr = string.Empty;
			string value = new ApiClientNATech().PostApi("Users/Register", strRequestData, string.Empty, out sErr);
			RegisterResponse registerResponse = JsonConvert.DeserializeObject<RegisterResponse>(value);
			if (registerResponse != null && registerResponse.UserId != 0)
			{
				MessageBox.Show(this, "Đăng ký thành công! Tiến hành kích hoạt tài khoản", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				frmActiveAccount frmActiveAccount2 = new frmActiveAccount();
				frmActiveAccount2.Email = registerResponse.Email;
				frmActiveAccount2.ShowDialog(this);
				if (frmActiveAccount2.IsActived)
				{
					Close();
				}
				else
				{
					MessageBox.Show(this, "Chưa kích hoạt tài khoản. Hãy kích hoạt tài khoản để sử dụng phần mềm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
			else if (registerResponse != null && registerResponse.sErr != null)
			{
				MessageBox.Show(this, registerResponse.sErr.ErrorMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtUserName_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			txtEmail.Focus();
		}
	}

	private void txtEmail_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			txtPassword.Focus();
		}
	}

	private void txtPassword_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			txtPasswordConfim.Focus();
		}
	}

	private void txtPasswordConfim_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			txtPhone.Focus();
		}
	}

	private void txtPhone_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			btnSubmit_Click(null, null);
		}
	}

	private void frmRegister_Load(object sender, EventArgs e)
	{
		try
		{
			txtUserName.Focus();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NATechApi.frmRegister));
		this.label1 = new System.Windows.Forms.Label();
		this.txtUserName = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtEmail = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtPassword = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtPasswordConfim = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtPhone = new System.Windows.Forms.TextBox();
		this.btnSubmit = new System.Windows.Forms.Button();
		this.btnClose = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(85, 51);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(116, 20);
		this.label1.TabIndex = 0;
		this.label1.Text = "Tên đăng nhập";
		this.txtUserName.Location = new System.Drawing.Point(243, 45);
		this.txtUserName.Name = "txtUserName";
		this.txtUserName.Size = new System.Drawing.Size(321, 26);
		this.txtUserName.TabIndex = 0;
		this.txtUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(txtUserName_KeyDown);
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(85, 83);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 20);
		this.label2.TabIndex = 0;
		this.label2.Text = "Email";
		this.txtEmail.Location = new System.Drawing.Point(243, 77);
		this.txtEmail.Name = "txtEmail";
		this.txtEmail.Size = new System.Drawing.Size(321, 26);
		this.txtEmail.TabIndex = 1;
		this.txtEmail.KeyDown += new System.Windows.Forms.KeyEventHandler(txtEmail_KeyDown);
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(85, 115);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(75, 20);
		this.label3.TabIndex = 0;
		this.label3.Text = "Mật khẩu";
		this.txtPassword.Location = new System.Drawing.Point(243, 109);
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.PasswordChar = '*';
		this.txtPassword.Size = new System.Drawing.Size(321, 26);
		this.txtPassword.TabIndex = 2;
		this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(txtPassword_KeyDown);
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(85, 147);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(136, 20);
		this.label4.TabIndex = 0;
		this.label4.Text = "Nhập lại mật khẩu";
		this.txtPasswordConfim.Location = new System.Drawing.Point(243, 141);
		this.txtPasswordConfim.Name = "txtPasswordConfim";
		this.txtPasswordConfim.PasswordChar = '*';
		this.txtPasswordConfim.Size = new System.Drawing.Size(321, 26);
		this.txtPasswordConfim.TabIndex = 3;
		this.txtPasswordConfim.KeyDown += new System.Windows.Forms.KeyEventHandler(txtPasswordConfim_KeyDown);
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(85, 179);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(81, 20);
		this.label5.TabIndex = 0;
		this.label5.Text = "Điện thoại";
		this.txtPhone.Location = new System.Drawing.Point(243, 173);
		this.txtPhone.Name = "txtPhone";
		this.txtPhone.Size = new System.Drawing.Size(321, 26);
		this.txtPhone.TabIndex = 4;
		this.txtPhone.KeyDown += new System.Windows.Forms.KeyEventHandler(txtPhone_KeyDown);
		this.btnSubmit.Location = new System.Drawing.Point(294, 224);
		this.btnSubmit.Name = "btnSubmit";
		this.btnSubmit.Size = new System.Drawing.Size(131, 36);
		this.btnSubmit.TabIndex = 5;
		this.btnSubmit.Text = "Đăng ký";
		this.btnSubmit.UseVisualStyleBackColor = true;
		this.btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
		this.btnClose.Location = new System.Drawing.Point(431, 224);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(131, 36);
		this.btnClose.TabIndex = 6;
		this.btnClose.Text = "Đóng";
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(683, 315);
		base.Controls.Add(this.btnClose);
		base.Controls.Add(this.btnSubmit);
		base.Controls.Add(this.txtPhone);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.txtPasswordConfim);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.txtPassword);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.txtEmail);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.txtUserName);
		base.Controls.Add(this.label1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmRegister";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Đăng ký tài khoản";
		base.Load += new System.EventHandler(frmRegister_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
