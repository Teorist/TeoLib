using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace TeoLib.Models
{
	public class BaseModel
	{
		public static string DefaultString = null;
		public static int? DefaultInt = null;
		public static DateTime? DefaultDateTime = null;
		protected bool _isModified = false;

		public virtual bool IsModified {
			get { return _isModified; }
			set { _isModified = value; }
		}
		public double Version { get; set; }

		public BaseModel()
		{
			IsModified = false;
			Version = double.Parse(string.Format("{0}.{1}", Assembly.GetExecutingAssembly().GetName().Version.Major, Assembly.GetExecutingAssembly().GetName().Version.Minor), CultureInfo.InvariantCulture);
		}

		public BaseModel(BaseModel other)
		{
			IsModified = other.IsModified;
			Version = other.Version;
		}
	}
}
