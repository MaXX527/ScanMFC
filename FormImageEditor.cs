using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CraftSynth.ImageEditor;

namespace ScanMFC
{
	public partial class FormImageEditor : Form
	{
		private MainForm imageEditor1;
		public FormImageEditor()
		{
			InitializeComponent();
			this.imageEditor1.ParentForm = this;
		}

		private void FormImageEditor_Load(object sender, EventArgs e)
		{
			

		}
	}
}
