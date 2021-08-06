using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Domain.Events;
using Domain.Models.Quote;
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

        // /api/quote/quotes => GET all
        [HttpGet("quotes")]
        public ActionResult<IEnumerable<Quote>> GetQuotes()
        {
            var json = JsonConvert.SerializeObject(_repository.GetQuotes());

            return Ok(json);
        }

        // /api/quote/quotes
        [HttpPost("quotes")]
        public async Task<ActionResult> CreateQuote()
        {
            Contact contact;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                string rawValue = await reader.ReadToEndAsync();
                contact = JsonConvert.DeserializeObject<Contact>(rawValue);
            }

            _repository.CreateQuote(contact);

            return StatusCode(201);
        }

        // api/quote/quotes/{id}/draft-item
        [HttpPost("quotes/{id}/draft-item")]
        public async Task<ActionResult> AddItemToQuote(Guid id)
        {
            Item newItem;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                string rawValue = await reader.ReadToEndAsync();
                newItem = JsonConvert.DeserializeObject<Item>(rawValue);
            }

            var quote = _repository.GetQuote(id);
            quote.AddQuoteItem(newItem);
            _repository.Save(quote);

            return StatusCode(201);
        }

        [HttpPost("updateQuoteItemPrice/{quoteId}")]
        public void UpdateQuoteItemPrice(Guid id, string quoteItemMessage, double newPrice)
        {
            var quote = _repository.GetQuote(id);
            quote.UpdatePriceOnQuoteItem(quoteItemMessage, newPrice);
            _repository.Save(quote);
        }

        [HttpPost("sendQuote/{quoteId}")]
        public async Task SendQuote(Guid id)
        {
            var quote = _repository.GetQuote(id);
            if (quote.Id != Guid.Empty)
            {
                await _eventBus.Publish(new QuoteSent(quote, quote.Contact.Email));
            }
        }

        [HttpPost("updateContact/{quoteId}")]
        public void UpdateContact(Guid id, Contact contact)
        {
            var quote = _repository.GetQuote(id);
            quote.EditContact(contact);
            _repository.Save(quote);
        }

        public Quote GetQuote(Guid id)
        {
            return _repository.GetQuote(id);
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