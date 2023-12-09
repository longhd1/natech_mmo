using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NATech;

public class frmChangeMAC : Form
{
	private IContainer components = null;

	private TextBox txtCurrentMac;

	private Button txtReadMAC;

	public frmChangeMAC()
	{
		InitializeComponent();
	}

	private void txtReadMAC_Click(object sender, EventArgs e)
	{
		RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\\r\n{4D36E972-E325-11CE-BFC1-08002BE10318}\\0011", writable: true);
		string text = (string)registryKey.GetValue("NetworkAddress");
		registryKey.Close();
		txtCurrentMac.Text = text;
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
		this.txtCurrentMac = new System.Windows.Forms.TextBox();
		this.txtReadMAC = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.txtCurrentMac.Location = new System.Drawing.Point(100, 59);
		this.txtCurrentMac.Name = "txtCurrentMac";
		this.txtCurrentMac.Size = new System.Drawing.Size(220, 20);
		this.txtCurrentMac.TabIndex = 0;
		this.txtReadMAC.Location = new System.Drawing.Point(357, 59);
		this.txtReadMAC.Name = "txtReadMAC";
		this.txtReadMAC.Size = new System.Drawing.Size(75, 23);
		this.txtReadMAC.TabIndex = 1;
		this.txtReadMAC.Text = "Đọc MAC";
		this.txtReadMAC.UseVisualStyleBackColor = true;
		this.txtReadMAC.Click += new System.EventHandler(txtReadMAC_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(950, 413);
		base.Controls.Add(this.txtReadMAC);
		base.Controls.Add(this.txtCurrentMac);
		base.Name = "frmChangeMAC";
		this.Text = "frmChangeMAC";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
