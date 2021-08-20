using System;
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
using QuoteAPI.DTOs;

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
        public IActionResult GetQuotes()
        {
            return new OkObjectResult(_repository.GetQuotes());
            // return a response object instead so if the domain is changed the receiving API doesn't get the wrong data (compile time error vs runtime)
        }

        // /api/quote/quotes
        [HttpPost("quotes")]
        public void CreateQuote(Contact contactRequest)
        {
            _repository.CreateQuote(contactRequest);
        }

        // api/quote/quotes/{id}/draft-item
        [HttpPost("quotes/{id}/draft-item")]
        public void AddItemToQuote(Guid id, ItemRequestDTO item)
        {
            var newItem = new Item(item.Message, item.Price);

            var quote = _repository.GetQuote(id);
            quote.AddQuoteItem(newItem);
            _repository.Save(quote);
        }

        [HttpPut("quotes/{id}/draft-item/")]
        public void UpdateDraftQuoteItem(Guid id, Item updatedItem)
        {
            var quote = _repository.GetQuote(id);

            quote.UpdateDraftQuoteItemPrice(updatedItem);
            _repository.Save(quote);
        }

        [HttpPut("quotes/{quoteId}/draft-item/{itemId}/finalise")]
        public async void FinaliseAndSendQuote(Guid quoteId, Guid itemId)
        {
            var quote = _repository.GetQuote(quoteId);

            if (quote.Id == Guid.Empty) return;

            // await _eventBus.Publish(new QuoteSent(quote, quote.Contact.Email)); // <=== get this working!
            quote.FinaliseDraftItem(itemId);
        }

        [HttpDelete("quotes/{quoteId}/")]
        public void DeleteQuote(Guid quoteId)
        {
            _repository.DeleteQuote(quoteId);
        }

        [HttpDelete("quotes/{quoteId}/draft-item/{itemId}")]
        public void DeleteDraftItem(Guid quoteId, Guid itemId)
        {
            var quote = _repository.GetQuote(quoteId);
            quote.DeleteDraftItem(itemId);
        }

        // don't have button / form hooked up in the UI for this endpoint yet
        // [HttpPost("updateContact/{quoteId}")]
        // public void UpdateContact(Guid id, Contact contact)
        // {
        //     var quote = _repository.GetQuote(id);
        //     quote.EditContact(contact);
        //     _repository.Save(quote);
        // }

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