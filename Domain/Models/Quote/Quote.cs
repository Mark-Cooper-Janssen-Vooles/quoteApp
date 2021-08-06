using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Domain.Models.Quote
{
    [DataContract]
    public class Quote
    {
        [DataMember]
        public Contact Contact { get; }
        [DataMember]
        public Guid Id { get; }
        [DataMember]
        private List<Item> Items { get; }
        [DataMember]
        private List<Item> DraftItems { get; }

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

        public void UpdatePriceOnQuoteItem(string quoteItemMessage, double newPrice)
        {
            var quoteItem = Items.FirstOrDefault(x => x.Message == quoteItemMessage);
            quoteItem?.UpdatePrice(newPrice);
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

        public void FinaliseDraftItem(Item item)
        {
            if (!DraftItems.Exists(x => x.Message == item.Message))
                return;

            DraftItems.RemoveAll(x => x.Message == item.Message);
            Items.Add(item);
        }

        public void Finalise()
        {
            throw new NotImplementedException();
        }
    }
}