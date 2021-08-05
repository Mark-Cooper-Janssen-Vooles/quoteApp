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

        // ENDPOINTS:
        // [DONE] /api/quote/quotes => GET all
        // /api/quote/newQuote => POST + create
        // /api/quote/{quoteId}/newDraftItem => POST + create
        // /api/quote/{quoteId}/updateDraftItem/{draftItemId} => PUT + update
        // /api/quote/{quoteId}/finaliseAndSendItem/{draftItemId} => PUT + update (to non-draft)
        // /api/quote/{quoteId}/delete => DELETE quote
        // /api/quote/{quoteId}/{draftItemId}/delete => DELETE draft item

        // anything missing? yeah... editing quote?

        // ======

        // /api/quote/quotes => GET all
        [HttpGet("quotes")]
        public ActionResult<IEnumerable<Quote>> GetQuotes()
        {
            var json = JsonConvert.SerializeObject(_repository.GetQuotes());

            return Ok(json);
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

        public void AddItemToQuote(Guid id, Item newItem)
        {
            var quote = _repository.GetQuote(id);
            quote.AddQuoteItem(newItem);
            _repository.Save(quote);
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