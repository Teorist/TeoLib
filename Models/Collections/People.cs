using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Text;

namespace TeoLib.Models.Collections
{
	[Serializable, XmlRoot(ElementName="people"), DataContract]
	public class People : BaseModel
	{
		//[XmlElement]
		[DataMember]
		public List<PersonViewModel> Persons { get; set; }

		public override bool IsModified
		{
			get
			{
				if (null != Persons && 0 < Persons.Count)
				{
					_isModified |= 0 < (from person in Persons where true == person.IsModified select person.Id).Count();
				}
//#if DEBUG
//                _isModified = true;
//#endif
				return _isModified;
			}
		}

		public People() : base()
		{
			Persons = new List<PersonViewModel>();
		}

		public People(People other) : base(other)
		{
			if (null == other.Persons)
				Persons = null;
			else
				Persons.AddRange(other.Persons);
		}
	}
}
