using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Domain.Models.Quote
{
    public class Quote
    {
        public Contact Contact { get; }
        public Guid Id { get; }
        public List<Item> Items { get; }
        public List<Item> DraftItems { get; }

        public Quote(Guid id, List<Item> items, Contact contact)
        {
            Items = items;
            Contact = contact;
            Id = id;
            DraftItems = new List<Item>();
        }

        public void EditContact(Contact newContact)
        {
            Contact.Name = newContact.Name;
            Contact.Email = newContact.Email;
        }

        public bool AddQuoteItem(Item newItem)
        {
            if (DraftItems.Exists(x => x.Message == newItem.Message))
                return false;

            DraftItems.Add(newItem);
            return true;
        }

        public void UpdateDraftQuoteItemPrice(Item newItem)
        {
            var quoteItem = DraftItems.FirstOrDefault(x => x.Message == newItem.Message);
            quoteItem?.UpdatePrice(newItem.Price);
            quoteItem?.UpdateMessage(newItem.Message);
        }

        public bool HasDraftItems()
        {
            return DraftItems.Any();
        }

        public bool HasFinalisedItems()
        {
            return Items.Any();
        }

        public IEnumerable<Item> GetAllItems()
        {
            return DraftItems.Concat(Items).ToList();
        }

        public Guid GetQuoteId()
        {
            return Id;
        }

        public void FinaliseDraftItem(Guid itemId)
        {
            if (!DraftItems.Exists(x => x.Id == itemId))
                return;

            var item = DraftItems[DraftItems.FindIndex(x => x.Id == itemId)];
            Items.Add(item);
            DraftItems.RemoveAll(x => x.Id == itemId);
        }

        public void Finalise()
        {
            throw new NotImplementedException();
        }
    }
}