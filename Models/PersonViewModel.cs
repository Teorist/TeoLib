using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Text;

namespace TeoLib.Models
{
	[Serializable, DataContract(Name="person")]
	public class PersonViewModel : Person, IEquatable<Person>, IEquatable<PersonViewModel>
	{
		public int? BorrowedItemsCount { get; set; }
		public bool IsLate { get; set; }

		public PersonViewModel() : base()
		{
			BorrowedItemsCount = DefaultInt;
			IsLate = false;
		}

		public PersonViewModel(Person other) : base(other)
		{
			BorrowedItemsCount = DefaultInt;
			IsLate = false;
		}

		public PersonViewModel(PersonViewModel other) : base(other)
		{
			BorrowedItemsCount = other.BorrowedItemsCount;
			IsLate = other.IsLate;
		}

		#region IEquatable
		public new bool Equals(Person other)
		{
			return base.Equals(other);
		}

		public bool Equals(PersonViewModel other)
		{
			return base.Equals((Person)other);
		}

		public override bool Equals(object other)
		{
			return base.Equals(other);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion
	}
}
