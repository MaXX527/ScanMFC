using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WIA;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using iTextSharp;
using ImageMagick;
using FreeImageAPI;

using Dynarithmic;

using System.Resources;
//using System.Globalization;

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
		IntPtr pArray; // Массив сканеров для DTWAIN
		static IntPtr SelectedSource; // Указатель на выбранный сканер

		private readonly ResourceManager stringManager = new ResourceManager("ru-RU", System.Reflection.Assembly.GetExecutingAssembly());

		int hr = 0;
		private const int MAX_FILES = 256; // Наибольшее количество файлов за одно сканирование

		//static StreamWriter w = File.AppendText(@"C:\Temp\scanmfc.log");
		static IntPtr aFileNames;     // Список имен файлов для сканирования
		static int numFileName = 0;   // Номер текущего отсканированного файла в массиве aFileNames
		static string sFileName = ""; // Имя текущего отсканированного файла в массиве aFileNames для передачи в метод AppendFile
		private static Form1 form1;   // Для вызова метода AppendFile из статического метода CaptureTwainProc
									  //private static bool bFileReady = false;    // Когда true - файл готов и надо его обработать и добавить на форму
		private static bool bStopAtError = false; // Если после сканирования status != 1000, завершить цикл обработки из-за какой-то ошибки
		private static bool bAcquireDone = false;  // Когда true - сканирование закончено и цикл обработки можно прервать
		private static int countAcquired = 0;

		private enum WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES
		{
			WIA_FEED = 0x01,
			WIA_FLAT = 0x02,
			WIA_DUP = 0x04,
			WIA_DETECT_FLAT = 0x08,
			WIA_DETECT_SCAN = 0x10,
			WIA_DETECT_FEED = 0x20,
			WIA_DETECT_DUP = 0x40,
			WIA_DETECT_FEED_AVAIL = 0x80,
			WIA_DETECT_DUP_AVAIL = 0x100
		}

		private enum WIA_DPS_DOCUMENT_HANDLING_STATUS
		{
			WIA_FEED_READY = 0x01,
			WIA_FLAT_READY = 0x02,
			WIA_DUP_READY = 0x04,
			WIA_FLAT_COVER_UP = 0x08,
			WIA_PATH_COVER_UP = 0x10,
			WIA_PAPER_JAM = 0x20
		}

		public Form1()
		{
			InitializeComponent();
			form1 = this;
		}

		private void button1_Click_OFF(object sender, EventArgs e)
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

			//device.Properties["Pages"].set_Value(1); // WIA_IPS_PAGES Планшет - 1 страница

			int FeedReady = 0;
			bool hasMorePages = true;

			if (chkFeeder.Checked)
			{
				//device.Properties["Pages"].set_Value(0); // Много страниц
				foreach (Property property in device.Properties)
					if ("Document Handling Status" == property.Name) // #define WIA_DPS_DOCUMENT_HANDLING_STATUS                     3087 // 0xc0f
						FeedReady = (int)property.get_Value();
				hasMorePages = (FeedReady & (int)WIA_DPS_DOCUMENT_HANDLING_STATUS.WIA_FEED_READY) != 0;

				if (!hasMorePages)
					toolStripStatus.Text = "Нет документов в автоподатчике";

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

			WIA.CommonDialog commonDialog = new WIA.CommonDialog();

			WIA.ImageProcess imageProcess = new WIA.ImageProcess();  // use to compress jpeg.
			imageProcess.Filters.Add(imageProcess.FilterInfos["Convert"].FilterID);
			imageProcess.Filters[1].Properties["FormatID"].set_Value(ImageFormatID.wiaFormatJPEG);
			imageProcess.Filters[1].Properties["Quality"].set_Value(trkJpegQuality.Value * 10);

			ImageFile imageFileFront;
			ImageFile imageFileBack;

			/*int scanCapabilities = 0;
			foreach (Property property in device.Properties)
				if ("Document Handling Capabilities" == property.Name)
					scanCapabilities = (int)property.get_Value();
			bool WiaFeed = (scanCapabilities & (int)WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES.WIA_FEED) != 0;
			bool WiaFlat = (scanCapabilities & (int)WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES.WIA_FLAT) != 0;
			bool WiaDup = (scanCapabilities & (int)WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES.WIA_DUP) != 0;
			bool WiaDetectFlat = (scanCapabilities & (int)WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES.WIA_DETECT_FLAT) != 0;
			bool WiaDetectScan = (scanCapabilities & (int)WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES.WIA_DETECT_SCAN) != 0;
			bool WiaDetectFeed = (scanCapabilities & (int)WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES.WIA_DETECT_FEED) != 0;
			bool WiaDetectDup = (scanCapabilities & (int)WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES.WIA_DETECT_DUP) != 0;
			bool WiaDetectFeedAvail = (scanCapabilities & (int)WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES.WIA_DETECT_FEED_AVAIL) != 0;
			bool WiaDetectDupAvail = (scanCapabilities & (int)WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES.WIA_DETECT_DUP_AVAIL) != 0;
			MessageBox.Show("WiaFeed: " + WiaFeed.ToString() + "\n" +
				            "WiaFlat: " + WiaFlat.ToString() + "\n" +
							"WiaDup: " + WiaDup.ToString() + "\n" +
							"WiaDetectFlat: " + WiaDetectFlat.ToString() + "\n" +
							"WiaDetectScan: " + WiaDetectScan.ToString() + "\n" +
							"WiaDetectFeed: " + WiaDetectFeed.ToString() + "\n" +
							"WiaDetectDup: " + WiaDetectDup.ToString() + "\n" +
							"WiaDetectFeedAvail: " + WiaDetectFeedAvail.ToString() + "\n" +
							"WiaDetectDupAvail: " + WiaDetectDupAvail.ToString());
			int scanCapacity = 0;
			foreach (Property property in device.Properties)
				if ("Document Handling Capacity" == property.Name)
					scanCapacity = (int)property.get_Value();
			MessageBox.Show("Document Handling Capacity: " + scanCapacity);*/

			//commonDialog.ShowSelectDevice();
			try
			{
				while (hasMorePages)
				{
					//scanner.Properties["Pages"].set_Value(1);

					imageFileFront = (ImageFile)commonDialog.ShowTransfer(scanner, ImageFormatID.wiaFormatJPEG, true);

					//imageFileFront = commonDialog.ShowAcquireImage(WiaDeviceType.ScannerDeviceType, WiaImageIntent.GrayscaleIntent, WiaImageBias.MinimizeSize, ImageFormatID.wiaFormatJPEG, false, true, true);

					//imageFileFront = (ImageFile)scanner.Transfer(ImageFormatID.wiaFormatJPEG);

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

					/*if (chkDuplex.Checked)
					{
						imageFileBack = (ImageFile)commonDialog.ShowTransfer(scanner, FormatID.wiaFormatJPEG, true);

						filePathBack = GetFileName(); // Path.GetTempFileName() + ".jpg";
						imageProcess.Apply(imageFileBack).SaveFile(filePathBack);

						Marshal.ReleaseComObject(imageFileBack);

						AppendFile(filePathBack);
					}*/
					foreach (Property property in device.Properties)
						if ("Document Handling Status" == property.Name) // #define WIA_DPS_DOCUMENT_HANDLING_STATUS                     3087 // 0xc0f
							FeedReady = (int)property.get_Value();
					hasMorePages = (FeedReady & (int)WIA_DPS_DOCUMENT_HANDLING_STATUS.WIA_FEED_READY) != 0;
				}
			}
			catch (COMException comerr)
			{
				switch ((uint)comerr.ErrorCode)
				{
					case 0x80210002:
						toolStripStatus.Text = "Замятие бумаги";
						break;
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
					default:
						toolStripStatus.Text = String.Format("COMException 0x{0:X}", comerr.ErrorCode);
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
			//w.Close();

			//pictureBox1.Image = null;

			imageList.Dispose();

			// Удаление всех файлов при выходе
			//foreach (string file in scanFileNames)
			//	File.Delete(file);

			SaveConfig();

			hr = TwainAPI.DTWAIN_SysDestroy();
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
			listView1.ListViewItemSorter = new ListViewItemComparer();

			deviceManager = new DeviceManager();
			/*for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
			{
				if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType) continue;

				cmbScanners.Items.Add(deviceManager.DeviceInfos[i].Properties["Name"].get_Value());
			}

			if(deviceManager.DeviceInfos.Count < 1) // Сканеров не найдено
			{
				cmbScanners.Items.Add("Сканеры не найдены");
				button1.Enabled = false; // Отключить кнопку "Сканировать"
			}*/

			TwainAPI.DTWAIN_SysInitialize();

			pArray = TwainAPI.DTWAIN_ArrayInit();
			hr = TwainAPI.DTWAIN_EnumSources(ref pArray);
			int nCountSources = TwainAPI.DTWAIN_ArrayGetCount(pArray);

			if (nCountSources < 1) // Сканеров не найдено
			{
				cmbScanners.Items.Add("Сканеры не найдены");
				button1.Enabled = false; // Отключить кнопку "Сканировать"
				hr = TwainAPI.DTWAIN_SysDestroy();
			}

			string prevSourceName = Properties.Settings.Default.ScannerName;
			IntPtr CurSource = IntPtr.Zero;
			StringBuilder lpszVer = new StringBuilder(256);
			for (int i = 0; i < nCountSources; i++)
			{
				StringBuilder szName = new StringBuilder(256);
				hr = TwainAPI.DTWAIN_ArrayGetAt(pArray, i, ref CurSource);
				hr = TwainAPI.DTWAIN_GetSourceProductName(CurSource, szName, 255);
				cmbScanners.Items.Add(szName.ToString());
				if (szName.ToString() == prevSourceName)
					cmbScanners.SelectedIndex = i;
			}

			LoadConfig();

			/*if ((TwainAPI.DTWAIN_GetTwainAvailability() & TwainAPI.DTWAIN_TWAINDSM_VERSION2) > 0)
			{ // DTWAIN_TWAINDSM_VERSION2
				TwainAPI.DTWAIN_SetTwainDSM(TwainAPI.DTWAIN_TWAINDSM_VERSION2);
				toolStripDSM.Text = "DSM: Version 2";
			}
			else
			{
				TwainAPI.DTWAIN_SetTwainDSM(TwainAPI.DTWAIN_TWAINDSM_LEGACY);
				toolStripDSM.Text = "DSM: Legacy";
			}*/


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
			else
				e.Effect = DragDropEffects.Move;
		}

		private void listView1_DragDrop(object sender, DragEventArgs e)
		{
			int numItem; // Номер итема, на который перетащили картинку
						 // Притащили новые файлы мышкой из папки
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				AddFilesToListView((string[])e.Data.GetData(DataFormats.FileDrop));
			// Передвинули существующие картинки
			if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem"))
			{
				ListViewItem fromItem = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;
				//ListViewItem copyFromItem = (ListViewItem)fromItem.Clone();
				ListViewItem toItem = listView1.GetItemAt(e.X - listView1.Left - this.Left, e.Y - listView1.Top - this.Top);
				if (null == toItem)
				{
					//textBox1.AppendText(" Item index " + listView1.Items.Count.ToString() + "\r\n");
					numItem = listView1.Items.Count - 1;
				}
				else
				{
					//textBox1.AppendText(" Item index " + toItem.Index.ToString() + "\r\n");
					numItem = toItem.Index;
				}

				/*foreach (ListViewItem it in listView1.Items)
					textBox1.AppendText(it.Text + " = " + it.Index.ToString() + " = " + it.ImageIndex.ToString() + "\r\n");
				textBox1.AppendText(listView1.Items.Count.ToString() + "\r\n");*/

				listView1.Items.Remove(fromItem);

				/*foreach (ListViewItem it in listView1.Items)
					textBox1.AppendText(it.Text + " = " + it.Index.ToString() + " = " + it.ImageIndex.ToString() + "\r\n");
				textBox1.AppendText(listView1.Items.Count.ToString() + "\r\n");*/

				listView1.Items.Insert(numItem, fromItem);

				/*foreach (ListViewItem it in listView1.Items)
					textBox1.AppendText(it.Text + " = " + it.Index.ToString() + " = " + it.ImageIndex.ToString() + "\r\n");
				textBox1.AppendText(listView1.Items.Count.ToString() + "\r\n");*/

				//listView1.RedrawItems(0, listView1.Items.Count-1, false);
				//for (int idx = 0; idx < listView1.Items.Count; idx++)
				//	listView1.Items[idx].ImageIndex = idx;
			}
		}

		private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
		{
			ListViewItem lvi = (ListViewItem)e.Item;
			listView1.DoDragDrop((object)lvi, DragDropEffects.Move);
		}

		private void listView1_DragOver(object sender, DragEventArgs e)
		{
			/*if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem"))
			{
				ListViewItem lvi = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;
				Size offset = Size.Subtract(Cursor.Size, new Size(Cursor.HotSpot.X, Cursor.HotSpot.Y));
				lvi.Position = Point.Subtract(listView1.PointToClient(new Point(e.X, e.Y)), offset);
				e.Effect = DragDropEffects.Copy;
			}*/
		}

		// https://stackoverflow.com/questions/1406808/wait-for-file-to-be-freed-by-process
		private bool IsFileReady(string filename)
		{
			// If the file can be opened for exclusive access it means that the file
			// is no longer locked by another process.
			try
			{
				using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
					return inputStream.Length > 0;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private void AppendFile(string file)
		{
			// Ждем пока появится отсканированный файл
			//while (!File.Exists(file))
			//{ w.WriteLine("File " + file + " not exists"); }
			// Подождать освобождения файла, вдруг сканер его еще не дописал
			//while (!form1.IsFileReady(file))
			//{ w.WriteLine("File " + file + " locked"); }
			//w.WriteLine(String.Format("sFileName = {0}, file = {1}, numFileName = {2}", sFileName, file, numFileName));

			// Выровнять криво отсканированный рисунок
			/*if (chkDeskew.Checked)
			{
				MagickImage image = new MagickImage(file);
				image.Deskew(new Percentage((int)numDeskew.Value));
				image.Write(file);
				image.Dispose();
			}*/

			// Image thumb = Image.FromFile(file); //.GetThumbnailImage(71, 100, () => false, IntPtr.Zero);
			Bitmap thumb = GetThumbnailImage(file);

			imageList.Images.Add(thumb);
			listView1.Items.Add(Path.GetFileName(file));
			listView1.Items[count].ImageIndex = imageList.Images.Count - 1;
			count++;
			toolStripStatusLabelFilesCount.Text = String.Format("Файлов: {0}", count);

			thumb.Dispose();

			scanFileNames.Add(file);
		}

		/// <summary>
		///  Создает маленький рисунок для помещения в ListView
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		private static Bitmap GetThumbnailImage(string filePath /*Image img*/)
		{
			using (Image img = Image.FromFile(filePath))
			{
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
					imgHeight = (int)(imgWidth / ratio);
					yOffset = (int)((100 - imgHeight) / 2);
				}
				else // Рисунок слишком узкий
				{
					imgHeight = 100;
					imgWidth = (int)(imgHeight * ratio);
					xOffset = (int)((71 - imgWidth) / 2);
				}

				g.DrawImage(img, xOffset, yOffset, imgWidth, imgHeight);

				//imageList.Images[idx] = bmp;
				//bmp.Dispose();
				img.Dispose();
				return bmp;
			}
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
			for (int idx = listView1.Items.Count - 1; idx >= 0; idx--)
			{
				if (listView1.Items[idx].Selected)
				{
					string fileToDelete = scanFileNames[listView1.Items[idx].ImageIndex];
					//scanFileNames.RemoveAt(listView1.Items[idx].ImageIndex);
					//imageList.Images.RemoveAt(idx);
					listView1.Items.RemoveAt(idx);

					count--;

					if (chkDeleteFiles.Checked) File.Delete(fileToDelete);

					toolStripStatusLabelFilesCount.Text = String.Format("Файлов: {0}", count);

					/*foreach (ListViewItem it in listView1.Items)
						textBox1.AppendText(it.Text + " = " + it.Index.ToString() + " = " + it.ImageIndex.ToString() + "\r\n");
					textBox1.AppendText(listView1.Items.Count.ToString() + "\r\n");*/
				}
			}

			// Index меняется автоматом, а ImageIndex нужно установить принудительно
			/*for (int idx = 0; idx < listView1.Items.Count; idx++)
				listView1.Items[idx].ImageIndex = idx;*/
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
					images.Add(scanFileNames[item.ImageIndex]);
					images[s++].Strip(); // Иначе PDF может получиться кривой с ошибкой insufficient data for an image
				}
				try
				{
					images.Write(savePDFDialog.FileName, MagickFormat.Pdf);
				}
				catch (MagickException me)
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
					iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(scanFileNames[item.ImageIndex]);
					image.SetAbsolutePosition(0.0f, 0.0f);
					image.ScaleToFit(image.Width / image.DpiX * 72f, image.Height / image.DpiY * 72f);
					iTextSharp.text.Chunk chunk = new iTextSharp.text.Chunk();
					chunk.SetNewPage();
					if (s > 0) document.Add(chunk);
					if (deleteFiles & !document.Add(image)) deleteFiles = false;
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

			savePDFDialog.Filter = "Файлы TIFF|*.tif;*.tiff"; // stringManager.GetString("Файлы TIFF|*.tif;*.tiff", CultureInfo.CurrentUICulture);
			savePDFDialog.InitialDirectory = PDFInitialDir;
			if (DialogResult.OK != savePDFDialog.ShowDialog()) return;

			Cursor.Current = Cursors.WaitCursor;

			// Вариант с FreeImage - быстро, но сложнее и иногда инвертирует цвета

			// Сохранить первую страницу
			FIBITMAP dib_color = FreeImage.LoadEx(scanFileNames[listView1.SelectedItems[0].ImageIndex]);

			FIBITMAP dib = FreeImage.ConvertToGreyscale(dib_color);
			FreeImage.UnloadEx(ref dib_color);
			if (FREE_IMAGE_COLOR_TYPE.FIC_MINISBLACK == FreeImage.GetColorType(dib))
				FreeImage.Invert(dib);

			//deleteFiles =
			FreeImage.SaveEx(ref dib, savePDFDialog.FileName, FREE_IMAGE_FORMAT.FIF_TIFF, FREE_IMAGE_SAVE_FLAGS.TIFF_CCITTFAX4, FREE_IMAGE_COLOR_DEPTH.FICD_01_BPP_THRESHOLD, true);
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
					fib_color = FreeImage.LoadEx(scanFileNames[listView1.SelectedItems[i].ImageIndex]);
					fib = FreeImage.ConvertToGreyscale(fib_color);
					FreeImage.UnloadEx(ref fib_color);

					if (FREE_IMAGE_COLOR_TYPE.FIC_MINISBLACK == FreeImage.GetColorType(fib))
						FreeImage.Invert(fib);

					FreeImage.SaveEx(ref fib, tmpFile, FREE_IMAGE_FORMAT.FIF_TIFF, FREE_IMAGE_SAVE_FLAGS.TIFF_CCITTFAX4, FREE_IMAGE_COLOR_DEPTH.FICD_01_BPP_THRESHOLD, true);
					fib = FreeImageAPI.FreeImage.LoadEx(tmpFile);

					FreeImage.AppendPage(fmb, fib);
					FreeImage.UnloadEx(ref fib);
					File.Delete(tmpFile);
				}
				//deleteFiles = 
				FreeImage.CloseMultiBitmapEx(ref fmb);
			}


			// Вариант с ImageMagick - проще, но очень медленно
			/*int s = 0;

			MagickImageCollection images = new MagickImageCollection();
			foreach (ListViewItem item in listView1.SelectedItems)
			{
				images.Add(scanFileNames[item.ImageIndex]);
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
			if (DialogResult.OK == dlgSelectDir.ShowDialog())
			{
				txtFileDir.Text = dlgSelectDir.SelectedPath;
			}
		}

		private string GetFileName()
		{
			string file = txtFileDir.Text + "\\" + txtFileName.Text + String.Format("_{0:000}", fileSuffix++) + ".jpg";
			while (File.Exists(file))
				file = txtFileDir.Text + "\\" + txtFileName.Text + String.Format("_{0:000}", fileSuffix++) + ".jpg";
			return file;
		}

		private void SaveConfig()
		{
			//Properties.Settings.Default.Scanner = cmbScanners.SelectedIndex;
			Properties.Settings.Default.ScannerName = cmbScanners.SelectedItem.ToString();
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
			Properties.Settings.Default.ShowInterface = chkShowInterface.Checked;
			Properties.Settings.Default.DSM = cmbDSM.SelectedIndex;
			//Properties.Settings.Default.bDeskew = chkDeskew.Checked;
			Properties.Settings.Default.nDeskew = numDeskew.Value;
			Properties.Settings.Default.Save();
		}

		private void LoadConfig()
		{
			//cmbScanners.SelectedIndex = Properties.Settings.Default.Scanner;
			//cmbScanners.SelectedText = Properties.Settings.Default.ScannerName;
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
			chkShowInterface.Checked = Properties.Settings.Default.ShowInterface;
			cmbDSM.SelectedIndex = Properties.Settings.Default.DSM;
			//chkDeskew.Checked = Properties.Settings.Default.bDeskew;
			numDeskew.Value = Properties.Settings.Default.nDeskew;
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			dlgAddFiles.Multiselect = true;
			dlgAddFiles.InitialDirectory = PDFInitialDir;
			dlgAddFiles.Filter = "Изображения (*.pdf;*.jpg;*.jpeg)|*.pdf;*.jpg;*.jpeg"; //stringManager.GetString("Изображения (*.pdf;*.jpg;*.jpeg)|*.pdf;*.jpg;*.jpeg", CultureInfo.CurrentUICulture);
			if (DialogResult.OK == dlgAddFiles.ShowDialog())
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
					if (ExecuteCommandAsync(args))
						foreach (string fileName in Directory.GetFiles(txtFileDir.Text, Path.GetFileNameWithoutExtension(f) + "_*.jpg"))
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
			//listView1.ListViewItemSorter = new ListViewItemComparer(); // Для сортировки элементов по индексу

			/*Image oldImage = (Image)imageList.Images[oldIdx].Clone();
			imageList.Images[oldIdx] = imageList.Images[newIdx];
			imageList.Images[newIdx] = oldImage;
			oldImage.Dispose();

			listView1.Items[oldIdx].ImageIndex = oldIdx;
			listView1.Items[newIdx].ImageIndex = newIdx;*/
			//foreach(ListViewItem i in listView1.Items)
			//	i.ImageIndex = i.Index;

			/*string oldFileName = scanFileNames[oldIdx];
			scanFileNames[oldIdx] = scanFileNames[newIdx];
			scanFileNames[newIdx] = oldFileName;*/

			//listView1.Refresh();
		}

		private void button2_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(txtFileDir.Text))
			{
				MessageBox.Show("Выберите путь для отсканированных файлов");
				return;
			}

			int status = 0;
			//IntPtr SelectedSource = IntPtr.Zero; // = TwainAPI.DTWAIN_SelectSource();
			SelectedSource = TwainAPI.DTWAIN_SelectSourceByName(cmbScanners.SelectedItem.ToString());
			if (0 == TwainAPI.DTWAIN_IsSourceOpen(SelectedSource))
				hr = TwainAPI.DTWAIN_OpenSource(SelectedSource);
			//hr = TwainAPI.DTWAIN_ArrayGetAt(pArray, cmbScanners.SelectedIndex, ref SelectedSource);
			hr = TwainAPI.DTWAIN_SetResolution(SelectedSource, float.Parse(cmbResolution.SelectedItem.ToString()));
			hr = TwainAPI.DTWAIN_EnableFeeder(SelectedSource, chkFeeder.Checked ? 1 : 0);
			hr = TwainAPI.DTWAIN_EnableDuplex(SelectedSource, chkDuplex.Checked ? 1 : 0);
			hr = TwainAPI.DTWAIN_SetPDFJpegQuality(SelectedSource, trkJpegQuality.Value * 10);
			hr = TwainAPI.DTWAIN_SetBrightness(SelectedSource, trkBrightness.Value * 10.0f);
			hr = TwainAPI.DTWAIN_SetContrast(SelectedSource, trkContrast.Value * 10.0f);

			//IntPtr 
			aFileNames = TwainAPI.DTWAIN_ArrayCreate(TwainAPI.DTWAIN_ARRAYSTRING, MAX_FILES);
			for (int i = 0; i < MAX_FILES; i++)
			{
				//string s = GetFileName();
				hr = TwainAPI.DTWAIN_ArraySetAtString(aFileNames, i, GetFileName());
				//textBox1.AppendText(s + "\r\n");
			}
			fileSuffix -= MAX_FILES;

			int maxPages = chkFeeder.Checked ? TwainAPI.DTWAIN_ACQUIREALL : 1;

			hr = TwainAPI.DTWAIN_SetMaxAcquisitions(SelectedSource, maxPages);

			// int maxPages = 1;
			//do
			//{
			//	fName = GetFileName();
			//	hr = TwainAPI.DTWAIN_AcquireFile(SelectedSource, fName, TwainAPI.DTWAIN_JPEG, TwainAPI.DTWAIN_USENATIVE | TwainAPI.DTWAIN_USELONGNAME, cmbColor.SelectedIndex /*TwainAPI.DTWAIN_PT_DEFAULT*/, maxPages, chkShowInterface.Checked ? 1 : 0, 1, ref status);
			//	if (File.Exists(fName))
			//		AppendFile(fName);
			//} while (status == TwainAPI.DTWAIN_TN_ACQUIREDONE);
			//return;

			hr = TwainAPI.DTWAIN_EnableMsgNotify(1);
			TwainAPI.DTwainCallback cb = new TwainAPI.DTwainCallback(CaptureTwainProc);
			TwainAPI.DTWAIN_SetCallback(cb, chkShowInterface.Checked ? 1 : 0);

			numFileName = 0;
			bAcquireDone = false;
			bStopAtError = false;
			//Thread th = new Thread(new ThreadStart(AddScanThread));
			//th.Start();
			hr = TwainAPI.DTWAIN_AcquireFileEx(SelectedSource, (int)aFileNames, TwainAPI.DTWAIN_JPEG, TwainAPI.DTWAIN_USENATIVE | TwainAPI.DTWAIN_USELONGNAME, cmbColor.SelectedIndex, maxPages, chkShowInterface.Checked ? 1 : 0, 0, ref status);
			//bAcquireDone = true;
			TwainAPI.DTWAIN_SetCallback(null, 0);
			hr = TwainAPI.DTWAIN_EnableMsgNotify(0);

			/*StringBuilder fileName = new StringBuilder(MAX_FILES);
			for (int i = 0; i < MAX_FILES; i++)
			{
				hr = TwainAPI.DTWAIN_ArrayGetAtString(aFileNames, i, fileName);
				if (File.Exists(fileName.ToString()))
					AppendFile(fileName.ToString());
				else
					break;
				fileName.Clear();
				++fileSuffix;
			}*/

			//hr = TwainAPI.DTWAIN_ArrayDestroy(aFileNames);
			//hr = TwainAPI.DTWAIN_CloseSourceUI(SelectedSource);
			//hr = TwainAPI.DTWAIN_CloseSource(SelectedSource);

			if (status != TwainAPI.DTWAIN_TN_ACQUIREDONE)
			{
				bStopAtError = true;
				switch (status)
				{
					case TwainAPI.DTWAIN_TN_ACQUIREFAILED:
						toolStripStatus.Text = "ACQUIREFAILED";
						break;
					case TwainAPI.DTWAIN_TN_ACQUIRECANCELLED:
						toolStripStatus.Text = "ACQUIRECANCELLED";
						break;
					default:
						StringBuilder lpszVer = new StringBuilder(256);
						hr = TwainAPI.DTWAIN_GetErrorString(status, lpszVer, 255);
						toolStripStatus.Text = lpszVer.ToString();
						break;
				}
				return;
			}
		}

		private static int CaptureTwainProc(int wParam, int lParam, int UserData)
		{
			//w.WriteLine(String.Format("wParam = {0}", wParam));
			// UserData = 1 if chkShowInterface.Checked
			int hr = 0;
			switch (wParam)
			{
				/* Notification of acquisition being started */
				case TwainAPI.DTWAIN_TN_ACQUIRESTARTED:
					break;
				/* Transfer is about to take place */
				case TwainAPI.DTWAIN_TN_TRANSFERREADY:
					break;
				/* Transfer was done! */
				case TwainAPI.DTWAIN_TN_TRANSFERDONE:
					//MessageBox.Show("Transfer was done");
					//hr = TwainAPI.DTWAIN_CloseSourceUI(SelectedSource);
					break;
				/* Acquired all pages.  We can get the DIBs! */
				case TwainAPI.DTWAIN_TN_ACQUIREDONE:
					//MessageBox.Show("Acquired all pages");
					//if (1 == UserData)
					hr = TwainAPI.DTWAIN_CloseSourceUI(SelectedSource);
					bAcquireDone = true;
					countAcquired = 0;
					hr = TwainAPI.DTWAIN_ArrayDestroy(aFileNames);
					break;
				case TwainAPI.DTWAIN_TN_ACQUIRECANCELLED:
					bStopAtError = true;
					hr = TwainAPI.DTWAIN_CloseSourceUI(SelectedSource);
					break;
				case TwainAPI.DTWAIN_TN_ACQUIREFAILED:
					bStopAtError = true;
					hr = TwainAPI.DTWAIN_CloseSourceUI(SelectedSource);
					break;
				case TwainAPI.DTWAIN_TN_ACQUIRETERMINATED:
					bStopAtError = true;
					hr = TwainAPI.DTWAIN_CloseSourceUI(SelectedSource);
					break;
				case TwainAPI.DTWAIN_TN_FILEPAGESAVEOK:
					StringBuilder pStr = new StringBuilder(256);
					hr = TwainAPI.DTWAIN_ArrayGetAtString(aFileNames, numFileName++, pStr);
					sFileName = pStr.ToString();

					//form1.AppendFile(sFileName);

					Task tskAdd = new Task(() =>
					{
						string sFile = sFileName;
						// Ждем пока появится отсканированный файл
						while (!File.Exists(sFile))
						{
							if (bStopAtError) { return; }
							Thread.Sleep(10);
						}
						// Подождать освобождения файла, вдруг сканер его еще не дописал
						while (!form1.IsFileReady(sFile))
						{
							if (bStopAtError) { return; }
							Thread.Sleep(10);
						}

						// Выровнять криво отсканированный рисунок
						/*if (form1.chkDeskew.Checked)
						{
							MagickImage image = new MagickImage(sFile);
							image.Deskew(new Percentage((int)form1.numDeskew.Value));
							image.Write(sFile);
							image.Dispose();
						}*/

						Bitmap thumb = GetThumbnailImage(sFile);

						form1.listView1.Invoke(new Action(() => form1.imageList.Images.Add(thumb)));
						form1.listView1.Invoke(new Action(() => form1.listView1.Items.Add(Path.GetFileName(sFile))));
						form1.listView1.Invoke(new Action(() => form1.listView1.Items[count].ImageIndex = form1.imageList.Images.Count - 1));
						//ListView1Add(sFile);
						count++;
						form1.Invoke(new Action(() => form1.toolStripStatusLabelFilesCount.Text = String.Format("Файлов: {0}", count)));

						thumb.Dispose();

						form1.scanFileNames.Add(sFile);
					});
					tskAdd.Start();

					countAcquired++;
					form1.toolStripMsg.Text = String.Format("bAcquireDone={0} numFileName={1} countAcquired={2}", bAcquireDone, numFileName, countAcquired);
					//bFileReady = true;

					pStr.Clear();
					++fileSuffix;
					break;
				default:
					//MessageBox.Show(wParam.ToString());
					break;
			}
			return 1;
			//w.Close();
		}

		// Процедура добваления отсканированных файлов по мере готовности
		/*private static void AddScanThread()
		{
			int hres;
			do
			{
				if (bStopAtError) { break; }
				//if (bFileReady)
				//{
				StringBuilder pStr = new StringBuilder(256);
				hres = TwainAPI.DTWAIN_ArrayGetAtString(aFileNames, numFileName++, pStr);
				sFileName = pStr.ToString();

				//w.WriteLine(String.Format("sFileName = {0}, numFileName = {1}", sFileName, numFileName));

				// Ждем пока появится отсканированный файл
				while (!File.Exists(sFileName))
				{
					if (bStopAtError) { return; }
					Thread.Sleep(10);
				}
				// Подождать освобождения файла, вдруг сканер его еще не дописал
				while (!form1.IsFileReady(sFileName))
				{
					if (bStopAtError) { return; }
					Thread.Sleep(10);
				}

				
				// Выровнять криво отсканированный рисунок
				if (form1.chkDeskew.Checked)
				{
					MagickImage image = new MagickImage(sFileName);
					image.Deskew(new Percentage((int)form1.numDeskew.Value));
					image.Write(sFileName);
					image.Dispose();
				}
				
				Bitmap thumb = GetThumbnailImage(sFileName);
				
				form1.listView1.Invoke(new Action(() => form1.imageList.Images.Add(thumb)));
				form1.listView1.Invoke(new Action(() => form1.listView1.Items.Add(Path.GetFileName(sFileName))));
				form1.listView1.Invoke(new Action(() => form1.listView1.Items[count].ImageIndex = form1.imageList.Images.Count - 1));
				//ListView1Add(sFileName);
				count++;
				form1.toolStripStatusLabelFilesCount.Text = String.Format("Файлов: {0}", count);

				thumb.Dispose();

				form1.scanFileNames.Add(sFileName);
				pStr.Clear();
				++fileSuffix;
				//bFileReady = false;
				//}
				//Thread.Sleep(200);
				//w.WriteLine(String.Format("?{0} {1} == {2} ERR?{3}", bAcquireDone, numFileName, countAcquired, bStopAtError));
				//if (bAcquireDone)
				//	if (numFileName == countAcquired)
				//		break;
			} while ( true );  //while (!( bAcquireDone && ( numFileName == countAcquired )));
			countAcquired = 0;
			hres = TwainAPI.DTWAIN_ArrayDestroy(aFileNames);
			//hres = TwainAPI.DTWAIN_CloseSourceUI(SelectedSource);
		}*/

		/*private static void ListView1Add(string sFileName)
		{
			Bitmap thumb = form1.GetThumbnailImage(sFileName);
			form1.listView1.Invoke(new Action(() => form1.imageList.Images.Add(thumb)));
			form1.listView1.Invoke(new Action(() => form1.listView1.Items.Add(Path.GetFileName(sFileName))));
			form1.listView1.Invoke(new Action(() => form1.listView1.Items[count].ImageIndex = form1.imageList.Images.Count - 1));
			thumb.Dispose();
		}*/

		private void cmbDSM_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cmbDSM.SelectedIndex)
			{
				case 0:
					hr = TwainAPI.DTWAIN_SetTwainDSM(TwainAPI.DTWAIN_TWAINDSM_VERSION2);
					toolStripDSM.Text = "DSM: Version 2";
					break;
				case 1:
					hr = TwainAPI.DTWAIN_SetTwainDSM(TwainAPI.DTWAIN_TWAINDSM_LEGACY);
					toolStripDSM.Text = "DSM: Legacy";
					break;
			}
		}

		private void btnDeskew_Click(object sender, EventArgs e)
		{		
			Percentage percentage = new Percentage((int)form1.numDeskew.Value);

			toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
			toolStripProgressBar1.MarqueeAnimationSpeed = 100;

			List<Task> listTask = new List<Task>();

			foreach (ListViewItem item in listView1.SelectedItems)
			{
				//for (int idx = listView1.Items.Count - 1; idx >= 0; idx--)
				//listView1.Invoke(new Action(() => bSel = listView1.Items[idx].Selected));
				//if (bSel)

				string fileToDeskew = scanFileNames[item.ImageIndex];

				Task task = new Task(() =>
				{
					// Выровнять криво отсканированный рисунок
					MagickImage image = new MagickImage(fileToDeskew);
					image.Deskew(percentage);
					image.Write(fileToDeskew);
					image.Dispose();
					imageList.Images[item.ImageIndex] = GetThumbnailImage(fileToDeskew);
				});
				listTask.Add(task);

			}
			ThreadPool.QueueUserWorkItem(DoDeskew, listTask);
			//listView1.Refresh();
			//toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
			//toolStripProgressBar1.MarqueeAnimationSpeed = 0;
		}

		// Запускает задачи выравнивания рисунков в отдельном потоке и ожидает завршения всех задач
		private void DoDeskew(object state)
		{
			foreach (Task t in (List<Task>)state)
				t.Start();
			Task.WaitAll(((List<Task>)state).ToArray());
			listView1.Invoke(new Action(() => listView1.Refresh()));
			form1.Invoke(new Action(() => toolStripProgressBar1.Style = ProgressBarStyle.Continuous));
			form1.Invoke(new Action(() => toolStripProgressBar1.MarqueeAnimationSpeed = 0));
		}
	}

	public static class ImageFormatID
	{
		public const string wiaFormatBMP = "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}";
		public const string wiaFormatPNG = "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}";
		public const string wiaFormatGIF = "{B96B3CB0-0728-11D3-9D7B-0000F81EF32E}";
		public const string wiaFormatJPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";
		public const string wiaFormatTIFF = "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}";
	}
}
