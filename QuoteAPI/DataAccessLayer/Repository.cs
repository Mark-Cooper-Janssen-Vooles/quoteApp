using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models.Quote;

namespace QuoteAPI.DataAccessLayer
{
    public class Repository : IRepository
    {
        private readonly Dictionary<Guid, Quote> _quotes;

        public Repository() // every time i hit the endpoints it comes here??? just want to inject this once and have it in memory
        {
            _quotes = new Dictionary<Guid, Quote>();
            AddSeededQuote(); // seeding some data, remove this eventually
        }

        public Repository(List<Quote> quotes)
        {
            _quotes =  quotes.ToDictionary(x => x.Id);;
        }

        private void AddSeededQuote()
        {
            var guid = Guid.NewGuid();
            var seedItems = new List<Item>() {new Item(Guid.NewGuid(), "Seeded quote", 12.99)};
            var seedContact = new Contact("mark", "test@test.com");
            _quotes.Add(guid, new Quote(guid, seedItems, seedContact));
        }

        public IEnumerable<Quote> GetQuotes()
        {
            return _quotes.Select(pair => pair.Value);
        }

        public void CreateQuote(Contact contact)
        {
            var guid = Guid.NewGuid();
            _quotes.Add(guid, new Quote(guid, new List<Item>(), contact));
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