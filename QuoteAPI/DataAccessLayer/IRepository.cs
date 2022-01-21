using System;
using System.Collections.Generic;
using Domain.Models.Quote;

namespace QuoteAPI.DataAccessLayer
{
    public interface IRepository
    {
        IEnumerable<Quote> GetQuotes();
        Quote GetQuote(Guid id);
        void Save(Quote quote);
        void DeleteQuote(Guid id);
    }
}