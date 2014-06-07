using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace TeoLib.Models
{
	[Serializable, DataContract(Name = "item")]
	public class Item : BaseModel, IEquatable<Item>
	{
		[XmlAttribute(AttributeName = "id")]
		[DataMember(Name = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "type")]
		[DataMember(Name = "type")]
		public string Type { get; set; }

		[XmlElement(ElementName = "title")]
		[DataMember(Name = "title")]
		public string Title { get; set; }

		[XmlElement(ElementName = "note")]
		[DataMember(Name = "note")]
		public string Note { get; set; }

		[XmlElement(ElementName = "quality")]
		[DataMember(Name = "quality")]
		public int? Quality { get; set; }

		public Item() : base()
		{
			Id = DefaultString;
			Type = DefaultString;
			Title = DefaultString;
			Note = DefaultString;
			Quality = DefaultInt;
		}

		public Item(Item other) : base(other)
		{
			Id = other.Id;
			Type = other.Type;
			Title = other.Title;
			Note = other.Note;
			Quality = other.Quality;
		}

		#region IEquatable
		public bool Equals(Item other)
		{
			if (null == other)
				return false;

			return Id == other.Id;
		}

		public override bool Equals(object other)
		{
			if (null != other)
				return false;

			return Equals((Item)other);
		}

		public override int GetHashCode()
		{
			return null != Id ? Id.GetHashCode() : 0;
		}
		#endregion
	}
}
