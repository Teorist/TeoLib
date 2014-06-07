using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeoLib.Models;
using TeoLib.Models.Collections;
using TeoLib.Utils;

namespace TeoLib.Services
{
	public class LibraryService : BaseService
	{
		public LibraryService() : base()
		{
		}

		public Library GetLibrary()
		{
			DebugWrite("LibraryService.GetLibrary()");

			return Repository.GetLibrary();
		}

		public Error SaveLibrary(Library library)
		{
			DebugWrite("LibraryService.SaveLibrary(...)");

			if (null != library)
				return library.IsModified ? Repository.SaveLibrary(library) : Error.None;
			else
				return Error.EmptyArgument;
		}

		public Error UpdateItems(Library library, ItemViewModel item)
		{
			Error error = Error.None;

			DebugWrite("LibraryService.UpdateItems(...)");

			if (null == library || null == item)
			{
				error = Error.EmptyArgument;
			}
			else if (item.IsModified)
			{
				if (string.IsNullOrWhiteSpace(item.Id))
				{
					item.Id = Guid.NewGuid().ToString();

					if (0 < (from items in library.Items where items.Id == item.Id select items.Id).Count())
						error = Error.CollectionItemAlreadyExists;
					else
					{
						bool itemAdded = false;

						for (int i = 0; (i < library.Items.Count) && !itemAdded; i++)
						{
							if (0 > string.Compare(item.Title, library.Items[i].Title, true))
							{
								library.Items.Insert(i, item);
								itemAdded = true;
							}
							else if (0 == string.Compare(item.Title, library.Items[i].Title, true)
									 && 0 > string.Compare(item.Type, library.Items[i].Type, true))
							{
								library.Items.Insert(i, item);
								itemAdded = true;
							}
						}

						if (!itemAdded)
							library.Items.Add(item);
					}
				}
				else
				{
					foreach (ItemViewModel currentItem in library.Items)
					{
						if (item.Id == currentItem.Id)
						{
							currentItem.Type = item.Type;
							currentItem.Title = item.Title;
							currentItem.Quality = item.Quality;
							currentItem.Status = item.Status;
							currentItem.Note = item.Note;
							currentItem.IsModified = item.IsModified;
						}
					}
				}
			}

			return error;
		}

		public ItemViewModel GetLibraryItem(Library library, string itemId)
		{
			DebugWrite("PeopleService.GetLibraryItem(...)");

			ItemViewModel item = null;

			if (null != library && !string.IsNullOrWhiteSpace(itemId))
				item = (from items in library.Items
						where items.Id == itemId
						select items).FirstOrDefault();

			return item;
		}

		public Error UpdateItemStatus(ItemViewModel item, LendingTransaction transaction)
		{
			DebugWrite("PeopleService.UpdateItemStatus(...)");

			Error error = Error.None;

			if (null != item)
			{
				if (null == transaction)
					item.Status = string.Empty;
				//else if (!transaction.ReturnDate.HasValue && transaction.ExpectedDate.HasValue && DateTime.Now > transaction.ExpectedDate.Value)
				//    item.Status = "late";
				else if (!transaction.ReturnDate.HasValue)
					item.Status = "out";
				else
					item.Status = "in";
			}

			return error;
		}

		public Error UpdateLibraryStatus(Library library, Transactions transactions)
		{
			DebugWrite("LibraryService.UpdateItemsStatus(...)");

			Error error = Error.None;

			if (null != library && null != transactions)
			{
				// Out items
				List<string> items = (from transaction in transactions.LendingTransactions
										 where !transaction.ReturnDate.HasValue
										 select transaction.Item).ToList();

				if (null != items)
				{
					foreach (string itemId in items)
					{
						GetLibraryItem(library, itemId).Status = "out";
					}
				}

				// In items
				items = (from transaction in transactions.LendingTransactions
						 where transaction.ReturnDate.HasValue
						 select transaction.Item).ToList();

				if (null != items)
				{
					foreach (string itemId in items)
					{
						GetLibraryItem(library, itemId).Status = "in";
					}
				}
			}

			return error;
		}

		public Error SortLibrary(Library library)
		{
			DebugWrite("LibraryService.SortLibrary(...)");

			Error error = Error.None;

			if (null != library && null != library.Items)
			{
				List<ItemViewModel> items = library.Items.OrderBy(x => x.Title).ThenBy(x=>x.Type).ToList();
				library.Items = items;
			}

			return error;
		}
	}
}
