using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WIA;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using iTextSharp;
using ImageMagick;
using FreeImageAPI;

namespace ScanMFC
{
	public partial class Form1 : Form
	{
		private string filePathFront;
		private string filePathBack;
		const float A4width = 8.27f;
		const float A4height = 11.69f;

		List<string> scanFileNames;

		ImageList imageList;

		static int count = 0;
		static int fileSuffix = 0;

		public static string filePath = "";
		public static int jpegQ;
		string PDFInitialDir = "";

		DeviceManager deviceManager;

		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			/*DeviceManager deviceManager = new DeviceManagerClass();
			for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
			{
				if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType) continue;
				textBox1.AppendText(String.Format("{0} = {1}", i, deviceManager.DeviceInfos[i].Properties["Name"].get_Value()));
			}

			DeviceInfo deviceInfo = deviceManager.DeviceInfos[1];*/
			DeviceInfo deviceInfo = deviceManager.DeviceInfos[cmbScanners.SelectedIndex + 1];
			Device device = deviceInfo.Connect();
			Item scanner = device.Items[1];

			int resolution = int.Parse(cmbResolution.SelectedItem.ToString());
			int width_pixel = (int)(A4width * resolution);
			int height_pixel = (int)(A4height * resolution);


			//#define WIA_IPA_DATATYPE                                     4103 // 0x1007
			//#define WIA_IPA_DATATYPE_STR                                 L"Data Type"
			//#define WIA_DATA_THRESHOLD             0
			//#define WIA_DATA_DITHER                1
			//#define WIA_DATA_GRAYSCALE             2
			//#define WIA_DATA_COLOR                 3
			//#define WIA_DATA_COLOR_THRESHOLD       4
			//#define WIA_DATA_COLOR_DITHER          5

			//
			// WIA_IPS_CUR_INTENT flags
			// WIA_IPS_CUR_INTENT
			//#define WIA_INTENT_NONE                   0x00000000
			//#define WIA_INTENT_IMAGE_TYPE_COLOR       0x00000001
			//#define WIA_INTENT_IMAGE_TYPE_GRAYSCALE   0x00000002
			//#define WIA_INTENT_IMAGE_TYPE_TEXT        0x00000004
			//#define WIA_INTENT_IMAGE_TYPE_MASK        0x0000000F
			//#define WIA_INTENT_MINIMIZE_SIZE          0x00010000
			//#define WIA_INTENT_MAXIMIZE_QUALITY       0x00020000
			//#define WIA_INTENT_BEST_PREVIEW           0x00040000
			//#define WIA_INTENT_SIZE_MASK              0x000F0000

			//#define WIA_IPA_DEPTH                                        4104 // 0x1008
			//#define WIA_IPA_DEPTH_STR                                    L"Bits Per Pixel"

			int WIA_IPS_CUR_INTENT = 0; // WIA_INTENT_NONE
			int WIA_IPA_DATATYPE = 0; // WIA_DATA_THRESHOLD
			int WIA_IPA_DEPTH = 0;

			switch (cmbColor.SelectedIndex)
			{
				case 0: // ч-б
					WIA_IPA_DATATYPE = 0; // WIA_DATA_THRESHOLD
					WIA_IPS_CUR_INTENT = 4; // WIA_INTENT_IMAGE_TYPE_TEXT
					WIA_IPA_DEPTH = 1;
					break;
				case 1: // серый
					WIA_IPA_DATATYPE = 2; // WIA_DATA_GRAYSCALE
					WIA_IPS_CUR_INTENT = 2; // WIA_INTENT_IMAGE_TYPE_GRAYSCALE
					WIA_IPA_DEPTH = 8;
					break;
				case 2: // цвет
					WIA_IPA_DATATYPE = 3; // WIA_DATA_COLOR
					WIA_IPS_CUR_INTENT = 1; // WIA_INTENT_IMAGE_TYPE_COLOR
					WIA_IPA_DEPTH = 24;
					break;
			}

			//Guid WiaImgFmt_JPEG = new Guid(0xb96b3cae, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
			//scanner.Properties["4106"].set_Value(WiaImgFmt_JPEG);

			//#define WIA_IPA_COMPRESSION                                  4107 // 0x100b
			//#define WIA_IPA_COMPRESSION_STR                              L"Compression"
			//
			// WIA_IPA_COMPRESSION constants
			//
			//#define WIA_COMPRESSION_NONE           0
			//#define WIA_COMPRESSION_BI_RLE4        1
			//#define WIA_COMPRESSION_BI_RLE8        2
			//#define WIA_COMPRESSION_G3             3
			//#define WIA_COMPRESSION_G4             4
			//#define WIA_COMPRESSION_JPEG           5

			/*for (int i = 0; i <= 5; i++)
			{
				System.Diagnostics.Debug.WriteLine(i);
				try
				{
					scanner.Properties["4107"].set_Value(i);
				}
				catch (ArgumentException comerr)
				{
					System.Diagnostics.Debug.WriteLine(comerr.Message);
				}
			}*/

			//
			// WIA_DPS_DOCUMENT_HANDLING_SELECT flags
			//
			//#define FEEDER                        0x001
			//#define FLATBED                       0x002
			//#define DUPLEX                        0x004
			//#define FRONT_FIRST                   0x008
			//#define BACK_FIRST                    0x010
			//#define FRONT_ONLY                    0x020
			//#define BACK_ONLY                     0x040
			//#define NEXT_PAGE                     0x080
			//#define PREFEED                       0x100
			//#define AUTO_ADVANCE                  0x200

			//device.Properties["Document Handling Select"].set_Value(5); // 3088
			int duplex_mode = 2; // Планшет

			//device.Properties["3096"].set_Value(1); // WIA_IPS_PAGES Планшет - 1 страница

			if (chkFeeder.Checked)
			{
				//device.Properties["3096"].set_Value(0); // Много страниц
				if (chkDuplex.Checked)
					duplex_mode = 5; // Автоподатчик обе стороны
				else
					duplex_mode = 1; // Автоподатчик одна сторона
			}

			device.Properties["Document Handling Select"].set_Value(duplex_mode);

			// Яркость WIA_IPS_BRIGHTNESS 6154
			scanner.Properties["6154"].set_Value(trkBrightness.Value);
			// Контраст WIA_IPS_CONTRAST 6155
			scanner.Properties["6155"].set_Value(trkContrast.Value);

			scanner.Properties["4103"].set_Value(WIA_IPA_DATATYPE);
			scanner.Properties["4104"].set_Value(WIA_IPA_DEPTH);
			scanner.Properties["6146"].set_Value(WIA_IPS_CUR_INTENT);

			scanner.Properties["6147"].set_Value(resolution);
			scanner.Properties["6148"].set_Value(resolution);

			scanner.Properties["6151"].set_Value(width_pixel);
			scanner.Properties["6152"].set_Value(height_pixel);

			//Type type = scanner.Properties["4107"].GetType;
			//System.Diagnostics.Debug.WriteLine("4107: " + type.);

			/*foreach (Property propertyItem in scanner.Properties)
			{
				if (!propertyItem.IsReadOnly)
				{
					System.Diagnostics.Debug.WriteLine(String.Format("{0}\t{1}\t{2}", propertyItem.Name, propertyItem.PropertyID, propertyItem.get_Value()));
				}
			}*/

			CommonDialogClass commonDialog = new CommonDialogClass();

			WIA.ImageProcess imageProcess = new WIA.ImageProcess();  // use to compress jpeg.
			imageProcess.Filters.Add(imageProcess.FilterInfos["Convert"].FilterID);
			imageProcess.Filters[1].Properties["FormatID"].set_Value(FormatID.wiaFormatJPEG);
			imageProcess.Filters[1].Properties["Quality"].set_Value(trkJpegQuality.Value * 10);

			ImageFile imageFileFront;
			ImageFile imageFileBack;

			try
			{
				while (true)
				{
					imageFileFront = (ImageFile)commonDialog.ShowTransfer(scanner, FormatID.wiaFormatJPEG, true);
					//imageFileFront = (ImageFile)scanner.Transfer(FormatID.wiaFormatJPEG);
					filePathFront = GetFileName(); // Path.GetTempFileName() + ".jpg";
					imageProcess.Apply(imageFileFront).SaveFile(filePathFront);
					//imageFileFront.SaveFile(filePathFront);
					Marshal.ReleaseComObject(imageFileFront);
					/*thumb = Image.FromFile(filePathFront).GetThumbnailImage(105, 149, () => false, IntPtr.Zero);
					
					imageList.Images.Add(thumb);
					listView1.Items.Add(Path.GetFileName(filePathFront));
					listView1.Items[count].ImageIndex = count++;
					
					scanFileNames.Add(filePathFront);*/
					AppendFile(filePathFront);

					if (!chkFeeder.Checked) // Планшет - сканируем только одну страницу
						break;

					if (chkDuplex.Checked)
					{
						imageFileBack = (ImageFile)commonDialog.ShowTransfer(scanner, FormatID.wiaFormatJPEG, true);
						//imageFileBack = (ImageFile)scanner.Transfer(FormatID.wiaFormatJPEG);
						filePathBack = GetFileName(); // Path.GetTempFileName() + ".jpg";
						imageProcess.Apply(imageFileBack).SaveFile(filePathBack);
						//imageFileBack.SaveFile(filePathBack);
						Marshal.ReleaseComObject(imageFileBack);
						/*thumb = Image.FromFile(filePathBack).GetThumbnailImage(105, 149, () => false, IntPtr.Zero);

						imageList.Images.Add(thumb);
						listView1.Items.Add(Path.GetFileName(filePathBack));
						listView1.Items[count].ImageIndex = count++;

						scanFileNames.Add(filePathBack);*/
						AppendFile(filePathBack);
					}
				}
			}
			catch (COMException comerr)
			{
				switch ((uint)comerr.ErrorCode)
				{
					case 0x80210003:
						toolStripStatus.Text = "Страницы закончились";
						break;
					case 0x80210006:
						toolStripStatus.Text = "Сканер занят или не готов";
						break;
					case 0x80210064:
						toolStripStatus.Text = "Сканирование отменено";
						break;
					case 0x8021000C:
						toolStripStatus.Text = "Некорректные настройки устройства WIA";
						break;
					case 0x80210005:
						toolStripStatus.Text = "Устройство выключено";
						break;
					case 0x80210001:
						toolStripStatus.Text = "Неведомая ошибка";
						break;
				}
			}
			finally
			{
				//imageFileFront = null;
				//imageFileBack = null;
				//thumb = null;
			}

		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			//pictureBox1.Image = null;

			imageList.Dispose();

			// Удаление всех файлов при выходе
			//foreach (string file in scanFileNames)
			//	File.Delete(file);

			SaveConfig();
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			scanFileNames = new List<string>();

			imageList = new ImageList();
			imageList.ImageSize = new Size(71, 100);

			listView1.LargeImageList = imageList;

			deviceManager = new DeviceManagerClass();
			for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
			{
				if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType) continue;

				cmbScanners.Items.Add(deviceManager.DeviceInfos[i].Properties["Name"].get_Value());
			}

			if(deviceManager.DeviceInfos.Count < 1) // Сканеров не найдено
			{
				cmbScanners.Items.Add("Сканеры не найдены");
				button1.Enabled = false; // Отключить кнопку "Сканировать"
			}

			LoadConfig();

			// Подсказки для элементов управления
			toolTipForm1.SetToolTip(this.btnAdd, "Добавить файлы изображений");
			toolTipForm1.SetToolTip(this.btnDelete, "Удалить выбранные файлы");
			toolTipForm1.SetToolTip(this.btnMoveLeft, "Переместить скан влево");
			toolTipForm1.SetToolTip(this.btnMoveRight, "Переместить скан вправо");
			toolTipForm1.SetToolTip(this.btnPDF, "Сохранить выбранные файлы в PDF");
			toolTipForm1.SetToolTip(this.btnTIFF, "Сохранить выбранные файлы в TIFF");
		}

		private void trkJpegQuality_ValueChanged(object sender, EventArgs e)
		{
			lblJpegQuality.Text = (10 * trkJpegQuality.Value).ToString();
		}

		private void trkBrightness_ValueChanged(object sender, EventArgs e)
		{
			lblBrightness.Text = trkBrightness.Value.ToString();
		}

		private void trkContrast_ValueChanged(object sender, EventArgs e)
		{
			lblContrast.Text = trkContrast.Value.ToString();
		}

		private void listView1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}

		private void listView1_DragDrop(object sender, DragEventArgs e)
		{
			AddFilesToListView((string[])e.Data.GetData(DataFormats.FileDrop));
		}

		private void AppendFile(string file)
		{
			// Image thumb = Image.FromFile(file); //.GetThumbnailImage(71, 100, () => false, IntPtr.Zero);
			Bitmap thumb = GetThumbnailImage( file );

			imageList.Images.Add(thumb);
			listView1.Items.Add(Path.GetFileName(file));
			listView1.Items[count].ImageIndex = count++;
			toolStripStatusLabelFilesCount.Text = String.Format("Файлов: {0}", count);

			thumb.Dispose();

			scanFileNames.Add(file);
		}

		/// <summary>
		///  Создает маленький рисунок для помещения в ListView
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		private Bitmap GetThumbnailImage(string filePath /*Image img*/)
		{
			Image img = Image.FromFile(filePath);
			int imgWidth = 71;
			int imgHeight = 100;
			int yOffset = 0;
			int xOffset = 0;

			Bitmap bmp = new Bitmap(imgWidth, imgHeight);
			Graphics g = Graphics.FromImage(bmp);
			g.Clear(Color.White);

			float ratio = (float)img.Width / (float)img.Height;
			if (ratio > 0.71f) // Рисунок слишком широкий
			{
				imgWidth = 71;
				imgHeight = (int) (imgWidth / ratio);
				yOffset = (int) ((100 - imgHeight) / 2);
			}
			else // Рисунок слишком узкий
			{
				imgHeight = 100;
				imgWidth = (int) (imgHeight* ratio);
				xOffset = (int) ((71 - imgWidth) / 2);
			}

			g.DrawImage(img, xOffset, yOffset, imgWidth, imgHeight);

			//imageList.Images[idx] = bmp;
			//bmp.Dispose();
			img.Dispose();
			return bmp;
		}

		// Открывает рисунок для редактирования
		private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (1 == listView1.SelectedItems.Count)
			{
				int idx = listView1.SelectedItems[0].ImageIndex;
				// Коируем картинку во временный файл, вдруг пользователь отменит редактирование
				filePath = Path.GetTempPath() + Path.GetFileName(scanFileNames[idx]);
				File.Copy(scanFileNames[idx], filePath, true);
			
				FormEdit formEdit = new FormEdit()
				{
					Visible = false
				};
				jpegQ = trkJpegQuality.Value * 10;
				formEdit.ShowDialog();

				if (formEdit.bImageChanged) // Файл изменился, загрузить новую миниатюру
				{
					// Перезаписываем файл отредактированным
					File.Copy(filePath, scanFileNames[idx], true);
					//filePath = scanFileNames[idx];

					imageList.Images[idx] = GetThumbnailImage(scanFileNames[idx]);
					listView1.Refresh();
				}

				// Временный файл удалить в любом случае
				File.Delete(filePath);

				formEdit.Dispose();

				/*FormImageEditor formEdit = new FormImageEditor()
				{
					Visible = false
				};
				formEdit.ShowDialog();
				formEdit.Dispose();*/
			}
		}

		// Удаление выделенных файлов
		private void btnDelete_Click(object sender, EventArgs e)
		{
			//foreach(ListViewItem item in listView1.SelectedItems)
			for (int idx = listView1.Items.Count - 1; idx >= 0;  idx--)
			{
				if (listView1.Items[idx].Selected)
				{
					string fileToDelete = scanFileNames[idx];
					scanFileNames.RemoveAt(idx);
					imageList.Images.RemoveAt(idx);
					listView1.Items.RemoveAt(idx);

					count--;

					if(chkDeleteFiles.Checked) File.Delete(fileToDelete);

					toolStripStatusLabelFilesCount.Text = String.Format("Файлов: {0}", count);
				}
			}

			// Index меняется автоматом, а ImageIndex нужно установить принудительно
			for (int idx = 0; idx < listView1.Items.Count; idx++)
				listView1.Items[idx].ImageIndex = idx;
		}

		// Сохраняет выделенные файлы в PDF
		private void btnPDF_Click(object sender, EventArgs e)
		{
			if (0 == listView1.SelectedItems.Count) return;

			bool deleteFiles = chkDeleteFiles.Checked; // Удалять jpeg'и после создания pdf

			savePDFDialog.Filter = "Файлы PDF|*.pdf";
			savePDFDialog.InitialDirectory = PDFInitialDir;
			if (DialogResult.OK != savePDFDialog.ShowDialog()) return;

			Cursor.Current = Cursors.WaitCursor;

			int s = 0;

			if (radioImageMagick.Checked)
			{
				MagickImageCollection images = new MagickImageCollection();
				foreach (ListViewItem item in listView1.SelectedItems)
				{
					images.Add(scanFileNames[item.Index]);
					images[s++].Strip(); // Иначе PDF может получиться кривой с ошибкой insufficient data for an image
				}
				try
				{
					images.Write(savePDFDialog.FileName, MagickFormat.Pdf);
				}
				catch(MagickException me)
				{
					toolStripStatus.Text = me.Message;
					deleteFiles = false;
				}
				images.Dispose();
			}

			if (radioiTextSharp.Checked)
			{
				iTextSharp.text.Document document = new iTextSharp.text.Document();
				FileStream fileStream = new FileStream(savePDFDialog.FileName, FileMode.Create);
				iTextSharp.text.pdf.PdfWriter.GetInstance(document, fileStream);
				document.Open();
				foreach (ListViewItem item in listView1.SelectedItems)
				{
					iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(scanFileNames[item.Index]);
					image.SetAbsolutePosition(0.0f, 0.0f);
					image.ScaleToFit(image.Width / image.DpiX * 72f, image.Height / image.DpiY * 72f);
					iTextSharp.text.Chunk chunk = new iTextSharp.text.Chunk();
					chunk.SetNewPage();
					if(s > 0) document.Add(chunk);
					if(deleteFiles & !document.Add(image)) deleteFiles = false;
					chunk = null;
					image = null;
					s++;
				}

				document.Close();
				document.Dispose();
				fileStream.Close();
				fileStream.Dispose();
			}

			if (deleteFiles) btnDelete_Click(btnPDF, null);

			Cursor.Current = Cursors.Default;
		}

		// Сохраняет выделенные файлы в TIFF
		private void btnTIFF_Click(object sender, EventArgs e)
		{
			if (0 == listView1.SelectedItems.Count) return;

			bool deleteFiles = chkDeleteFiles.Checked; // Удалять jpeg'и после создания tiff

			savePDFDialog.Filter = "Файлы TIFF|*.tif;*.tiff";
			savePDFDialog.InitialDirectory = PDFInitialDir;
			if (DialogResult.OK != savePDFDialog.ShowDialog()) return;

			Cursor.Current = Cursors.WaitCursor;
			
			// Вариант с FreeImage - быстро, но сложнее и иногда инвертирует цвета

			// Сохранить первую страницу
			FIBITMAP dib_color = FreeImage.LoadEx(scanFileNames[listView1.SelectedItems[0].Index]);

			FIBITMAP dib = FreeImage.ConvertToGreyscale(dib_color);
			FreeImage.UnloadEx(ref dib_color);
			if (FREE_IMAGE_COLOR_TYPE.FIC_MINISBLACK == FreeImage.GetColorType(dib))
				FreeImage.Invert(dib);

			deleteFiles = FreeImage.SaveEx(ref dib, savePDFDialog.FileName, 
				FREE_IMAGE_FORMAT.FIF_TIFF, FREE_IMAGE_SAVE_FLAGS.TIFF_CCITTFAX4, FREE_IMAGE_COLOR_DEPTH.FICD_01_BPP, true);
			FreeImage.UnloadEx(ref dib);

			if (listView1.SelectedItems.Count > 1)
			{
				string tmpFile = Path.GetTempFileName() + ".tif";
				FIBITMAP fib;
				FIBITMAP fib_color;
				FIMULTIBITMAP fmb = FreeImage.OpenMultiBitmapEx(savePDFDialog.FileName);
				// Добавить остальные страницы
				for (int i = 1; i < listView1.SelectedItems.Count; i++)
				{
					fib_color = FreeImage.LoadEx(scanFileNames[listView1.SelectedItems[i].Index]);
					fib = FreeImage.ConvertToGreyscale(fib_color);
					FreeImage.UnloadEx(ref fib_color);

					if (FREE_IMAGE_COLOR_TYPE.FIC_MINISBLACK == FreeImage.GetColorType(fib))
						FreeImage.Invert(fib);

					FreeImage.SaveEx(ref fib, tmpFile, 
						FREE_IMAGE_FORMAT.FIF_TIFF, FREE_IMAGE_SAVE_FLAGS.TIFF_CCITTFAX4, FREE_IMAGE_COLOR_DEPTH.FICD_01_BPP, true);
					fib = FreeImageAPI.FreeImage.LoadEx(tmpFile);

					FreeImage.AppendPage(fmb, fib);
					FreeImage.UnloadEx(ref fib);
					File.Delete(tmpFile);
				}
				deleteFiles = FreeImage.CloseMultiBitmapEx(ref fmb);
			}
			
			/*
			// Вариант с ImageMagick - проще, но очень медленно
			int s = 0;

			MagickImageCollection images = new MagickImageCollection();
			foreach (ListViewItem item in listView1.SelectedItems)
			{
				images.Add(scanFileNames[item.Index]);
				images[s].Strip(); // По аналогии с PDF на всякий случай
				images[s].Format = MagickFormat.Tif;
				images[s].Settings.Compression = CompressionMethod.Group4;
				images[s].Depth = 1;
				s++;
			}
			try
			{
				images.Write(savePDFDialog.FileName, MagickFormat.Tif);
			}
			catch (MagickException me)
			{
				toolStripStatus.Text = me.Message;
				deleteFiles = false;
			}
			images.Dispose();
			*/
			if (deleteFiles) btnDelete_Click(btnPDF, null);

			Cursor.Current = Cursors.Default;
		}

		private void btnSelectDir_Click(object sender, EventArgs e)
		{
			if(DialogResult.OK == dlgSelectDir.ShowDialog())
			{
				txtFileDir.Text = dlgSelectDir.SelectedPath;
			}
		}

		private string GetFileName()
		{
			string file = txtFileDir.Text + "\\" + txtFileName.Text + String.Format("_{0:000}", fileSuffix++) + ".jpg";
			while(File.Exists(file))
				file = txtFileDir.Text + "\\" + txtFileName.Text + String.Format("_{0:000}", fileSuffix++) + ".jpg";
			return file;
		}

		private void SaveConfig()
		{
			Properties.Settings.Default.Scanner = cmbScanners.SelectedIndex;
			Properties.Settings.Default.Color = cmbColor.SelectedIndex;
			Properties.Settings.Default.Resolution = cmbResolution.SelectedIndex;
			Properties.Settings.Default.JpegQuality = trkJpegQuality.Value;
			Properties.Settings.Default.FileDir = txtFileDir.Text;
			Properties.Settings.Default.FileName = txtFileName.Text;
			Properties.Settings.Default.Feeder = chkFeeder.Checked;
			Properties.Settings.Default.Duplex = chkDuplex.Checked;
			Properties.Settings.Default.Form1_Maximized = this.WindowState == FormWindowState.Maximized;
			Properties.Settings.Default.Form1_Size_Width = this.Size.Width;
			Properties.Settings.Default.Form1_Size_Height = this.Size.Height;
			Properties.Settings.Default.PDFInitialDir = PDFInitialDir;
			Properties.Settings.Default.Brightness = trkBrightness.Value;
			Properties.Settings.Default.Contrast = trkContrast.Value;
			Properties.Settings.Default.UseiTextSharp = radioiTextSharp.Checked;
			Properties.Settings.Default.DeleteFiles = chkDeleteFiles.Checked;
			Properties.Settings.Default.Save();
		}

		private void LoadConfig()
		{
			cmbScanners.SelectedIndex = Properties.Settings.Default.Scanner;
			cmbColor.SelectedIndex = Properties.Settings.Default.Color;
			cmbResolution.SelectedIndex = Properties.Settings.Default.Resolution;
			trkJpegQuality.Value = Properties.Settings.Default.JpegQuality;
			txtFileDir.Text = Properties.Settings.Default.FileDir;
			txtFileName.Text = Properties.Settings.Default.FileName;
			chkFeeder.Checked = Properties.Settings.Default.Feeder;
			chkDuplex.Checked = Properties.Settings.Default.Duplex;
			if (Properties.Settings.Default.Form1_Maximized)
				this.WindowState = FormWindowState.Maximized;
			else
			{
				this.Width = Properties.Settings.Default.Form1_Size_Width;
				this.Height = Properties.Settings.Default.Form1_Size_Height;
			}
			PDFInitialDir = Properties.Settings.Default.PDFInitialDir;
			trkBrightness.Value = Properties.Settings.Default.Brightness;
			trkContrast.Value = Properties.Settings.Default.Contrast;
			radioiTextSharp.Checked = Properties.Settings.Default.UseiTextSharp;
			radioImageMagick.Checked = !Properties.Settings.Default.UseiTextSharp;
			chkDeleteFiles.Checked = Properties.Settings.Default.DeleteFiles;
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			dlgAddFiles.Multiselect = true;
			dlgAddFiles.InitialDirectory = PDFInitialDir;
			dlgAddFiles.Filter = "Изображения (*.pdf;*.jpg;*.jpeg)|*.pdf;*.jpg;*.jpeg";
			if(DialogResult.OK == dlgAddFiles.ShowDialog())
			{
				AddFilesToListView(dlgAddFiles.FileNames);
			}
		}

		private void AddFilesToListView(string[] names)
		{
			Cursor.Current = Cursors.WaitCursor;
			foreach (string f in names)
			{
				if (".jpg" == Path.GetExtension(f) || ".jpeg" == Path.GetExtension(f))
				{
					AppendFile(f);
				}
				if (".pdf" == Path.GetExtension(f))
				{
					/*MagickReadSettings settings = new MagickReadSettings();
					settings.Density = new Density(Int32.Parse(cmbResolution.SelectedItem.ToString()), Int32.Parse(cmbResolution.SelectedItem.ToString()));
					settings.Format = MagickFormat.Pdf;

					using (MagickImageCollection images = new MagickImageCollection())
					{
						// Add all the pages of the pdf file to the collection
						images.Read(f, settings);

						int page = 1;
						foreach (MagickImage image in images)
						{
							// Write page to file that contains the page number
							image.Format = MagickFormat.Jpeg;
							image.Density = new Density(Int32.Parse(cmbResolution.SelectedItem.ToString()), Int32.Parse(cmbResolution.SelectedItem.ToString()));
							image.Quality = trkJpegQuality.Value * 10;
							string jpegFileName = String.Format(@"{0}\{1}_{2:000}.jpg", txtFileDir.Text, Path.GetFileNameWithoutExtension(f), page);
							image.Write(jpegFileName);
							image.Dispose();
							AppendFile(jpegFileName);
							page++;
						}
					}*/

					string salt = RandomString(4);
					string jpegFileName = String.Format(@"{0}\{1}_{2}%02d.jpg", txtFileDir.Text, Path.GetFileNameWithoutExtension(f), salt);
					string args = String.Format("-dBATCH -dNOPAUSE -dSAFER -sDEVICE=jpeg -dJPEGQ={0} -r{1} -dPDFFitPage -dFIXEDMEDIA -sDEFAULTPAPERSIZE=a4 -sOutputFile=\"{2}\" \"{3}\"",
						trkJpegQuality.Value * 10, int.Parse(cmbResolution.SelectedItem.ToString()), jpegFileName, f);
					if(ExecuteCommandAsync(args))
						foreach(string fileName in Directory.GetFiles(txtFileDir.Text, Path.GetFileNameWithoutExtension(f) + "_*.jpg"))
							AppendFile(fileName);
		
				}
			}
			Cursor.Current = Cursors.Default;
		}

		// Добавляет к имени pdf случайные символы на случай, если добавляются pdf с одинаковыми именами
		private static string RandomString(int length)
		{
			Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}

		// Запуск команды https://www.codeproject.com/Articles/25983/How-to-Execute-a-Command-in-C
		private void ExecuteCommandSync(object arguments)
		{
			try
			{
				string gswin32c = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Replace(@"ScanMFC.exe", @"gs9.50\bin\gswin32c.exe");
				//MessageBox.Show(gswin32c);
				System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo(gswin32c, (string)arguments)
				{
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};
				System.Diagnostics.Process proc = new System.Diagnostics.Process()
				{
					StartInfo = procStartInfo
				};
				proc.Start();
				// Get the output into a string
				string result = proc.StandardOutput.ReadToEnd();
				proc.Dispose();
			}
			catch (Exception objException)
			{
				// Log the exception
				MessageBox.Show("Ошибка запуска gswin32c.exe:\n" + objException.Message);
			}
		}

		// Запуск команды в отдельном потоке https://www.codeproject.com/Articles/25983/How-to-Execute-a-Command-in-C
		public bool ExecuteCommandAsync(string command)
		{
			bool ret = false;
			try
			{
				Thread objThread = new Thread(new ParameterizedThreadStart(ExecuteCommandSync));
				objThread.IsBackground = true;
				objThread.Priority = ThreadPriority.Normal;
				objThread.Start(command);
				objThread.Join();
				ret = true;
			}
			catch (ThreadStartException objException)
			{
				MessageBox.Show("Ошибка запуска gswin32c.exe:\n" + objException.Message);
			}
			catch (ThreadAbortException objException)
			{
				MessageBox.Show("Ошибка запуска gswin32c.exe:\n" + objException.Message);
			}
			catch (Exception objException)
			{
				MessageBox.Show("Ошибка запуска gswin32c.exe:\n" + objException.Message);
			}
			return ret;
		}

		// Сортировщик элементов ListView по индексу
		class ListViewItemComparer : System.Collections.IComparer
		{
			public ListViewItemComparer()
			{
			}
			public int Compare(object x, object y)
			{
				return ((ListViewItem)x).Index - ((ListViewItem)y).Index;
			}
		}

		// Перемещение рисунка влево
		private void btnMoveLeft_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count == 1) // Двигаем только если выделен один рисунок
			{
				SwapItemsInListView(-1);
			}
		}

		// Перемещение рисунка вправо
		private void btnMoveRight_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count == 1) // Двигаем только если выделен один рисунок
			{
				SwapItemsInListView(1);
			}
		}

		// Обмен двух рисунков
		private void SwapItemsInListView(int direction)
		{
			ListViewItem item = listView1.SelectedItems[0];
			int oldIdx = item.Index;
			int newIdx = oldIdx + direction;
			if (-1 == newIdx || listView1.Items.Count == newIdx) // Если выделен первый элемент или последний элемент - ничего не двигаем
				return;

			listView1.Items.RemoveAt(oldIdx);
			listView1.Items.Insert(newIdx, item);
			listView1.Items[newIdx].Selected = true;
			listView1.ListViewItemSorter = new ListViewItemComparer(); // Для сортировки элементов по индексу

			Image oldImage = (Image)imageList.Images[oldIdx].Clone();
			imageList.Images[oldIdx] = imageList.Images[newIdx];
			imageList.Images[newIdx] = oldImage;
			oldImage.Dispose();

			listView1.Items[oldIdx].ImageIndex = oldIdx;
			listView1.Items[newIdx].ImageIndex = newIdx;
			//foreach(ListViewItem i in listView1.Items)
			//	i.ImageIndex = i.Index;
			
			string oldFileName = scanFileNames[oldIdx];
			scanFileNames[oldIdx] = scanFileNames[newIdx];
			scanFileNames[newIdx] = oldFileName;

			//listView1.Refresh();
		}

	}
}
