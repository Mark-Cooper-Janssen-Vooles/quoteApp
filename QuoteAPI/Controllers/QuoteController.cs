using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Events;
using Domain.Models.Quote;
using MediatR;

namespace QuoteAPI
{
    public class QuoteController
    {
        private List<Quote> QuoteList { get; }
        private IEventBus _eventBus { get; }

        public QuoteController(IEnumerable<Quote> quoteList, IEventBus eventBus)
        {
            _eventBus = eventBus;
            QuoteList = quoteList.ToList();
        }

        public IEnumerable<Quote> GetQuotes()
        {
            return QuoteList;
        }

        public Quote GetQuote(Guid id)
        {
            return QuoteList.FirstOrDefault(x => x.GetQuoteId() == id);
        }

        public void AddItemToQuote(Guid quoteId, Item newItem)
        {
            var quote = GetQuote(quoteId);
            quote.AddQuoteItem(newItem);
        }

        public void UpdateQuoteItemPrice(Guid quoteId, string quoteItemMessage, double newPrice)
        {
            var quote = GetQuote(quoteId);
            quote.UpdatePriceOnQuoteItem(quoteItemMessage, newPrice);
        }

        public async Task SendQuote(Guid quoteId)
        {
            var quote = GetQuote(quoteId);
            await _eventBus.Publish(new QuoteSent(quote, "example@example.com"));
        }
    }

    public interface IEventBus
    {
        Task Publish(QuoteSent quoteSent);
    }
}