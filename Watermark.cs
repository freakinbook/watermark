using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watermark3
{
	interface IWorker
	{
		void Notify(string message);
	}
	class Watermark : IWorker
	{
		public List<ILogger> Loggers { get; private set; }
		public string WatermarkPath { get; private set; }

		public Watermark(string watermarkPath)
		{
			Loggers = new List<ILogger>();
			WatermarkPath = watermarkPath;
		}

		public void AddWatermark(string imagePath)
		{
			using (Image image = Image.FromFile(imagePath))
			using (Image watermark = Image.FromFile(WatermarkPath))
			using (TextureBrush brush = new TextureBrush(watermark))
			{
				Graphics graphics = null;
				try
				{
					graphics = Graphics.FromImage(image);
				}
				catch (Exception ex)
				{
					//here we handle indexed pixels
					Notify($"Image {imagePath} has indexed pixels.");
					Console.WriteLine(ex.Message);
					Bitmap tempImage = new Bitmap(image.Width, image.Height);
					graphics = Graphics.FromImage(tempImage);
					graphics.DrawImage(image, 0, 0);
				}
				finally
				{
					int x = image.Width - 10 - watermark.Width;
					int y = image.Height - 10 - watermark.Height;
					brush.TranslateTransform(x, y);
					graphics.FillRectangle(brush, new Rectangle(x, y, watermark.Width, watermark.Height));
					image.Save(GetNewFileName(imagePath));
				}
			}
		}

		public void MarkAllFiles(string[] filePaths)
		{
			if (filePaths.Length == 0)
			{
				Notify("No files to watermark.");
				return;
			}
			for (int i = 0; i < filePaths.Length; i++)
			{
				AddWatermark(filePaths[i]);
			}
		}

		private string GetNewFileName(string fileName)
		{
			string extension = Path.GetExtension(fileName);
			string newFileName = "w_" + Path.GetFileNameWithoutExtension(fileName) + extension;
			string directory = Path.GetDirectoryName(fileName);
			string newFullName = Path.Combine(directory, newFileName);
			File.Create(newFullName).Close();
			Notify($"File {newFullName} created.");
			return newFullName;
		}

		public void Notify(string message)
		{
			for (int i = 0; i < Loggers.Count; i++)
			{
				Loggers[i].Log(message);
			}
		}
	}
}
