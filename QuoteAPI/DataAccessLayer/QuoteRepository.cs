using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Domain.Models.Quote;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;

namespace QuoteAPI.DataAccessLayer
{
    public class QuoteRepository : IRepository
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public QuoteRepository(IAmazonDynamoDB dynamoDbClient) // every time i hit the endpoints it comes here??? just want to inject this once and have it in memory
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task<IEnumerable<Quote>> GetQuotes()
        {
            var request = new QueryRequest
            {
                TableName = "quoteDB4",
                KeyConditionExpression = "entityType = :v_type",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":v_type", new AttributeValue { S ="quote" }}
                },
                Limit = 10
            };
            var queryResponse = await _dynamoDbClient.QueryAsync(request);

            var quotes = queryResponse.Items.Select(x => JsonConvert.DeserializeObject<Quote>(x["entity"].S));
            return quotes;
        }

        // public Quote GetQuote(Guid id)
        // {
        //     try
        //     {
        //         return _quotes[id];
        //     }
        //     catch
        //     {
        //         return new Quote(Guid.Empty, new List<Item>(), new Contact("", ""));
        //     }
        // }
        //
        public async void Save(Quote quote)
        {
            var quoteString = JsonConvert.SerializeObject(quote);
            var request = new PutItemRequest
            {
                TableName = "quoteDB4",
                Item = new Dictionary<string, AttributeValue>()
                {
                    {"entityType", new AttributeValue { S = "quote" }},
                    {"entity", new AttributeValue { S = quoteString}},
                    {"id", new AttributeValue {S = JsonConvert.SerializeObject(Guid.NewGuid())}}
                }
            };

            await _dynamoDbClient.PutItemAsync(request);
        }

        // public void DeleteQuote(Guid id)
        // {
        //     _quotes.Remove(id);
        // }
    }
}