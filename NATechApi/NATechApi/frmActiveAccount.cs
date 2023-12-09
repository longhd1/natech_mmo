using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using NATechApi.Models;

namespace NATechApi;

public class frmActiveAccount : Form
{
	private IContainer components = null;

	private Button btnClose;

	private Button btnSubmit;

	private TextBox txtCode;

	private Label label3;

	private TextBox txtEmail;

	private Label label2;

	private Label label1;

	public string Email { get; set; }

	public bool IsActived { get; set; }

	public frmActiveAccount()
	{
		InitializeComponent();
	}

	private void btnSubmit_Click(object sender, EventArgs e)
	{
		try
		{
			ActiveRequest strRequestData = new ActiveRequest(txtEmail.Text.Trim(), txtCode.Text.Trim(), ApiClientNATech.SoftId);
			string sErr = string.Empty;
			string value = new ApiClientNATech().PostApi("Users/ActiveAccount", strRequestData, string.Empty, out sErr);
			ActiveResponse activeResponse = JsonConvert.DeserializeObject<ActiveResponse>(value);
			if (activeResponse != null && activeResponse.IsActived)
			{
				IsActived = true;
				MessageBox.Show(this, "Kích hoạt tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				Close();
				return;
			}
			IsActived = false;
			if (activeResponse != null && activeResponse.sErr != null)
			{
				MessageBox.Show(this, activeResponse.sErr.ErrorMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void frmActiveAccount_Load(object sender, EventArgs e)
	{
		try
		{
			if (!string.IsNullOrEmpty(Email))
			{
				txtEmail.Text = Email;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtEmail_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyCode == Keys.Return)
			{
				txtCode.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtCode_KeyDown(object sender, KeyEventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NATechApi.frmActiveAccount));
		this.btnClose = new System.Windows.Forms.Button();
		this.btnSubmit = new System.Windows.Forms.Button();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtEmail = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.btnClose.Location = new System.Drawing.Point(416, 154);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(131, 36);
		this.btnClose.TabIndex = 3;
		this.btnClose.Text = "Đóng";
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		this.btnSubmit.Location = new System.Drawing.Point(279, 154);
		this.btnSubmit.Name = "btnSubmit";
		this.btnSubmit.Size = new System.Drawing.Size(131, 36);
		this.btnSubmit.TabIndex = 2;
		this.btnSubmit.Text = "Kích hoạt";
		this.btnSubmit.UseVisualStyleBackColor = true;
		this.btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
		this.txtCode.Location = new System.Drawing.Point(227, 105);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(321, 26);
		this.txtCode.TabIndex = 1;
		this.txtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(txtCode_KeyDown);
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(69, 111);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(99, 20);
		this.label3.TabIndex = 3;
		this.label3.Text = "Mã kích hoạt";
		this.txtEmail.Location = new System.Drawing.Point(227, 73);
		this.txtEmail.Name = "txtEmail";
		this.txtEmail.Size = new System.Drawing.Size(321, 26);
		this.txtEmail.TabIndex = 0;
		this.txtEmail.KeyDown += new System.Windows.Forms.KeyEventHandler(txtEmail_KeyDown);
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(69, 79);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 20);
		this.label2.TabIndex = 4;
		this.label2.Text = "Email";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 163);
		this.label1.ForeColor = System.Drawing.Color.Blue;
		this.label1.Location = new System.Drawing.Point(107, 28);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(386, 20);
		this.label1.TabIndex = 9;
		this.label1.Text = "Mở hộp thư đến hoặc hòm thư rác để lấy mã kích hoạt";
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(644, 242);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.btnClose);
		base.Controls.Add(this.btnSubmit);
		base.Controls.Add(this.txtCode);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.txtEmail);
		base.Controls.Add(this.label2);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmActiveAccount";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Kích hoạt tài khoản";
		base.Load += new System.EventHandler(frmActiveAccount_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
