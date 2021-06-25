using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Domain.Events;
using Domain.Models.Quote;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace QuoteAPI
{
    [Route("api/quote")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private List<Quote> QuoteList { get; }
        private IEventBus _eventBus { get; }

        public QuoteController(IEnumerable<Quote> quoteList, IEventBus eventBus)
        {
            _eventBus = eventBus;
            QuoteList = quoteList.ToList();
        }

        public IEnumerable<Quote> GetQuotes()
        {
            return QuoteList;
        }

        public Quote GetQuote(Guid id)
        {
            return QuoteList.FirstOrDefault(x => x.GetQuoteId() == id);
        }

        public void AddItemToQuote(Guid quoteId, Item newItem)
        {
            var quote = GetQuote(quoteId);
            quote.AddQuoteItem(newItem);
        }

        [HttpPost("updatequoteitemprice/{quoteId}")]
        public void UpdateQuoteItemPrice(Guid quoteId, string quoteItemMessage, double newPrice)
        {
            //var quote = GetQuote(quoteId);
            var quote = new Quote(quoteId, new List<Item>());
            quote.UpdatePriceOnQuoteItem(quoteItemMessage, newPrice);
        }

        [HttpPost("sendquote/{quoteId}")]
        public async Task SendQuote(Guid quoteId)
        {
            // validate quote is a real quote, log if not
            //var quote = GetQuote(quoteId);
            var quote = new Quote(quoteId, new List<Item>());
            await _eventBus.Publish(new QuoteSent(quote, "example@example.com"));
        }
    }

    public interface IEventBus
    {
        Task Publish(QuoteSent quoteSent);
    }

    class EventBus : IEventBus
    {
        public async Task Publish(QuoteSent quoteSent)
        {
            // publish message to SQS
            var client = new AmazonSQSClient(RegionEndpoint.APSoutheast2);
            var request = new SendMessageRequest("https://sqs.ap-southeast-2.amazonaws.com/534833720216/QuoteAPI-Email", JsonConvert.SerializeObject(quoteSent));
            await client.SendMessageAsync(request);
        }
    }
}