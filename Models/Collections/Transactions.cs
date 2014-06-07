using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace TeoLib.Models.Collections
{
	[Serializable, XmlRoot(ElementName = "transactions"), DataContract(Name="transactions")]
	public class Transactions : BaseModel
	{
		//[XmlElement]
		[DataMember]
		public List<LendingTransaction> LendingTransactions { get; set; }

		public override bool IsModified
		{
			get
			{
				if (null != LendingTransactions && 0 < LendingTransactions.Count)
				{
					_isModified |= 0 < (from transaction in LendingTransactions where true == transaction.IsModified select transaction.Id).Count();
				}
//#if DEBUG
//                _isModified = true;
//#endif
				return _isModified;
			}
		}

		public Transactions() : base()
		{
			LendingTransactions = new List<LendingTransaction>();
		}

		public Transactions(Transactions other) : base(other)
		{
			if (null == other.LendingTransactions)
				LendingTransactions = null;
			else
				LendingTransactions.AddRange(other.LendingTransactions);
		}
	}
}
