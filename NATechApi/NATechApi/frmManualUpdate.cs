using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NATechApi;

public class frmManualUpdate : Form
{
	private IContainer components = null;

	public frmManualUpdate()
	{
		InitializeComponent();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NATechApi.frmManualUpdate));
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(814, 430);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmManualUpdate";
		this.Text = "frmManualUpdate";
		base.ResumeLayout(false);
	}
}
