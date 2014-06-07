using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeoLib.Models;
using TeoLib.Models.Collections;
using TeoLib.Utils;

namespace TeoLib.Services
{
	public class TransactionService : BaseService
	{
		public TransactionService() : base()
		{
		}

		public Transactions GetTransactions()
		{
			DebugWrite("TransactionService.GetTransactions()");

			return Repository.GetTransactions();
		}

		public Error SaveTransactions(Transactions transactions)
		{
			DebugWrite("TransactionService.SaveTransactions(...)");

			if (null != transactions)
				return transactions.IsModified ? Repository.SaveTransactions(transactions) : Error.None;
			else
				return Error.EmptyArgument;
		}

		public Error UpdateLendingTransactions(Transactions transactions, LendingTransaction lendingTransaction)
		{
			Error error = Error.None;

			DebugWrite("TransactionService.UpdateLendingTransactions(...)");

			if (null == transactions || null == lendingTransaction)
			{
				error = Error.EmptyArgument;
			}
			else if (lendingTransaction.IsModified /*&&
					!string.IsNullOrWhiteSpace(lendingTransaction.Item) &&
				    !string.IsNullOrWhiteSpace(lendingTransaction.Borrower) &&
					lendingTransaction.LendingDate.HasValue*/)
			{
				if (string.IsNullOrWhiteSpace(lendingTransaction.Id))
				{
                    lendingTransaction.Id = Guid.NewGuid().ToString(); //string.Format("{0}_{1}_{2}", lendingTransaction.Item.Trim().ToLowerInvariant(), lendingTransaction.Borrower.Trim().ToLowerInvariant(), lendingTransaction.LendingDate.Value.ToString("yyyy-MM-dd")).Replace(' ', '_');
                    transactions.LendingTransactions.Add(lendingTransaction);

                    //if (0 < (from lendingTransactions in transactions.LendingTransactions where lendingTransactions.Id == lendingTransaction.Id select lendingTransactions.Id).Count())
                    //    error = Error.CollectionItemAlreadyExists;
                    //else
                    //    transactions.LendingTransactions.Add(lendingTransaction);
				}
				else
				{
					foreach (LendingTransaction currentTransaction in transactions.LendingTransactions)
					{
						if (lendingTransaction.Id == currentTransaction.Id)
						{
							currentTransaction.Borrower = lendingTransaction.Borrower;
							currentTransaction.Item = lendingTransaction.Item;
							currentTransaction.LendingDate = lendingTransaction.LendingDate;
							currentTransaction.ReturnDate = lendingTransaction.ReturnDate;
							currentTransaction.Note = lendingTransaction.Note;
							currentTransaction.IsModified = lendingTransaction.IsModified;
						}
					}
				}
			}

			return error;
		}

        public LendingTransaction GetItemCurrentTransaction(Transactions transactions, string itemId)
        {
			DebugWrite("TransactionService.GetItemCurrentTransaction(...)");

            LendingTransaction transaction = null;

            if (null != transactions && !string.IsNullOrWhiteSpace(itemId))
				transaction = (from trans in GetItemTransactions(transactions, itemId)
                               where !trans.ReturnDate.HasValue
                               select trans).FirstOrDefault();

            return transaction;
        }

		public IEnumerable<LendingTransaction> GetItemTransactions(Transactions transactions, string itemId)
		{
			DebugWrite("TransactionService.GetItemTransactions(...)");

			if (null == transactions || string.IsNullOrWhiteSpace(itemId))
				return null;

			return (from trans in transactions.LendingTransactions
							   where trans.Item == itemId
							   select trans);
		}

		public LendingTransaction GetPersonCurrentTransaction(Transactions transactions, string personId)
		{
			DebugWrite("TransactionService.GetPersonCurrentTransaction(...)");

			LendingTransaction transaction = null;

			if (null != transactions && !string.IsNullOrWhiteSpace(personId))
				transaction = (from trans in GetPersonTransactions(transactions, personId)
							   where !trans.ReturnDate.HasValue
							   select trans).FirstOrDefault();

			return transaction;
		}

		public IEnumerable<LendingTransaction> GetPersonTransactions(Transactions transactions, string personId)
		{
			DebugWrite("TransactionService.GetPersonTransactions(...)");

			if (null == transactions || string.IsNullOrWhiteSpace(personId))
				return null;

			return (from trans in transactions.LendingTransactions
					where trans.Borrower == personId
					select trans);
		}

		public Error ClearHistory(Transactions transactions)
		{
			Error error = Error.None;

			if (null != transactions)
			{
				List<LendingTransaction> currentTransOnly = (from trans in transactions.LendingTransactions
															 where !trans.ReturnDate.HasValue
															 select trans).ToList();

				transactions.LendingTransactions = currentTransOnly;
			}

			return error;
		}
	}
}
