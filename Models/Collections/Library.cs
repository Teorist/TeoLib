using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace TeoLib.Models.Collections
{
	[Serializable, XmlRoot(ElementName="library"), DataContract]
	public class Library : BaseModel
	{
		//[XmlElement]
		[DataMember]
		public List<ItemViewModel> Items { get; set; }

		public override bool IsModified
		{
			get
			{
				if (null != Items && 0 < Items.Count)
				{
					_isModified |= 0 < (from item in Items where true == item.IsModified select item.Id).Count();
				}
//#if DEBUG
//                _isModified = true;
//#endif
				return _isModified;
			}
		}

		public Library() : base()
		{
			Items = new List<ItemViewModel>();
		}

		public Library(Library other) : base(other)
		{
			if (null == other.Items)
				Items = null;
			else
				Items.AddRange(other.Items);
		}
	}
}
