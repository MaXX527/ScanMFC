using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using FreeImageAPI;
using ImageMagick;
using System.IO;

namespace ScanMFC
{
	public partial class FormEdit : Form
	{
		Image image;
		Rectangle rect; // Выделение области рисунка
		Rectangle rectImage; // Размер рисунка
		Point point1, point2;
		public bool bImageChanged; // Признак изменения рисунка
		public bool bOKPressed; // Форма закрывается кнопкой OK, не надо лишних вопросов задавать
		public FormEdit()
		{
			InitializeComponent();
		}

		private void FormEdit_Load(object sender, EventArgs e)
		{
			bOKPressed = false;

			// Размер формы менять нельзя, иначе все настройки выделения собьются
			//Debug.WriteLine(SystemInformation.VirtualScreen.Height.ToString());
			int formHeight = (int)(0.8 * SystemInformation.VirtualScreen.Height);
			int formWidth = (int)(0.7 * formHeight);
			Size = new Size(formWidth, formHeight);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			// Начальное положение формы берем из настроек
			Location = new Point(Properties.Settings.Default.FormEdit_Location_X, Properties.Settings.Default.FormEdit_Location_Y);

			image = Image.FromFile(Form1.filePath);
			pictureBox1.Image = image;
			bImageChanged = false;

			rectImage = new Rectangle();
			float ratioW = (float)pictureBox1.ClientSize.Width / (float)pictureBox1.Image.Size.Width;
			float ratioH = (float)pictureBox1.ClientSize.Height / (float)pictureBox1.Image.Size.Height;

			// Элемент управления шире рисунка, высота одинакова
			if (ratioW > ratioH)
			{
				rectImage.X = (int)((pictureBox1.ClientSize.Width - pictureBox1.Image.Size.Width * ratioH) / 2);
				rectImage.Height = pictureBox1.ClientSize.Height;
				rectImage.Width = (int)((pictureBox1.ClientSize.Width - rectImage.X * 2));
				rectImage.Y = 0;
			}
			// Элемент управления выше рисунка, ширина одинакова
			else
			{
				rectImage.Y = (int)((pictureBox1.ClientSize.Height - pictureBox1.Image.Size.Height * ratioW) / 2);
				rectImage.Width = pictureBox1.ClientSize.Width;
				rectImage.Height = (int)((pictureBox1.ClientSize.Height - rectImage.Y * 2));
				rectImage.X = 0;
			}

			//Debug.WriteLine(String.Format("rectImage X={0} Y={1} Width={2} Height={3}", rectImage.X, rectImage.Y, rectImage.Width, rectImage.Height));
			//Debug.WriteLine(String.Format("pictureBox1.ClientRectangle X={0} Y={1} Width={2} Height={3}", pictureBox1.ClientRectangle.X, pictureBox1.ClientRectangle.Y, pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height));

			// Подсказки для элементов управления
			toolTipFormEdit.SetToolTip(this.btnCrop, "Обрезать");
			toolTipFormEdit.SetToolTip(this.btnLeft, "Повернуть налево");
			toolTipFormEdit.SetToolTip(this.btnRight, "Повернуть направо");
			toolTipFormEdit.SetToolTip(this.btnAround, "Перевернуть");
		}

		private void FormEdit_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (bImageChanged && !bOKPressed)
				if (DialogResult.No == MessageBox.Show("Сохранить изменения?", "Редактор", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
					bImageChanged = false;

			image.Dispose();
			pictureBox1.Image.Dispose();

			Properties.Settings.Default.FormEdit_Location_X = Location.X;
			Properties.Settings.Default.FormEdit_Location_Y = Location.Y;
			Properties.Settings.Default.Save();
		}

		private void btnCrop_Click(object sender, EventArgs e)
		{
			/*image.Dispose();
			pictureBox1.Image.Dispose();
			
			FIBITMAP dib = FreeImage.Copy(FreeImage.LoadEx(Form1.filePath), rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
			FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JPEG, dib, Form1.filePath, FREE_IMAGE_SAVE_FLAGS.DEFAULT);
			FreeImage.UnloadEx(ref dib);

			image = Image.FromFile(Form1.filePath);
			pictureBox1.Image = image;*/

			float ratio = (float)image.Width / (float)rectImage.Width;

			//Debug.WriteLine((image.Width / rectImage.Width).ToString() + "   " + ratio.ToString());
			Rectangle crop = new Rectangle((int)(ratio * (rect.X - rectImage.X)), (int)(ratio * (rect.Y - rectImage.Y)), (int)(ratio * rect.Width), (int)(ratio * rect.Height));
			//Debug.WriteLine(String.Format("crop X={0} Y={1} Width={2} Height={3}", crop.X, crop.Y, crop.Width, crop.Height));

			Bitmap bmp = new Bitmap(image);
			MagickImage magickImage = new MagickImage(bmp);
			bmp.Dispose();
			
			magickImage.Crop(new MagickGeometry(crop));
			magickImage.Format = MagickFormat.Jpeg;
			magickImage.Density = new Density(image.HorizontalResolution, image.VerticalResolution);
			magickImage.Quality = Form1.jpegQ;
			/*switch (image)
			{
				case ColorPalette. 0:
					magickImage.Depth = 1;
					break;
				case 1:
					magickImage.Depth = 8;
					break;
				case 2:
					magickImage.Depth = 24;
					break;
			}*/
			image.Dispose();
			//File.Delete(Form1.filePath);
			magickImage.Write(Form1.filePath);
			magickImage.Dispose();

/*			Bitmap original = new Bitmap(image);
			original.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			Bitmap bmp = original.Clone(crop, image.PixelFormat);

			Bitmap newBmp = new Bitmap(bmp);
			bmp.Dispose();
			original.Dispose();
			newBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			pictureBox1.Image.Dispose();
			image.Dispose();

			newBmp.Save(Form1.filePath, ImageFormat.Jpeg);
			
			newBmp.Dispose();
*/
			// Загружаем отредактированный файл заново
			image = Image.FromFile(Form1.filePath);
			pictureBox1.Image = image;
			//pictureBox1.Refresh();
			// Обнулить точки выбора чтобы исчез прямоугольник
			point1.X = point1.Y = point2.X = point2.Y = 0;

			bImageChanged = true;
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.DarkBlue, 2.0f);
			myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

			rect = new Rectangle( (point1.X < point2.X ? point1.X : point2.X), (point1.Y < point2.Y ? point1.Y : point2.Y), Math.Abs(point1.X - point2.X), Math.Abs(point1.Y - point2.Y));
			//rect = new Rectangle(point1.X, point1.Y, point2.X - point1.X, point2.Y - point1.Y);

			//Debug.WriteLine(String.Format("{0} {1} {2} {3}", point1.X, point1.Y, point2.X, point2.Y));

			e.Graphics.DrawRectangle(myPen, rect);
			myPen.Dispose();
			//Debug.WriteLine(String.Format("{0} {1} {2} {3}", rect.X, rect.Y, rect.Width, rect.Height));
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				//Debug.WriteLine(String.Format("{0} {1}", Cursor.Position.X, Cursor.Position.Y));
				point1 = new Point()
				{
					X = e.X,
					Y = e.Y
				};
			}
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				//Debug.WriteLine(String.Format("{0} {1}", Cursor.Position.X, Cursor.Position.Y));
				point2 = new Point()
				{
					X = e.X,
					Y = e.Y
				};
				pictureBox1.Refresh();
			}
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				point2 = new Point()
				{
					X = e.X,
					Y = e.Y
				};
				pictureBox1.Refresh();
			}
		}

		private void btnLeft_Click(object sender, EventArgs e)
		{
			image.RotateFlip(RotateFlipType.Rotate270FlipNone);
			pictureBox1.Image = image;
			bImageChanged = true;
			image.Save(Form1.filePath);
		}

		private void btnRight_Click(object sender, EventArgs e)
		{
			image.RotateFlip(RotateFlipType.Rotate90FlipNone);
			pictureBox1.Image = image;
			bImageChanged = true;
			image.Save(Form1.filePath);
		}

		private void btnAround_Click(object sender, EventArgs e)
		{
			image.RotateFlip(RotateFlipType.Rotate180FlipNone);
			pictureBox1.Image = image;
			bImageChanged = true;
			image.Save(Form1.filePath);
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			bOKPressed = true;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			bImageChanged = false; // Нажали кнопку Отмена - не сохранять изменения
			Close();
		}

		private void pictureBox1_MouseHover(object sender, EventArgs e)
		{
			//Point point = pictureBox1.PointToClient(Cursor.Position);
			
			// Курсор внутри области выделения - перетаскивание
			/*if (rect.Contains(point))
				Cursor.Current = Cursors.SizeAll;
			else
				Cursor.Current = Cursors.Default;*/
		}
	}
}
