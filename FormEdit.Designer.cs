namespace ScanMFC
{
	partial class FormEdit
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEdit));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.toolTipFormEdit = new System.Windows.Forms.ToolTip(this.components);
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btnAround = new System.Windows.Forms.Button();
			this.btnRight = new System.Windows.Forms.Button();
			this.btnLeft = new System.Windows.Forms.Button();
			this.btnCrop = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(451, 553);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnAround);
			this.panel1.Controls.Add(this.btnRight);
			this.panel1.Controls.Add(this.btnLeft);
			this.panel1.Controls.Add(this.btnCrop);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(445, 34);
			this.panel1.TabIndex = 1;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnCancel);
			this.panel2.Controls.Add(this.btnOK);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 516);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(445, 34);
			this.panel2.TabIndex = 2;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(10, 2);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(91, 2);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Отмена";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point(10, 50);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(10);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(431, 453);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			this.pictureBox1.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
			this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
			this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
			// 
			// btnAround
			// 
			this.btnAround.Image = global::ScanMFC.Properties.Resources.reload;
			this.btnAround.Location = new System.Drawing.Point(104, 2);
			this.btnAround.Margin = new System.Windows.Forms.Padding(2);
			this.btnAround.Name = "btnAround";
			this.btnAround.Size = new System.Drawing.Size(30, 30);
			this.btnAround.TabIndex = 5;
			this.btnAround.UseVisualStyleBackColor = true;
			this.btnAround.Click += new System.EventHandler(this.btnAround_Click);
			// 
			// btnRight
			// 
			this.btnRight.Image = global::ScanMFC.Properties.Resources.right_arrow;
			this.btnRight.Location = new System.Drawing.Point(70, 2);
			this.btnRight.Margin = new System.Windows.Forms.Padding(2);
			this.btnRight.Name = "btnRight";
			this.btnRight.Size = new System.Drawing.Size(30, 30);
			this.btnRight.TabIndex = 4;
			this.btnRight.UseVisualStyleBackColor = true;
			this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
			// 
			// btnLeft
			// 
			this.btnLeft.Image = global::ScanMFC.Properties.Resources.left_curve_arrow;
			this.btnLeft.Location = new System.Drawing.Point(36, 2);
			this.btnLeft.Margin = new System.Windows.Forms.Padding(2);
			this.btnLeft.Name = "btnLeft";
			this.btnLeft.Size = new System.Drawing.Size(30, 30);
			this.btnLeft.TabIndex = 3;
			this.btnLeft.UseVisualStyleBackColor = true;
			this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
			// 
			// btnCrop
			// 
			this.btnCrop.Image = global::ScanMFC.Properties.Resources.crop_tool;
			this.btnCrop.Location = new System.Drawing.Point(2, 2);
			this.btnCrop.Margin = new System.Windows.Forms.Padding(2);
			this.btnCrop.Name = "btnCrop";
			this.btnCrop.Size = new System.Drawing.Size(30, 30);
			this.btnCrop.TabIndex = 2;
			this.btnCrop.UseVisualStyleBackColor = true;
			this.btnCrop.Click += new System.EventHandler(this.btnCrop_Click);
			// 
			// FormEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(451, 553);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FormEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "FormEdit";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormEdit_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnCrop;
		private System.Windows.Forms.Button btnRight;
		private System.Windows.Forms.Button btnLeft;
		private System.Windows.Forms.Button btnAround;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ToolTip toolTipFormEdit;
	}
}