using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using Domain.Models.Quote;
using Newtonsoft.Json;

namespace QuoteAPI.DataAccessLayer
{
    public class QuoteRepository : IRepository
    {
        private readonly IDynamoDbClient _dynamoDbClient;

        public QuoteRepository(IDynamoDbClient dynamoDbClient) // every time i hit the endpoints it comes here??? just want to inject this once and have it in memory
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public IEnumerable<Quote> GetQuotes()
        {
            var request = new QueryRequest
            {
                TableName = "quoteDB3",
                KeyConditionExpression = "type = :v_type",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":v_type", new AttributeValue { S ="quote" }}
                },
                Limit = 10
            };
            var queryResponse = _dynamoDbClient.Query(request);
            var quotes = queryResponse.Items.Select(x => JsonConvert.DeserializeObject<Quote>(x["entity"].S));
            return quotes;
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

        public void DeleteQuote(Guid id)
        {
            _quotes.Remove(id);
        }
    }

    public interface IDynamoDbClient
    {
        IEnumerable<Quote> GetItem(GetItemRequest request);
        QueryResponse Query(QueryRequest request);
    }
}