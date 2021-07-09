using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models.Quote;

namespace QuoteAPI.DataAccessLayer
{
    public class Repository : IRepository
    {
        private readonly Dictionary<Guid, Quote> _quotes;

        public Repository()
        {
            _quotes = new Dictionary<Guid, Quote>();
        }

        public Repository(List<Quote> quotes)
        {
            _quotes =  quotes.ToDictionary(x => x.Id);;
        }

        public IEnumerable<Quote> GetQuotes()
        {
            return _quotes.Select(pair => pair.Value);
        }

        public Quote GetQuote(Guid id)
        {
            try
            {
                return _quotes[id];
            }
            catch
            {
                return new Quote(Guid.Empty, new List<Item>(), new Contact("", ""));
            }
        }

        public void Save(Quote quote)
        {
            _quotes[quote.Id] = quote;
        }
    }
}