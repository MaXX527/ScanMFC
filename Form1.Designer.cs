namespace ScanMFC
{
	partial class Form1
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.button1 = new System.Windows.Forms.Button();
			this.chkFeeder = new System.Windows.Forms.CheckBox();
			this.chkDuplex = new System.Windows.Forms.CheckBox();
			this.cmbColor = new System.Windows.Forms.ComboBox();
			this.cmbResolution = new System.Windows.Forms.ComboBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.trkJpegQuality = new System.Windows.Forms.TrackBar();
			this.lblJpegQuality = new System.Windows.Forms.Label();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabelFilesCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripDSM = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripMsg = new System.Windows.Forms.ToolStripStatusLabel();
			this.savePDFDialog = new System.Windows.Forms.SaveFileDialog();
			this.cmbScanners = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblContrast = new System.Windows.Forms.Label();
			this.lblBrightness = new System.Windows.Forms.Label();
			this.trkContrast = new System.Windows.Forms.TrackBar();
			this.trkBrightness = new System.Windows.Forms.TrackBar();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnDeskew = new System.Windows.Forms.Button();
			this.lblDeskew = new System.Windows.Forms.Label();
			this.numDeskew = new System.Windows.Forms.NumericUpDown();
			this.cmbDSM = new System.Windows.Forms.ComboBox();
			this.lblDSM = new System.Windows.Forms.Label();
			this.chkShowInterface = new System.Windows.Forms.CheckBox();
			this.chkDeleteFiles = new System.Windows.Forms.CheckBox();
			this.btnSelectDir = new System.Windows.Forms.Button();
			this.txtFileDir = new System.Windows.Forms.TextBox();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnMoveLeft = new System.Windows.Forms.Button();
			this.btnMoveRight = new System.Windows.Forms.Button();
			this.btnPDF = new System.Windows.Forms.Button();
			this.btnTIFF = new System.Windows.Forms.Button();
			this.radioiTextSharp = new System.Windows.Forms.RadioButton();
			this.radioImageMagick = new System.Windows.Forms.RadioButton();
			this.dlgSelectDir = new System.Windows.Forms.FolderBrowserDialog();
			this.dlgAddFiles = new System.Windows.Forms.OpenFileDialog();
			this.toolTipForm1 = new System.Windows.Forms.ToolTip(this.components);
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			((System.ComponentModel.ISupportInitialize)(this.trkJpegQuality)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkContrast)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trkBrightness)).BeginInit();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numDeskew)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.button1.Location = new System.Drawing.Point(3, 453);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(132, 35);
			this.button1.TabIndex = 0;
			this.button1.Text = "Сканировать";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// chkFeeder
			// 
			this.chkFeeder.AutoSize = true;
			this.chkFeeder.Location = new System.Drawing.Point(10, 50);
			this.chkFeeder.Name = "chkFeeder";
			this.chkFeeder.Size = new System.Drawing.Size(96, 17);
			this.chkFeeder.TabIndex = 3;
			this.chkFeeder.Text = "Автоподатчик";
			this.chkFeeder.UseVisualStyleBackColor = true;
			// 
			// chkDuplex
			// 
			this.chkDuplex.AutoSize = true;
			this.chkDuplex.Location = new System.Drawing.Point(112, 50);
			this.chkDuplex.Name = "chkDuplex";
			this.chkDuplex.Size = new System.Drawing.Size(92, 17);
			this.chkDuplex.TabIndex = 4;
			this.chkDuplex.Text = "Обе стороны";
			this.chkDuplex.UseVisualStyleBackColor = true;
			// 
			// cmbColor
			// 
			this.cmbColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbColor.FormattingEnabled = true;
			this.cmbColor.Items.AddRange(new object[] {
            "Черно-белое",
            "Оттенки серого",
            "Цветное"});
			this.cmbColor.Location = new System.Drawing.Point(89, 25);
			this.cmbColor.Name = "cmbColor";
			this.cmbColor.Size = new System.Drawing.Size(121, 21);
			this.cmbColor.TabIndex = 5;
			// 
			// cmbResolution
			// 
			this.cmbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbResolution.FormattingEnabled = true;
			this.cmbResolution.Items.AddRange(new object[] {
            "75",
            "100",
            "150",
            "200",
            "300",
            "400",
            "600",
            "800",
            "1200",
            "2400"});
			this.cmbResolution.Location = new System.Drawing.Point(89, 52);
			this.cmbResolution.Name = "cmbResolution";
			this.cmbResolution.Size = new System.Drawing.Size(121, 21);
			this.cmbResolution.TabIndex = 6;
			// 
			// listView1
			// 
			this.listView1.AllowDrop = true;
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(283, 78);
			this.listView1.Name = "listView1";
			this.tableLayoutPanel1.SetRowSpan(this.listView1, 3);
			this.listView1.Size = new System.Drawing.Size(513, 467);
			this.listView1.TabIndex = 7;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView1_ItemDrag);
			this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
			this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
			this.listView1.DragOver += new System.Windows.Forms.DragEventHandler(this.listView1_DragOver);
			this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
			// 
			// trkJpegQuality
			// 
			this.trkJpegQuality.AutoSize = false;
			this.trkJpegQuality.LargeChange = 2;
			this.trkJpegQuality.Location = new System.Drawing.Point(76, 79);
			this.trkJpegQuality.Minimum = 1;
			this.trkJpegQuality.Name = "trkJpegQuality";
			this.trkJpegQuality.Size = new System.Drawing.Size(121, 20);
			this.trkJpegQuality.TabIndex = 10;
			this.trkJpegQuality.Value = 1;
			this.trkJpegQuality.ValueChanged += new System.EventHandler(this.trkJpegQuality_ValueChanged);
			// 
			// lblJpegQuality
			// 
			this.lblJpegQuality.AutoSize = true;
			this.lblJpegQuality.Location = new System.Drawing.Point(203, 86);
			this.lblJpegQuality.Name = "lblJpegQuality";
			this.lblJpegQuality.Size = new System.Drawing.Size(19, 13);
			this.lblJpegQuality.TabIndex = 11;
			this.lblJpegQuality.Text = "10";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatus,
            this.toolStripStatusLabelFilesCount,
            this.toolStripDSM,
            this.toolStripMsg,
            this.toolStripProgressBar1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 548);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(799, 22);
			this.statusStrip1.TabIndex = 12;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatus
			// 
			this.toolStripStatus.Name = "toolStripStatus";
			this.toolStripStatus.Size = new System.Drawing.Size(38, 17);
			this.toolStripStatus.Text = "Готов";
			// 
			// toolStripStatusLabelFilesCount
			// 
			this.toolStripStatusLabelFilesCount.Name = "toolStripStatusLabelFilesCount";
			this.toolStripStatusLabelFilesCount.Size = new System.Drawing.Size(37, 17);
			this.toolStripStatusLabelFilesCount.Text = "______";
			// 
			// toolStripDSM
			// 
			this.toolStripDSM.Name = "toolStripDSM";
			this.toolStripDSM.Size = new System.Drawing.Size(35, 17);
			this.toolStripDSM.Text = "DSM:";
			// 
			// toolStripMsg
			// 
			this.toolStripMsg.Name = "toolStripMsg";
			this.toolStripMsg.Size = new System.Drawing.Size(75, 17);
			this.toolStripMsg.Text = "toolStripMsg";
			// 
			// cmbScanners
			// 
			this.cmbScanners.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbScanners.FormattingEnabled = true;
			this.cmbScanners.Location = new System.Drawing.Point(57, 23);
			this.cmbScanners.Name = "cmbScanners";
			this.cmbScanners.Size = new System.Drawing.Size(213, 21);
			this.cmbScanners.TabIndex = 15;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.cmbScanners);
			this.groupBox1.Controls.Add(this.chkFeeder);
			this.groupBox1.Controls.Add(this.chkDuplex);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(274, 69);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Источник";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "Сканер";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.cmbColor);
			this.groupBox2.Controls.Add(this.cmbResolution);
			this.groupBox2.Controls.Add(this.lblContrast);
			this.groupBox2.Controls.Add(this.lblBrightness);
			this.groupBox2.Controls.Add(this.lblJpegQuality);
			this.groupBox2.Controls.Add(this.trkContrast);
			this.groupBox2.Controls.Add(this.trkBrightness);
			this.groupBox2.Controls.Add(this.trkJpegQuality);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(3, 78);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(274, 169);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Параметры изображения";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(7, 138);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(57, 13);
			this.label9.TabIndex = 8;
			this.label9.Text = "Контраст:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(7, 112);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 13);
			this.label7.TabIndex = 8;
			this.label7.Text = "Яркость:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(7, 86);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(57, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Качество:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(7, 55);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(73, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Разрешение:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 28);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Режим:";
			// 
			// lblContrast
			// 
			this.lblContrast.AutoSize = true;
			this.lblContrast.Location = new System.Drawing.Point(203, 138);
			this.lblContrast.Name = "lblContrast";
			this.lblContrast.Size = new System.Drawing.Size(13, 13);
			this.lblContrast.TabIndex = 11;
			this.lblContrast.Text = "1";
			// 
			// lblBrightness
			// 
			this.lblBrightness.AutoSize = true;
			this.lblBrightness.Location = new System.Drawing.Point(203, 112);
			this.lblBrightness.Name = "lblBrightness";
			this.lblBrightness.Size = new System.Drawing.Size(13, 13);
			this.lblBrightness.TabIndex = 11;
			this.lblBrightness.Text = "1";
			// 
			// trkContrast
			// 
			this.trkContrast.AutoSize = false;
			this.trkContrast.LargeChange = 10;
			this.trkContrast.Location = new System.Drawing.Point(76, 131);
			this.trkContrast.Maximum = 100;
			this.trkContrast.Minimum = -100;
			this.trkContrast.Name = "trkContrast";
			this.trkContrast.Size = new System.Drawing.Size(121, 20);
			this.trkContrast.TabIndex = 10;
			this.trkContrast.TickFrequency = 10;
			this.trkContrast.ValueChanged += new System.EventHandler(this.trkContrast_ValueChanged);
			// 
			// trkBrightness
			// 
			this.trkBrightness.AutoSize = false;
			this.trkBrightness.LargeChange = 10;
			this.trkBrightness.Location = new System.Drawing.Point(76, 105);
			this.trkBrightness.Maximum = 100;
			this.trkBrightness.Minimum = -100;
			this.trkBrightness.Name = "trkBrightness";
			this.trkBrightness.Size = new System.Drawing.Size(121, 20);
			this.trkBrightness.TabIndex = 10;
			this.trkBrightness.TickFrequency = 20;
			this.trkBrightness.ValueChanged += new System.EventHandler(this.trkBrightness_ValueChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.btnDeskew);
			this.groupBox3.Controls.Add(this.lblDeskew);
			this.groupBox3.Controls.Add(this.numDeskew);
			this.groupBox3.Controls.Add(this.cmbDSM);
			this.groupBox3.Controls.Add(this.lblDSM);
			this.groupBox3.Controls.Add(this.chkShowInterface);
			this.groupBox3.Controls.Add(this.chkDeleteFiles);
			this.groupBox3.Controls.Add(this.btnSelectDir);
			this.groupBox3.Controls.Add(this.txtFileDir);
			this.groupBox3.Controls.Add(this.txtFileName);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(3, 253);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(274, 194);
			this.groupBox3.TabIndex = 18;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Настройки файла";
			// 
			// btnDeskew
			// 
			this.btnDeskew.Location = new System.Drawing.Point(13, 138);
			this.btnDeskew.Name = "btnDeskew";
			this.btnDeskew.Size = new System.Drawing.Size(130, 20);
			this.btnDeskew.TabIndex = 11;
			this.btnDeskew.Text = "Выровнять сканы";
			this.btnDeskew.UseVisualStyleBackColor = true;
			this.btnDeskew.Click += new System.EventHandler(this.btnDeskew_Click);
			// 
			// lblDeskew
			// 
			this.lblDeskew.AutoSize = true;
			this.lblDeskew.Location = new System.Drawing.Point(190, 142);
			this.lblDeskew.Name = "lblDeskew";
			this.lblDeskew.Size = new System.Drawing.Size(15, 13);
			this.lblDeskew.TabIndex = 10;
			this.lblDeskew.Text = "%";
			// 
			// numDeskew
			// 
			this.numDeskew.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numDeskew.Location = new System.Drawing.Point(149, 138);
			this.numDeskew.Name = "numDeskew";
			this.numDeskew.Size = new System.Drawing.Size(38, 20);
			this.numDeskew.TabIndex = 9;
			// 
			// cmbDSM
			// 
			this.cmbDSM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDSM.FormattingEnabled = true;
			this.cmbDSM.Items.AddRange(new object[] {
            "Version 2",
            "Legacy"});
			this.cmbDSM.Location = new System.Drawing.Point(88, 167);
			this.cmbDSM.Name = "cmbDSM";
			this.cmbDSM.Size = new System.Drawing.Size(151, 21);
			this.cmbDSM.TabIndex = 8;
			this.cmbDSM.SelectedIndexChanged += new System.EventHandler(this.cmbDSM_SelectedIndexChanged);
			// 
			// lblDSM
			// 
			this.lblDSM.AutoSize = true;
			this.lblDSM.Location = new System.Drawing.Point(13, 170);
			this.lblDSM.Name = "lblDSM";
			this.lblDSM.Size = new System.Drawing.Size(69, 13);
			this.lblDSM.TabIndex = 7;
			this.lblDSM.Text = "DSM Version";
			// 
			// chkShowInterface
			// 
			this.chkShowInterface.AutoSize = true;
			this.chkShowInterface.Location = new System.Drawing.Point(13, 115);
			this.chkShowInterface.Name = "chkShowInterface";
			this.chkShowInterface.Size = new System.Drawing.Size(192, 17);
			this.chkShowInterface.TabIndex = 6;
			this.chkShowInterface.Text = "Показывать интерфейс сканера";
			this.chkShowInterface.UseVisualStyleBackColor = true;
			// 
			// chkDeleteFiles
			// 
			this.chkDeleteFiles.AutoSize = true;
			this.chkDeleteFiles.Location = new System.Drawing.Point(13, 91);
			this.chkDeleteFiles.Name = "chkDeleteFiles";
			this.chkDeleteFiles.Size = new System.Drawing.Size(200, 17);
			this.chkDeleteFiles.TabIndex = 5;
			this.chkDeleteFiles.Text = "Удалять отсканированные файлы";
			this.chkDeleteFiles.UseVisualStyleBackColor = true;
			// 
			// btnSelectDir
			// 
			this.btnSelectDir.Image = global::ScanMFC.Properties.Resources.folder;
			this.btnSelectDir.Location = new System.Drawing.Point(240, 58);
			this.btnSelectDir.Name = "btnSelectDir";
			this.btnSelectDir.Size = new System.Drawing.Size(30, 30);
			this.btnSelectDir.TabIndex = 4;
			this.btnSelectDir.UseVisualStyleBackColor = true;
			this.btnSelectDir.Click += new System.EventHandler(this.btnSelectDir_Click);
			// 
			// txtFileDir
			// 
			this.txtFileDir.Location = new System.Drawing.Point(13, 64);
			this.txtFileDir.Name = "txtFileDir";
			this.txtFileDir.Size = new System.Drawing.Size(226, 20);
			this.txtFileDir.TabIndex = 3;
			// 
			// txtFileName
			// 
			this.txtFileName.Location = new System.Drawing.Point(76, 20);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(192, 20);
			this.txtFileName.TabIndex = 2;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(10, 47);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(51, 13);
			this.label6.TabIndex = 1;
			this.label6.Text = "Каталог:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 23);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(67, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Имя файла:";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.listView1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.button1, 0, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 175F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(799, 548);
			this.tableLayoutPanel1.TabIndex = 19;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.Controls.Add(this.btnAdd);
			this.flowLayoutPanel1.Controls.Add(this.btnDelete);
			this.flowLayoutPanel1.Controls.Add(this.btnMoveLeft);
			this.flowLayoutPanel1.Controls.Add(this.btnMoveRight);
			this.flowLayoutPanel1.Controls.Add(this.btnPDF);
			this.flowLayoutPanel1.Controls.Add(this.btnTIFF);
			this.flowLayoutPanel1.Controls.Add(this.radioiTextSharp);
			this.flowLayoutPanel1.Controls.Add(this.radioImageMagick);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(283, 3);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(513, 56);
			this.flowLayoutPanel1.TabIndex = 20;
			// 
			// btnAdd
			// 
			this.btnAdd.Image = global::ScanMFC.Properties.Resources.sign_add;
			this.btnAdd.Location = new System.Drawing.Point(2, 2);
			this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(30, 30);
			this.btnAdd.TabIndex = 17;
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Image = global::ScanMFC.Properties.Resources.sign_error;
			this.btnDelete.Location = new System.Drawing.Point(36, 2);
			this.btnDelete.Margin = new System.Windows.Forms.Padding(2);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(30, 30);
			this.btnDelete.TabIndex = 13;
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnMoveLeft
			// 
			this.btnMoveLeft.Image = global::ScanMFC.Properties.Resources.sign_left;
			this.btnMoveLeft.Location = new System.Drawing.Point(70, 2);
			this.btnMoveLeft.Margin = new System.Windows.Forms.Padding(2);
			this.btnMoveLeft.Name = "btnMoveLeft";
			this.btnMoveLeft.Size = new System.Drawing.Size(30, 30);
			this.btnMoveLeft.TabIndex = 18;
			this.btnMoveLeft.UseVisualStyleBackColor = true;
			this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
			// 
			// btnMoveRight
			// 
			this.btnMoveRight.Image = global::ScanMFC.Properties.Resources.sign_right;
			this.btnMoveRight.Location = new System.Drawing.Point(104, 2);
			this.btnMoveRight.Margin = new System.Windows.Forms.Padding(2);
			this.btnMoveRight.Name = "btnMoveRight";
			this.btnMoveRight.Size = new System.Drawing.Size(30, 30);
			this.btnMoveRight.TabIndex = 19;
			this.btnMoveRight.UseVisualStyleBackColor = true;
			this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
			// 
			// btnPDF
			// 
			this.btnPDF.Image = global::ScanMFC.Properties.Resources.file_pdf;
			this.btnPDF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnPDF.Location = new System.Drawing.Point(138, 2);
			this.btnPDF.Margin = new System.Windows.Forms.Padding(2);
			this.btnPDF.Name = "btnPDF";
			this.btnPDF.Size = new System.Drawing.Size(60, 30);
			this.btnPDF.TabIndex = 14;
			this.btnPDF.Text = "PDF";
			this.btnPDF.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnPDF.UseVisualStyleBackColor = true;
			this.btnPDF.Click += new System.EventHandler(this.btnPDF_Click);
			// 
			// btnTIFF
			// 
			this.btnTIFF.Image = global::ScanMFC.Properties.Resources.file_picture;
			this.btnTIFF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnTIFF.Location = new System.Drawing.Point(202, 2);
			this.btnTIFF.Margin = new System.Windows.Forms.Padding(2);
			this.btnTIFF.Name = "btnTIFF";
			this.btnTIFF.Size = new System.Drawing.Size(65, 30);
			this.btnTIFF.TabIndex = 22;
			this.btnTIFF.Text = "TIFF";
			this.btnTIFF.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnTIFF.UseVisualStyleBackColor = true;
			this.btnTIFF.Click += new System.EventHandler(this.btnTIFF_Click);
			// 
			// radioiTextSharp
			// 
			this.radioiTextSharp.AutoSize = true;
			this.radioiTextSharp.Location = new System.Drawing.Point(272, 3);
			this.radioiTextSharp.Name = "radioiTextSharp";
			this.radioiTextSharp.Size = new System.Drawing.Size(76, 17);
			this.radioiTextSharp.TabIndex = 20;
			this.radioiTextSharp.TabStop = true;
			this.radioiTextSharp.Text = "iTextSharp";
			this.radioiTextSharp.UseVisualStyleBackColor = true;
			// 
			// radioImageMagick
			// 
			this.radioImageMagick.AutoSize = true;
			this.radioImageMagick.Location = new System.Drawing.Point(354, 3);
			this.radioImageMagick.Name = "radioImageMagick";
			this.radioImageMagick.Size = new System.Drawing.Size(89, 17);
			this.radioImageMagick.TabIndex = 21;
			this.radioImageMagick.TabStop = true;
			this.radioImageMagick.Text = "ImageMagick";
			this.radioImageMagick.UseVisualStyleBackColor = true;
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
			this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(799, 570);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.statusStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "ScanMFC";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.trkJpegQuality)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkContrast)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trkBrightness)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numDeskew)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox chkFeeder;
		private System.Windows.Forms.CheckBox chkDuplex;
		private System.Windows.Forms.ComboBox cmbColor;
		private System.Windows.Forms.ComboBox cmbResolution;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.TrackBar trkJpegQuality;
		private System.Windows.Forms.Label lblJpegQuality;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFilesCount;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnPDF;
		private System.Windows.Forms.SaveFileDialog savePDFDialog;
		private System.Windows.Forms.ComboBox cmbScanners;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnSelectDir;
		private System.Windows.Forms.TextBox txtFileDir;
		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.FolderBrowserDialog dlgSelectDir;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblBrightness;
		private System.Windows.Forms.TrackBar trkBrightness;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label lblContrast;
		private System.Windows.Forms.TrackBar trkContrast;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.OpenFileDialog dlgAddFiles;
		private System.Windows.Forms.ToolTip toolTipForm1;
		private System.Windows.Forms.Button btnMoveLeft;
		private System.Windows.Forms.Button btnMoveRight;
		private System.Windows.Forms.RadioButton radioiTextSharp;
		private System.Windows.Forms.RadioButton radioImageMagick;
		private System.Windows.Forms.Button btnTIFF;
		private System.Windows.Forms.CheckBox chkDeleteFiles;
		private System.Windows.Forms.CheckBox chkShowInterface;
		private System.Windows.Forms.ToolStripStatusLabel toolStripDSM;
		private System.Windows.Forms.ComboBox cmbDSM;
		private System.Windows.Forms.Label lblDSM;
		private System.Windows.Forms.Label lblDeskew;
		private System.Windows.Forms.NumericUpDown numDeskew;
		private System.Windows.Forms.ToolStripStatusLabel toolStripMsg;
		private System.Windows.Forms.Button btnDeskew;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
	}
}

