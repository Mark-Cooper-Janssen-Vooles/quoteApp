using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models.Quote
{
    public class Quote
    {
        private Guid Id { get; }
        private List<Item> Items { get; }
        private List<Item> DraftItems { get; }

        public Quote(Guid id, List<Item> items)
        {
            Items = items;
            Id = id;
            DraftItems = new List<Item>();
        }

        public void AddQuoteItem(Item newItem)
        {
            if (DraftItems.Exists(x => x.Message == newItem.Message))
                return;

            DraftItems.Add(newItem);
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