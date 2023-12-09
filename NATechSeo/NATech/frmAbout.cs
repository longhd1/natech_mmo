using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace NATech;

public class frmAbout : Form
{
	private IContainer components = null;

	private LabelControl labelControl1;

	private LabelControl labelControl2;

	private LabelControl labelControl3;

	private LabelControl labelControl4;

	private LabelControl labelControl5;

	public frmAbout()
	{
		InitializeComponent();
	}

	private void labelControl2_Click(object sender, EventArgs e)
	{
		Process.Start("https://www.facebook.com/na.com.vn");
	}

	private void frmAbout_Load(object sender, EventArgs e)
	{
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NATech.frmAbout));
		this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
		base.SuspendLayout();
		this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 13f, System.Drawing.FontStyle.Bold);
		this.labelControl1.Appearance.ForeColor = System.Drawing.Color.RoyalBlue;
		this.labelControl1.Location = new System.Drawing.Point(162, 40);
		this.labelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.labelControl1.Name = "labelControl1";
		this.labelControl1.Size = new System.Drawing.Size(224, 31);
		this.labelControl1.TabIndex = 0;
		this.labelControl1.Text = "Tác giả: NATECH";
		this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 13f, System.Drawing.FontStyle.Bold);
		this.labelControl2.Appearance.ForeColor = System.Drawing.Color.PaleVioletRed;
		this.labelControl2.Location = new System.Drawing.Point(101, 89);
		this.labelControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.labelControl2.Name = "labelControl2";
		this.labelControl2.Size = new System.Drawing.Size(368, 31);
		this.labelControl2.TabIndex = 0;
		this.labelControl2.Text = "Website: https://na.com.vn";
		this.labelControl2.Click += new System.EventHandler(labelControl2_Click);
		this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 13f, System.Drawing.FontStyle.Bold);
		this.labelControl3.Appearance.ForeColor = System.Drawing.Color.DodgerBlue;
		this.labelControl3.Location = new System.Drawing.Point(101, 171);
		this.labelControl3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.labelControl3.Name = "labelControl3";
		this.labelControl3.Size = new System.Drawing.Size(356, 31);
		this.labelControl3.TabIndex = 0;
		this.labelControl3.Text = "Phone/Zalo: 09222.33.666";
		this.labelControl3.Click += new System.EventHandler(labelControl2_Click);
		this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 13f, System.Drawing.FontStyle.Bold);
		this.labelControl4.Appearance.ForeColor = System.Drawing.Color.RoyalBlue;
		this.labelControl4.Location = new System.Drawing.Point(101, 130);
		this.labelControl4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.labelControl4.Name = "labelControl4";
		this.labelControl4.Size = new System.Drawing.Size(371, 31);
		this.labelControl4.TabIndex = 0;
		this.labelControl4.Text = "Fanpage: fb.com/na.com.vn";
		this.labelControl4.Click += new System.EventHandler(labelControl2_Click);
		this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 13f, System.Drawing.FontStyle.Bold);
		this.labelControl5.Appearance.ForeColor = System.Drawing.Color.DeepSkyBlue;
		this.labelControl5.Location = new System.Drawing.Point(101, 212);
		this.labelControl5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.labelControl5.Name = "labelControl5";
		this.labelControl5.Size = new System.Drawing.Size(262, 31);
		this.labelControl5.TabIndex = 0;
		this.labelControl5.Text = "Telegram: @longhoang02363";
		this.labelControl5.Click += new System.EventHandler(labelControl2_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(652, 338);
		base.Controls.Add(this.labelControl5);
		base.Controls.Add(this.labelControl3);
		base.Controls.Add(this.labelControl4);
		base.Controls.Add(this.labelControl2);
		base.Controls.Add(this.labelControl1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmAbout";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Tác giả";
		base.Load += new System.EventHandler(frmAbout_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
