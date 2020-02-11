using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Watermark3
{
	class FileFinder
	{
		public static string AppDirectory { get; private set; }

		static FileFinder()
		{
			AppDirectory = AppDomain.CurrentDomain.BaseDirectory;
		}

		public static string[] FindAllFiles(params string[] extensions)
		{
			if (extensions == null)
			{
				return Directory.GetFiles(AppDirectory);
			}
			string pattern = CreatePattern(extensions);
			Regex regex = new Regex(@pattern);
			return Directory.EnumerateFiles(AppDirectory, "*", SearchOption.TopDirectoryOnly).Where(fileName => regex.IsMatch(fileName)).ToArray();
		}

		private static string CreatePattern(params string[] parts)
		{
			string extensions = parts[0];
			if (parts.Length > 1)
			{
				for (int i = 1; i < parts.Length; i++)
				{
					extensions += "|" + parts[i];
				}
			}
			return $@"(?i:^.*\.({extensions})$)";
		}
	}
}
