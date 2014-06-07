using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeoLib.Utils
{
	public class FileTools
	{
		public static string AppLocation
		{
			get { return Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location); }
		}
	}
}
