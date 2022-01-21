using System;
using System.Collections;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Domain.Models.Quote;
using QuoteAPI.DataAccessLayer;
using Xunit;

namespace QuoteAPI.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public void GivenNoQuotesExist_WhenGetQuotes_ReturnsEmptyList()
        {
            var repo = new QuoteRepository();
            IEnumerable quotes = repo.GetQuotes();
            Assert.Empty(quotes);
        }
        
        [Fact]
        public void GivenQuotesExist_WhenGetQuotes_ReturnsQuotes()
        {
            var fakeDynamo = new FakeDynamo();
            var repo = new QuoteRepository();
            var quote = new Quote(Guid.NewGuid(), new List<Item>(), new Contact("Matt", "matt.birman@xero.com")); 
            repo.Save(quote);
            
            var expected = new List<Quote>()
            {
                quote
            };
            
            IEnumerable quotes = repo.GetQuotes();
            Assert.Equal(expected, quotes);
        }
    }

    public class FakeDynamo : IDynamoDbClient
    {
        private IDictionary<Guid, Quote> _quotes;

        public FakeDynamo(IDictionary<Guid, Quote> quotes)
        {
            _quotes = quotes;
        }

        public IEnumerable<Quote> GetItem(GetItemRequest request)
        {
            throw new NotImplementedException();
        }

        public QueryResponse Query(QueryRequest request)
        {
            return new QueryResponse()
            {
                Items = new List<Dictionary<string, AttributeValue>>
                {
                    {}
                }
            };
        }
    }
}