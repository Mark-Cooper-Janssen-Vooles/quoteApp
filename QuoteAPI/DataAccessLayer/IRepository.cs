using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models.Quote;

namespace QuoteAPI.DataAccessLayer
{
    public interface IRepository
    {
        Task<IEnumerable<Quote>> GetQuotes();
        // Quote GetQuote(Guid id);
        void Save(Quote quote);
        // void DeleteQuote(Guid id);
    }
}