using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeoLib.Models;
using TeoLib.Models.Collections;
using TeoLib.Utils;

namespace TeoLib.Services
{
	public class PeopleService : BaseService
	{
		public PeopleService() : base()
		{
		}

		public People GetPeople()
		{
			DebugWrite("PeopleService.GetPeople()");

			return Repository.GetPeople();
		}

		public Error SavePeople(People people)
		{
			DebugWrite("PeopleService.SavePeople(...)");

			if (null != people)
				return people.IsModified ? Repository.SavePeople(people) : Error.None;
			else
				return Error.EmptyArgument;
		}

		public Error UpdatePeople(People people, PersonViewModel person)
		{
			Error error = Error.None;

			DebugWrite("PeopleService.UpdatePeople(...)");

			if (null == people || null == person)
			{
				error = Error.EmptyArgument;
			}
			else if (person.IsModified)
			{
				if (string.IsNullOrWhiteSpace(person.Id))
				{
					person.Id = Guid.NewGuid().ToString();

					if (0 < (from persons in people.Persons where persons.Id == person.Id select persons.Id).Count())
						error = Error.CollectionItemAlreadyExists;
					else
					{
						bool personAdded = false;

						for (int i = 0; (i < people.Persons.Count) && !personAdded; i++)
						{
							if (0 > string.Compare(person.Name, people.Persons[i].Name, true))
							{
								people.Persons.Insert(i, person);
								personAdded = true;
							}
						}

						if (!personAdded)
							people.Persons.Add(person);
					}
				}
				else
				{
					foreach (PersonViewModel currentPerson in people.Persons)
					{
						if (person.Id == currentPerson.Id)
						{
							currentPerson.Name = person.Name;
							currentPerson.Phone = person.Phone;
							currentPerson.Email = person.Email;
							currentPerson.Note = person.Note;
							currentPerson.IsModified = person.IsModified;
						}
					}
				}
			}

			return error;
		}

		public PersonViewModel GetPeoplePerson(People people, string personId)
		{
			DebugWrite("PeopleService.GetPeoplePerson(...)");

			PersonViewModel person = null;

			if (null != people && !string.IsNullOrWhiteSpace(personId))
				person = (from persons in people.Persons
						  where persons.Id == personId
						  select persons).FirstOrDefault();

			return person;
		}

		public Error UpdatePersonStatus(PersonViewModel person, LendingTransaction transaction)
		{
			DebugWrite("PeopleService.UpdatePersonStatus(...)");

			Error error = Error.None;

			if (null != person)
			{
				if (null != transaction)
				{
					if (!transaction.ReturnDate.HasValue)
						person.BorrowedItemsCount += 1;
					else
						person.BorrowedItemsCount = person.BorrowedItemsCount.HasValue ? (int?)(person.BorrowedItemsCount.Value - 1) : null;

					//if (!transaction.ReturnDate.HasValue && transaction.ExpectedDate.HasValue && DateTime.Now > transaction.ExpectedDate.Value)
					//    person.IsLate = true;
				}
			}

			return error;
		}

		public Error UpdatePeopleStatus(People people, Transactions transactions)
		{
			DebugWrite("PeopleService.UpdatePeopleStatus(...)");

			Error error = Error.None;

			if (null != people && null != people.Persons && null != transactions)
			{
				foreach (PersonViewModel person in people.Persons)
				{
					List<string> outItems = (from transaction in transactions.LendingTransactions
											 where transaction.Borrower == person.Id && !transaction.ReturnDate.HasValue
											 select transaction.Item).ToList();

					person.BorrowedItemsCount = outItems.Count;
					//GetPeoplePerson(people, borrower).BorrowedItemsCount = outItems.Count;
				}
			}

			return error;
		}

		public Error SortPeople(People people)
		{
			DebugWrite("PeopleService.SortPeople(...)");

			Error error = Error.None;

			if (null != people && null != people.Persons)
			{
				List<PersonViewModel> persons = people.Persons.OrderBy(x=>x.Name).ToList();
				people.Persons = persons;
			}

			return error;
		}
	}
}
