using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace TeoLib.Models
{
	[Serializable, DataContract(Name="item")]
	public class ItemViewModel : Item, IEquatable<ItemViewModel>, IEquatable<Item>
	{
		public string Status { get; set; }

		public ItemViewModel() : base()
		{
			Status = DefaultString;
		}

		public ItemViewModel(Item other): base(other)
		{
			Status = DefaultString;
		}

		public ItemViewModel(ItemViewModel other) : base(other)
		{
			Status = other.Status;
		}

		#region IEquatable
		public new bool Equals(Item other)
		{
			return base.Equals(other);
		}

		public bool Equals(ItemViewModel other)
		{
			return base.Equals((Item)other);
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
