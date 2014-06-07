using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Text;

namespace TeoLib.Models
{
	[Serializable, DataContract(Name = "person")]
	public class Person : BaseModel, IEquatable<Person>
	{
		[XmlAttribute(AttributeName = "id")]
		[DataMember(Name = "id")]
		public string Id { get; set; }

		[XmlElement(ElementName = "name")]
		[DataMember(Name = "name")]
		public string Name { get; set; }

		[XmlElement(ElementName = "phone")]
		[DataMember(Name = "phone")]
		public string Phone { get; set; }

		[XmlElement(ElementName = "email")]
		[DataMember(Name = "email")]
		public string Email { get; set; }

		[XmlElement(ElementName = "note")]
		[DataMember(Name = "note")]
		public string Note { get; set; }

		public Person() : base()
		{
			Id = DefaultString;
			Name = DefaultString;
			Email = DefaultString;
			Phone = DefaultString;
			Note = DefaultString;
		}

		public Person(Person other) : base(other)
		{
			Id = other.Id;
			Name = other.Name;
			Email = other.Email;
			Phone = other.Phone;
			Note = other.Note;
		}

		#region IEquatable
		public bool Equals(Person other)
		{
			if (null == other)
				return false;

			return Id == other.Id;
		}

		public override bool Equals(object other)
		{
			if (null != other)
				return false;

			return Equals((Person)other);
		}

		public override int GetHashCode()
		{
			return null != Id ? Id.GetHashCode() : 0;
		}
		#endregion
	}
}
