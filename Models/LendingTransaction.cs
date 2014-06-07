using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace TeoLib.Models
{
	[Serializable, DataContract(Name="lending")]
	public class LendingTransaction : BaseModel
	{
		[XmlAttribute(AttributeName = "id")]
		[DataMember(Name = "id")]
		public string Id { get; set; }

		[XmlElement(ElementName = "item")]
		[DataMember(Name = "item")]
		public string Item { get; set; }

		[XmlElement(ElementName = "borrower")]
		[DataMember(Name = "borrower")]
		public string Borrower { get; set; }

		[XmlElement(ElementName = "note")]
		[DataMember(Name = "note")]
		public string Note { get; set; }

		[XmlElement(ElementName = "lendingDate")]
		[DataMember(Name = "lendingDate")]
		public DateTime? LendingDate { get; set; }

		[XmlElement(ElementName = "returnDate")]
		[DataMember(Name = "returnDate")]
		public DateTime? ReturnDate { get; set; }

		public LendingTransaction() : base()
		{
			Id = DefaultString;
			Item = DefaultString;
			Borrower = DefaultString;
			Note = DefaultString;
			LendingDate = DefaultDateTime;
			ReturnDate = DefaultDateTime;
		}

		public LendingTransaction(LendingTransaction other) : base(other)
		{
			Id = other.Id;
			Item = other.Item;
			Borrower = other.Borrower;
			Note = other.Note;
			LendingDate = other.LendingDate;
			ReturnDate = other.ReturnDate;
		}
	}
}
