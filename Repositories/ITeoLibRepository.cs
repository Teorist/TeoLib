using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeoLib.Models;
using TeoLib.Models.Collections;
using TeoLib.Utils;

namespace TeoLib.Repositories
{
	public interface ITeoLibRepository : IDisposable
	{
		DebugWrite DebugWriteLine { get; set; }

		Library GetLibrary();
		Error SaveLibrary(Library library);

		People GetPeople();
		Error SavePeople(People people);

		Transactions GetTransactions();
		Error SaveTransactions(Transactions transactions);
	}
}
