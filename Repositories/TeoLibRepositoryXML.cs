using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using TeoLib.Models;
using TeoLib.Models.Collections;
using TeoLib.Utils;

namespace TeoLib.Repositories
{
	public class TeoLibRepositoryXML : ITeoLibRepository
	{
		private string _dataLocation = null;
		public static readonly ITeoLibRepository Instance = new TeoLibRepositoryXML();

		public DebugWrite DebugWriteLine { get; set; }
		public string DataLocation
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_dataLocation))
				{
					_dataLocation = FileTools.AppLocation;

					if (0 < _dataLocation.Length && '\\' != _dataLocation[_dataLocation.Length - 1])
						_dataLocation += "\\";

					_dataLocation += "Data\\";
				}

				return _dataLocation;
			}
		}

		private TeoLibRepositoryXML()
		{
		}

		protected void DebugWrite(string text)
		{
			if (null != DebugWriteLine)
				DebugWriteLine(text);
		}

		#region Library
		public Library GetLibrary()
		{
			DebugWrite("TeoLibRepositoryXML.GetLibrary()");

			Library library = new Library();

			using (DataSet ds = ReadXMLData(BuildApplicationDataFilePath("Library.xml"), BuildApplicationDataFilePath("BibliTeo.xsd")))
			{
				if (null != ds)
				{
					if (null != ds.Tables["library"] && ds.Tables["library"].Columns.Contains("version") && 0 < ds.Tables["library"].Rows.Count)
						library.Version = !string.IsNullOrWhiteSpace(ds.Tables["library"].Rows[0]["version"].ToString()) ? double.Parse(ds.Tables["library"].Rows[0]["version"].ToString()) : 1.0;

					if (null != ds.Tables["item"])
						library.Items = (from item in ds.Tables["item"].AsEnumerable()
										 orderby item.Field<string>("title"), item.Field<string>("type")
										 select new ItemViewModel()
										 {
											 Id = item.Table.Columns.Contains("id") ? item.Field<string>("id") : ItemViewModel.DefaultString,
											 Title = item.Table.Columns.Contains("title") ? item.Field<string>("title") : ItemViewModel.DefaultString,
											 Type = item.Table.Columns.Contains("type") ? item.Field<string>("type") : ItemViewModel.DefaultString,
											 Note = item.Table.Columns.Contains("note") ? item.Field<string>("note") : ItemViewModel.DefaultString,
											 Status = ItemViewModel.DefaultString,//item.Table.Columns.Contains("status") ? item.Field<string>("status") : Item.DefaultString,
											 Quality = item.Table.Columns.Contains("quality") ? item.Field<int?>("quality") : ItemViewModel.DefaultInt
										 }).ToList();
				}
			}

			DebugWrite(string.Format("TeoLibRepositoryXML.GetLibrary: {0} items read, version {1}.", library.Items.Count, library.Version));

			return library;
		}

		public Error SaveLibrary(Library library)
		{
			DebugWrite("TeoLibRepositoryXML.SaveLibrary(...)");
			Error error = Error.None;

			using (DataSet ds = new DataSet("library"))
			{
				ds.Tables.Add((from item in library.Items
							   select new {
											id = item.Id,
											type = item.Type,
											title = item.Title,
											//status = item.Status,
											quality = item.Quality,
											note = item.Note
										   }).ToDataTable("item"));
				ds.Tables["item"].Columns["id"].ColumnMapping = MappingType.Attribute;
				ds.Tables["item"].Columns["type"].ColumnMapping = MappingType.Attribute;
				error = WriteXMLData(ds, BuildApplicationDataFilePath("Library.xml"));
			}

			return error;
		} 
		#endregion

		#region People
		public People GetPeople()
		{
			DebugWrite("TeoLibRepositoryXML.GetPeople()");

			People people = new People();

			using (DataSet ds = ReadXMLData(BuildApplicationDataFilePath("People.xml"), BuildApplicationDataFilePath("BibliTeo.xsd")))
			{
				if (null != ds)
				{
					if (null != ds.Tables["people"] && ds.Tables["people"].Columns.Contains("version") && 0 < ds.Tables["people"].Rows.Count)
						people.Version = !string.IsNullOrWhiteSpace(ds.Tables["people"].Rows[0]["version"].ToString()) ? double.Parse(ds.Tables["people"].Rows[0]["version"].ToString()) : 1.0;

					if (null != ds.Tables["person"])
						people.Persons = (from person in ds.Tables["person"].AsEnumerable()
										  orderby person.Field<string>("name")
										  select new PersonViewModel()
										  {
											   Id = person.Table.Columns.Contains("id") ? person.Field<string>("id") : PersonViewModel.DefaultString,
											   Name = person.Table.Columns.Contains("name") ? person.Field<string>("name") : PersonViewModel.DefaultString,
											   Email = person.Table.Columns.Contains("email") ? person.Field<string>("email") : PersonViewModel.DefaultString,
											   Phone = person.Table.Columns.Contains("phone") ? person.Field<string>("phone") : PersonViewModel.DefaultString,
											   Note = person.Table.Columns.Contains("note") ? person.Field<string>("note") : PersonViewModel.DefaultString
										  }).ToList();
				}
			}

			DebugWrite(string.Format("TeoLibRepositoryXML.GetPeople: {0} persons read, version {1}.", people.Persons.Count, people.Version));

			return people;
		}
		public Error SavePeople(People people)
		{
			DebugWrite("TeoLibRepositoryXML.SavePeople(...)");
			Error error = Error.None;

			using (DataSet ds = new DataSet("people"))
			{
				ds.Tables.Add((from person in people.Persons
							   select new {
											id = person.Id,
											name = person.Name,
											phone = person.Phone,
											email = person.Email,
											note = person.Note
										  }).ToDataTable("person"));
				ds.Tables["person"].Columns["id"].ColumnMapping = MappingType.Attribute;
				error = WriteXMLData(ds, BuildApplicationDataFilePath("People.xml"));
			}

			return error;
		} 
		#endregion

		#region Transactions
		public Transactions GetTransactions()
		{
			DebugWrite("TeoLibRepositoryXML.GetTransactions()");

			Transactions transactions = new Transactions();

			using (DataSet ds = ReadXMLData(BuildApplicationDataFilePath("Lending.xml"), BuildApplicationDataFilePath("BibliTeo.xsd")))
			{
				if (null != ds)
				{
					if (null != ds.Tables["transactions"] && ds.Tables["transactions"].Columns.Contains("version") && 0 < ds.Tables["transactions"].Rows.Count)
						transactions.Version = !string.IsNullOrWhiteSpace(ds.Tables["transactions"].Rows[0]["version"].ToString()) ? double.Parse(ds.Tables["transactions"].Rows[0]["version"].ToString()) : 1.0;

					if (null != ds.Tables["lending"])
						transactions.LendingTransactions = (from transaction in ds.Tables["lending"].AsEnumerable()
															select new LendingTransaction()
															{
																Id = transaction.Table.Columns.Contains("id") ? transaction.Field<string>("id") : LendingTransaction.DefaultString,
																Item = transaction.Table.Columns.Contains("item") ? transaction.Field<string>("item") : LendingTransaction.DefaultString,
																Borrower = transaction.Table.Columns.Contains("borrower") ? transaction.Field<string>("borrower") : LendingTransaction.DefaultString,
																LendingDate = transaction.Table.Columns.Contains("lendingDate") ? transaction.Field<DateTime?>("lendingDate") : LendingTransaction.DefaultDateTime,
																ReturnDate = transaction.Table.Columns.Contains("returnDate") ? transaction.Field<DateTime?>("returnDate") : LendingTransaction.DefaultDateTime,
																Note = transaction.Table.Columns.Contains("note") ? transaction.Field<string>("note") : LendingTransaction.DefaultString
															}).ToList();
				}
			}

			DebugWrite(string.Format("TeoLibRepositoryXML.GetLendingTransactions: {0} transactions read, version {1}.", transactions.LendingTransactions.Count, transactions.Version));

			return transactions;
		}
		public Error SaveTransactions(Transactions transactions)
		{
			DebugWrite("TeoLibRepositoryXML.SaveTransactions(...)");
			Error error = Error.None;

			using (DataSet ds = new DataSet("transactions"))
			{
				ds.Tables.Add((from transaction in transactions.LendingTransactions
							   select new {
											id = transaction.Id,
											item = transaction.Item,
											borrower = transaction.Borrower,
											lendingDate = transaction.LendingDate,
											returnDate = transaction.ReturnDate,
											note = transaction.Note
										  }).ToDataTable("lending"));
				ds.Tables["lending"].Columns["id"].ColumnMapping = MappingType.Attribute;
				error = WriteXMLData(ds, BuildApplicationDataFilePath("Lending.xml"));
			}

			return error;
		} 
		#endregion

		#region Tools
		private string BuildApplicationDataFilePath(string fileName)
		{
			return DataLocation + fileName;
		}

		private DataSet ReadXMLData(string filePath, string schemaPath)
		{
			DebugWrite(string.Format("TeoLibRepositoryXML.ReadXMLData({0})", filePath));

			DataSet ds = null;

			if (File.Exists(filePath))
			{
				ds = new DataSet();

				if (File.Exists(schemaPath))
					ds.ReadXmlSchema(schemaPath);

				ds.ReadXml(filePath);
			}

			return ds;
		}

		private Error WriteXMLData(DataSet data, string filePath)
		{
			Error error = Error.None;
			DebugWrite(string.Format("TeoLibRepositoryXML.WriteXMLData({0})", filePath));

			if (null != data && File.Exists(filePath))
			{
				data.WriteXml(filePath, XmlWriteMode.IgnoreSchema);
			}

			return error;
		}

		private bool ValidateXML(string filePath)
		{
			bool isValid = false;

			DebugWrite(string.Format("ValidateXML({0})", filePath));

			if (File.Exists(filePath))
			{
				XmlReaderSettings settings = new XmlReaderSettings();
				settings.CloseInput = true;
				settings.ValidationEventHandler += ValidateXML_Handler;
				settings.ValidationType = ValidationType.Schema;
				settings.Schemas.Add(null, BuildApplicationDataFilePath("BibliTeo.xsd"));
				settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings |
										   XmlSchemaValidationFlags.ProcessIdentityConstraints |
										   XmlSchemaValidationFlags.ProcessInlineSchema |
									       XmlSchemaValidationFlags.ProcessSchemaLocation;

				StringReader r = new StringReader(filePath);

				using (XmlReader validatingReader = XmlReader.Create(r, settings))
				{
					while (validatingReader.Read()) { /* just loop through document */ }
				}

				isValid = true;
			}

			DebugWrite(string.Format("ValidateXML returns: {0}.", isValid));

			return isValid;
		}

		private void ValidateXML_Handler(object sender, ValidationEventArgs e)
		{
			if (e.Severity == XmlSeverityType.Error || e.Severity == XmlSeverityType.Warning)
				DebugWrite(string.Format("XML Validation Error - Line: {0}, Position: {1} \"{2}\"", e.Exception.LineNumber, e.Exception.LinePosition, e.Exception.Message));
		}
		#endregion

		#region IDisposable
		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				//_dbContext.Dispose();
			}

			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
