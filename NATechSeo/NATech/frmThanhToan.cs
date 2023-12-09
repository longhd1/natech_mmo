using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using NATechApi;

namespace NATech;

public class frmThanhToan : Form
{
	private IContainer components = null;

	private LabelControl labelControl6;

	private LabelControl labelControl7;

	private LabelControl labelControl8;

	private LabelControl lbeContentBank;

	private LabelControl labelControl1;

	private LabelControl labelControl3;

	private SimpleButton btnPriceTable;

	private PictureEdit pictureEdit1;

	private PictureEdit pictureEdit2;

	private PictureEdit pictureEdit3;

	private TextEdit textEdit1;

	private LabelControl labelControl2;

	private LabelControl labelControl4;

	public frmThanhToan()
	{
		InitializeComponent();
	}

	private void labelControl2_Click(object sender, EventArgs e)
	{
	}

	private void frmThanhToan_Load(object sender, EventArgs e)
	{
		try
		{
			if (ApiClientNATech.loginResponse != null)
			{
				lbeContentBank.Text = "SEO " + ApiClientNATech.loginResponse.UserName;
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void btnPriceTable_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start("https://na.com.vn/phan-mem-seo-traffic#tab-description");
		}
		catch (Exception)
		{
			throw;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NATech.frmThanhToan));
		this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
		this.lbeContentBank = new DevExpress.XtraEditors.LabelControl();
		this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
		this.btnPriceTable = new DevExpress.XtraEditors.SimpleButton();
		this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
		this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
		this.pictureEdit3 = new DevExpress.XtraEditors.PictureEdit();
		this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
		this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
		((System.ComponentModel.ISupportInitialize)this.pictureEdit1.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureEdit2.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureEdit3.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.textEdit1.Properties).BeginInit();
		base.SuspendLayout();
		this.labelControl6.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl6.Appearance.ForeColor = System.Drawing.Color.SeaGreen;
		this.labelControl6.Appearance.Options.UseFont = true;
		this.labelControl6.Appearance.Options.UseForeColor = true;
		this.labelControl6.Location = new System.Drawing.Point(40, 571);
		this.labelControl6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.labelControl6.Name = "labelControl6";
		this.labelControl6.Size = new System.Drawing.Size(151, 29);
		this.labelControl6.TabIndex = 0;
		this.labelControl6.Text = "Chủ tài khoản:";
		this.labelControl6.Click += new System.EventHandler(labelControl2_Click);
		this.labelControl7.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl7.Appearance.ForeColor = System.Drawing.Color.Blue;
		this.labelControl7.Appearance.Options.UseFont = true;
		this.labelControl7.Appearance.Options.UseForeColor = true;
		this.labelControl7.Location = new System.Drawing.Point(212, 575);
		this.labelControl7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.labelControl7.Name = "labelControl7";
		this.labelControl7.Size = new System.Drawing.Size(316, 29);
		this.labelControl7.TabIndex = 0;
		this.labelControl7.Text = "NGUYỄN THỊ NGỌC TRANG";
		this.labelControl7.Click += new System.EventHandler(labelControl2_Click);
		this.labelControl8.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl8.Appearance.ForeColor = System.Drawing.Color.SeaGreen;
		this.labelControl8.Appearance.Options.UseFont = true;
		this.labelControl8.Appearance.Options.UseForeColor = true;
		this.labelControl8.Location = new System.Drawing.Point(40, 614);
		this.labelControl8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.labelControl8.Name = "labelControl8";
		this.labelControl8.Size = new System.Drawing.Size(104, 29);
		this.labelControl8.TabIndex = 0;
		this.labelControl8.Text = "Nội dung:";
		this.labelControl8.Click += new System.EventHandler(labelControl2_Click);
		this.lbeContentBank.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbeContentBank.Appearance.ForeColor = System.Drawing.Color.Blue;
		this.lbeContentBank.Appearance.Options.UseFont = true;
		this.lbeContentBank.Appearance.Options.UseForeColor = true;
		this.lbeContentBank.Location = new System.Drawing.Point(212, 614);
		this.lbeContentBank.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.lbeContentBank.Name = "lbeContentBank";
		this.lbeContentBank.Size = new System.Drawing.Size(373, 29);
		this.lbeContentBank.TabIndex = 0;
		this.lbeContentBank.Text = "SEO_Username (vd: SEO_NATech)";
		this.lbeContentBank.Click += new System.EventHandler(labelControl2_Click);
		this.labelControl1.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Green;
		this.labelControl1.Appearance.Options.UseFont = true;
		this.labelControl1.Appearance.Options.UseForeColor = true;
		this.labelControl1.Location = new System.Drawing.Point(74, 669);
		this.labelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.labelControl1.Name = "labelControl1";
		this.labelControl1.Size = new System.Drawing.Size(1068, 25);
		this.labelControl1.TabIndex = 0;
		this.labelControl1.Text = "Lưu ý: Check email để nhận thông báo kích hoạt bản quyền. Nếu quá 15p chưa nhận được mail thì liên hệ fb.com/na.com.vn";
		this.labelControl1.Click += new System.EventHandler(labelControl2_Click);
		this.labelControl3.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Red;
		this.labelControl3.Appearance.Options.UseFont = true;
		this.labelControl3.Appearance.Options.UseForeColor = true;
		this.labelControl3.Location = new System.Drawing.Point(108, 14);
		this.labelControl3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.labelControl3.Name = "labelControl3";
		this.labelControl3.Size = new System.Drawing.Size(1018, 25);
		this.labelControl3.TabIndex = 0;
		this.labelControl3.Text = "Lưu ý: NATECH không chịu trách nhiệm trường hợp mất bản quyền khi khách hàng không giao dịch qua STK bên dưới";
		this.labelControl3.Click += new System.EventHandler(labelControl2_Click);
		this.btnPriceTable.Appearance.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.btnPriceTable.Appearance.ForeColor = System.Drawing.Color.Blue;
		this.btnPriceTable.Appearance.Options.UseFont = true;
		this.btnPriceTable.Appearance.Options.UseForeColor = true;
		this.btnPriceTable.Location = new System.Drawing.Point(481, 782);
		this.btnPriceTable.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.btnPriceTable.MinimumSize = new System.Drawing.Size(0, 46);
		this.btnPriceTable.Name = "btnPriceTable";
		this.btnPriceTable.Size = new System.Drawing.Size(255, 80);
		this.btnPriceTable.TabIndex = 35;
		this.btnPriceTable.Text = "Xem bảng giá";
		this.btnPriceTable.ToolTip = "Xem bảng giá chi tiết trên na.com.vn";
		this.btnPriceTable.Click += new System.EventHandler(btnPriceTable_Click);
		this.pictureEdit1.Cursor = System.Windows.Forms.Cursors.Default;
		this.pictureEdit1.EditValue = resources.GetObject("pictureEdit1.EditValue");
		this.pictureEdit1.Location = new System.Drawing.Point(40, 71);
		this.pictureEdit1.Name = "pictureEdit1";
		this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
		this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
		this.pictureEdit1.Properties.ZoomAccelerationFactor = 1.0;
		this.pictureEdit1.Size = new System.Drawing.Size(312, 477);
		this.pictureEdit1.TabIndex = 36;
		this.pictureEdit2.Cursor = System.Windows.Forms.Cursors.Default;
		this.pictureEdit2.EditValue = resources.GetObject("pictureEdit2.EditValue");
		this.pictureEdit2.Location = new System.Drawing.Point(481, 71);
		this.pictureEdit2.Name = "pictureEdit2";
		this.pictureEdit2.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
		this.pictureEdit2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
		this.pictureEdit2.Properties.ZoomAccelerationFactor = 1.0;
		this.pictureEdit2.Size = new System.Drawing.Size(335, 477);
		this.pictureEdit2.TabIndex = 37;
		this.pictureEdit3.Cursor = System.Windows.Forms.Cursors.Default;
		this.pictureEdit3.EditValue = resources.GetObject("pictureEdit3.EditValue");
		this.pictureEdit3.Location = new System.Drawing.Point(932, 71);
		this.pictureEdit3.Name = "pictureEdit3";
		this.pictureEdit3.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
		this.pictureEdit3.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
		this.pictureEdit3.Properties.ZoomAccelerationFactor = 1.0;
		this.pictureEdit3.Size = new System.Drawing.Size(335, 477);
		this.pictureEdit3.TabIndex = 38;
		this.textEdit1.EditValue = "0x6e926b1f4c35aa33b18620dadd11af2c8558d00a";
		this.textEdit1.Location = new System.Drawing.Point(874, 572);
		this.textEdit1.Name = "textEdit1";
		this.textEdit1.Size = new System.Drawing.Size(448, 26);
		this.textEdit1.TabIndex = 39;
		this.labelControl2.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl2.Appearance.ForeColor = System.Drawing.Color.SeaGreen;
		this.labelControl2.Appearance.Options.UseFont = true;
		this.labelControl2.Appearance.Options.UseForeColor = true;
		this.labelControl2.Location = new System.Drawing.Point(763, 571);
		this.labelControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.labelControl2.Name = "labelControl2";
		this.labelControl2.Size = new System.Drawing.Size(93, 29);
		this.labelControl2.TabIndex = 0;
		this.labelControl2.Text = "Ví USDT";
		this.labelControl2.Click += new System.EventHandler(labelControl2_Click);
		this.labelControl4.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Blue;
		this.labelControl4.Appearance.Options.UseFont = true;
		this.labelControl4.Appearance.Options.UseForeColor = true;
		this.labelControl4.Location = new System.Drawing.Point(899, 606);
		this.labelControl4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.labelControl4.Name = "labelControl4";
		this.labelControl4.Size = new System.Drawing.Size(347, 29);
		this.labelControl4.TabIndex = 0;
		this.labelControl4.Text = "Mạng BEP20 - BNB Smart Chain";
		this.labelControl4.Click += new System.EventHandler(labelControl2_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1430, 920);
		base.Controls.Add(this.textEdit1);
		base.Controls.Add(this.pictureEdit3);
		base.Controls.Add(this.pictureEdit2);
		base.Controls.Add(this.pictureEdit1);
		base.Controls.Add(this.btnPriceTable);
		base.Controls.Add(this.labelControl3);
		base.Controls.Add(this.labelControl1);
		base.Controls.Add(this.lbeContentBank);
		base.Controls.Add(this.labelControl4);
		base.Controls.Add(this.labelControl7);
		base.Controls.Add(this.labelControl8);
		base.Controls.Add(this.labelControl2);
		base.Controls.Add(this.labelControl6);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmThanhToan";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Thanh toán";
		base.Load += new System.EventHandler(frmThanhToan_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureEdit1.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureEdit2.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureEdit3.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.textEdit1.Properties).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
