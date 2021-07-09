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
using QuoteAPI.DataAccessLayer;

namespace QuoteAPI
{
    [Route("api/quote")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IEventBus _eventBus;

        public QuoteController(IRepository repository, IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public IEnumerable<Quote> GetQuotes()
        {
            return _repository.GetQuotes();
        }

        public Quote GetQuote(Guid id)
        {
            return _repository.GetQuote(id);
        }

        public void AddItemToQuote(Guid id, Item newItem)
        {
            var quote = _repository.GetQuote(id);
            quote.AddQuoteItem(newItem);
            _repository.Save(quote);
        }

        [HttpPost("updatequoteitemprice/{quoteId}")]
        public void UpdateQuoteItemPrice(Guid quoteId, string quoteItemMessage, double newPrice)
        {
            //var quote = GetQuote(quoteId);
            var quote = new Quote(quoteId, new List<Item>(), new Contact("test", "example@example.com"));
            quote.UpdatePriceOnQuoteItem(quoteItemMessage, newPrice);
        }

        [HttpPost("sendquote/{quoteId}")]
        public async Task SendQuote(Guid quoteId)
        {
            // validate quote is a real quote, log if not
            //var quote = GetQuote(quoteId);
            var quote = new Quote(quoteId, new List<Item>{ new Item(new Guid(),"fixed bench", 20.00) }, new Contact("test", "example@example.com"));
            await _eventBus.Publish(new QuoteSent(quote, "example@example.com"));
        }

        [HttpPost("updateContact/{quoteId}")]
        public async Task UpdateContact(Guid quoteId)
        {
            var quote = new Quote( quoteId, new List<Item>(), new Contact("test", "example@example.com")); //get existing quote from db

            // quote stuff sent through headers
            quote.EditContact(new Contact("kevin rud", "kevin07@hotmail.com"));
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