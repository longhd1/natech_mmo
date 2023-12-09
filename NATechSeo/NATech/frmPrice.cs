using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace NATech;

public class frmPrice : Form
{
	private IContainer components = null;

	private MemoEdit memoEdit1;

	public frmPrice()
	{
		InitializeComponent();
	}

	private void labelControl2_Click(object sender, EventArgs e)
	{
		Process.Start("https://www.facebook.com/na.com.vn");
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NATech.frmPrice));
		this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
		((System.ComponentModel.ISupportInitialize)this.memoEdit1.Properties).BeginInit();
		base.SuspendLayout();
		this.memoEdit1.EditValue = resources.GetString("memoEdit1.EditValue");
		this.memoEdit1.Location = new System.Drawing.Point(12, 12);
		this.memoEdit1.Name = "memoEdit1";
		this.memoEdit1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10f);
		this.memoEdit1.Properties.Appearance.Options.UseFont = true;
		this.memoEdit1.Size = new System.Drawing.Size(411, 196);
		this.memoEdit1.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(590, 343);
		base.Controls.Add(this.memoEdit1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmPrice";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Bảng giá";
		((System.ComponentModel.ISupportInitialize)this.memoEdit1.Properties).EndInit();
		base.ResumeLayout(false);
	}
}
