namespace ScanMFC
{
	partial class FormImageEditor
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// FormImageEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Name = "FormImageEditor";
			this.Text = "FormImageEditor";
			this.Load += new System.EventHandler(this.FormImageEditor_Load);
			this.ResumeLayout(false);

			imageEditor1 = new CraftSynth.ImageEditor.MainForm();
			//this.imageEditor1.ArgumentFile = "";
			this.imageEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imageEditor1.InitialImage = null;
			this.imageEditor1.InitialImageAsFilePath = Form1.filePath;
			this.imageEditor1.InitialImageAsPngBytes = null;
			this.imageEditor1.Location = new System.Drawing.Point(0, 24);
			this.imageEditor1.Name = "imageEditor1";
			this.imageEditor1.Size = new System.Drawing.Size(634, 417);
			this.imageEditor1.TabIndex = 1;
			this.imageEditor1.ZoomOnMouseWheel = false;

			this.Controls.Add(imageEditor1);
		}

		#endregion
	}
}