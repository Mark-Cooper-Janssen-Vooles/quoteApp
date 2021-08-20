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
            var guid = new Guid("5233fecd-1320-4916-a5ba-f2c829d19e63");
            var seedItems = new List<Item>() {new Item("Seeded quote", 12.99)};
            var seedContact = new Contact("mark", "test@test.com");
            var quote = new Quote(guid, seedItems, seedContact);
            _quotes.Add(guid,quote);
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