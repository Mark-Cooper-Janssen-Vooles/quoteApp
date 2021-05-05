using System;
using System.Collections.Generic;
using System.Linq;
using QuoteAPI.Models;

namespace QuoteAPI
{
    public class Quote
    {
        public Guid Id { get; }
        public List<QuoteItem> Items { get; }

        public Quote(Guid id, List<QuoteItem> items)
        {
            Items = items;
            Id = id;
        }

        public void AddQuoteItem(QuoteItem newQuoteItem)
        {
            if (Items.Exists(x => x.Message == newQuoteItem.Message))
                return;

            Items.Add(newQuoteItem);
        }

        public void UpdatePriceOnQuoteItem(string quoteItemMessage, double newPrice)
        {
            var quoteItem = Items.FirstOrDefault(x => x.Message == quoteItemMessage);
            quoteItem?.UpdatePrice(newPrice);
        }
    }
}