using System;
using System.Collections.Generic;
using Domain.Models.Quote;

namespace QuoteAPI.DataAccessLayer
{
    public interface IRepository
    {
        IEnumerable<Quote> GetQuotes();
        void CreateQuote(Contact contact);
        Quote GetQuote(Guid id);
        void Save(Quote quote);
    }
}