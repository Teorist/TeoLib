using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace TeoLib.Utils
{
	public static class ExtensionsData
	{
		public static DataTable ToDataTable<T>(this IEnumerable<T> collection, string name)
		{
			if (null == collection)
				throw new ArgumentNullException("collection");

			DataTable dt = new DataTable(!string.IsNullOrEmpty(name) ? name : "DataTable");
			Type t = typeof(T);
			PropertyInfo[] pia = t.GetProperties();

			//Inspect the properties and create the columns in the DataTable
			foreach (PropertyInfo pi in pia)
			{
				Type ColumnType = pi.PropertyType;

				if ((ColumnType.IsGenericType))
				{
					ColumnType = ColumnType.GetGenericArguments()[0];
				}

				dt.Columns.Add(pi.Name, ColumnType);
			}

			//Populate the data table
			foreach (T item in collection)
			{
				DataRow dr = dt.NewRow();
				dr.BeginEdit();

				foreach (PropertyInfo pi in pia)
				{
					if (pi.GetValue(item, null) != null)
					{
						dr[pi.Name] = pi.GetValue(item, null);
					}
				}

				dr.EndEdit();
				dt.Rows.Add(dr);
			}

			return dt;
		}

		public static Error ToJson(this object obj, Stream stream)
		{
			Error error = Error.None;

			if (null == obj)
			{
				error = Error.EmptyArgument;
				throw new ArgumentNullException("obj");
			}

			if (null == stream)
			{
				error = Error.EmptyArgument;
				throw new ArgumentNullException("stream");
			}

			new DataContractJsonSerializer(obj.GetType()).WriteObject(stream, obj);

			return error;
		}

		public static object FromJson(this Stream stream, Type type)
		{
			Error error = Error.None;

			if (null == stream)
			{
				error = Error.EmptyArgument;
				throw new ArgumentNullException("stream");
			}

			return new DataContractJsonSerializer(type).ReadObject(stream);
		}

		public static Error ToXml(this object obj, Stream stream)
		{
			Error error = Error.None;

			if (null == obj)
			{
				error = Error.EmptyArgument;
				throw new ArgumentNullException("obj");
			}

			if (null == stream)
			{
				error = Error.EmptyArgument;
				throw new ArgumentNullException("stream");
			}

			new XmlSerializer(obj.GetType()).Serialize(stream, obj);

			return error;
		}

		public static object FromXml(this Stream stream, Type type)
		{
			Error error = Error.None;

			if (null == stream)
			{
				error = Error.EmptyArgument;
				throw new ArgumentNullException("stream");
			}

			return new XmlSerializer(type).Deserialize(stream);
		}
	}
}
