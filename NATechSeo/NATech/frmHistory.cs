using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace NATech;

public class frmHistory : Form
{
	private IContainer components = null;

	private LabelControl labelControl1;

	private LabelControl labelControl2;

	private LabelControl labelControl3;

	public frmHistory()
	{
		InitializeComponent();
	}

	private void labelControl2_Click(object sender, EventArgs e)
	{
		Process.Start("https://www.facebook.com/ThietKePhanMemQuanLy");
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NATech.frmHistory));
		this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
		base.SuspendLayout();
		this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 13f, System.Drawing.FontStyle.Bold);
		this.labelControl1.Appearance.ForeColor = System.Drawing.Color.RoyalBlue;
		this.labelControl1.Location = new System.Drawing.Point(98, 69);
		this.labelControl1.Name = "labelControl1";
		this.labelControl1.Size = new System.Drawing.Size(268, 22);
		this.labelControl1.TabIndex = 0;
		this.labelControl1.Text = "Tác giả: NATECH (na.com.vn)";
		this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 13f, System.Drawing.FontStyle.Bold);
		this.labelControl2.Appearance.ForeColor = System.Drawing.Color.SeaGreen;
		this.labelControl2.Location = new System.Drawing.Point(49, 97);
		this.labelControl2.Name = "labelControl2";
		this.labelControl2.Size = new System.Drawing.Size(368, 21);
		this.labelControl2.TabIndex = 0;
		this.labelControl2.Text = "Fanpage: fb.com/ThietKePhanMemQuanLy";
		this.labelControl2.Click += new System.EventHandler(labelControl2_Click);
		this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 13f, System.Drawing.FontStyle.Bold);
		this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Crimson;
		this.labelControl3.Location = new System.Drawing.Point(111, 124);
		this.labelControl3.Name = "labelControl3";
		this.labelControl3.Size = new System.Drawing.Size(232, 21);
		this.labelControl3.TabIndex = 0;
		this.labelControl3.Text = "Phone/Zalo: 09222.33.666";
		this.labelControl3.Click += new System.EventHandler(labelControl2_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(793, 329);
		base.Controls.Add(this.labelControl3);
		base.Controls.Add(this.labelControl2);
		base.Controls.Add(this.labelControl1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmHistory";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Tác giả";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
