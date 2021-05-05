using System;
using System.Collections.Generic;
using System.Linq;
using QuoteAPI.Models;

namespace QuoteAPI
{
    public class QuoteController
    {
        private List<Quote> QuoteList { get; }

        public QuoteController(IEnumerable<Quote> quoteList)
        {
            QuoteList = quoteList.ToList();
        }

        public IEnumerable<Quote> GetQuotes()
        {
            return QuoteList;
        }

        public Quote GetQuote(Guid id)
        {
            return QuoteList.FirstOrDefault(x => x.Id == id);
        }

        public void AddItemToQuote(Guid quoteId, QuoteItem newQuoteItem)
        {
            var quote = GetQuote(quoteId);
            quote.AddQuoteItem(newQuoteItem);
        }

        public void UpdateQuoteItemPrice(Guid quoteId, string quoteItemMessage, double newPrice)
        {
            var quote = GetQuote(quoteId);
            quote.UpdatePriceOnQuoteItem(quoteItemMessage, newPrice);
        }
    }
}