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
            AddSeededQuote();
        }

        private void AddSeededQuote()
        {
            // seeding some data, remove this eventually
            var guid = Guid.NewGuid();
            var seedItems = new List<Item>() {new Item(Guid.NewGuid(), "Seeded quote", 12.99)};
            var seedContact = new Contact("mark", "test@test.com");
            _quotes.Add(guid, new Quote(guid, seedItems, seedContact));
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