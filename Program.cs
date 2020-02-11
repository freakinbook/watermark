using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watermark3
{
	class Program
	{
		static void Main(string[] args)
		{
			Watermark wm = null;
			if (args.Length != 0)
			{
				if (File.Exists(args[0]))
				{
					wm = new Watermark(args[0]);
				}
				else
				{
					new Logger().Log("File with this name doesn't exist.");
					return;
				}
			}
			else
			{
				new Logger().Log("Watermark file was not supplied.");
				return;
			}
			wm.Loggers.Add(new Logger());
			string[] filePaths = FileFinder.FindAllFiles("jpg","png");
			wm.MarkAllFiles(filePaths);
		}
	}
}
