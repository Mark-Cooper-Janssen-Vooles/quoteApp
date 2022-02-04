using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using Domain.Models.Quote;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetQuotes()
        {
            return new OkObjectResult(await _repository.GetQuotes());
            // return a response object instead so if the domain is changed the receiving API doesn't get the wrong data (compile time error vs runtime)
        }

        // /api/quote/quotes
        [HttpPost("quotes")]
        public async void CreateQuote(Contact contactRequest)
        {
            var quote = new Quote(Guid.NewGuid(), new List<Item>(), contactRequest);
            _repository.Save(quote);
        }

        // api/quote/quotes/{id}/draft-item
        [HttpPost("quotes/{id}/draft-item")]
        public void AddItemToQuote(Guid id, ItemRequestDTO item)
        {
            var newItem = new Item(item.Message, item.Price);

            var quote = _repository.GetQuote(id).Result;
            // _repository.AddQuoteItem(quote);

            quote.AddQuoteItem(newItem);
            _repository.Save(quote);
        }

        // [HttpPut("quotes/{id}/draft-item/")]
        // public void UpdateDraftQuoteItem(Guid id, Item updatedItem)
        // {
        //     var quote = _repository.GetQuote(id);
        //
        //     quote.UpdateDraftQuoteItemPrice(updatedItem);
        //     _repository.Save(quote);
        // }
        //
        // [HttpPut("quotes/{quoteId}/draft-item/{itemId}/finalise")]
        // public async void FinaliseAndSendQuote(Guid quoteId, Guid itemId)
        // {
        //     var quote = _repository.GetQuote(quoteId);
        //     var draftItem = quote.GetDraftItem(itemId);
        //
        //     if (quote.Id == Guid.Empty || draftItem.Id == Guid.Empty) return;
        //     quote.FinaliseDraftItem(itemId);
        //
        //     await _eventBus.Publish(new QuoteSent(quote, quote.Contact.Email));
        // }
        //
        // [HttpDelete("quotes/{quoteId}/")]
        // public IActionResult DeleteQuote(Guid quoteId)
        // {
        //     _repository.DeleteQuote(quoteId);
        //
        //     return new StatusCodeResult(200);
        // }
        //
        // [HttpDelete("quotes/{quoteId}/draft-item/{itemId}")]
        // public IActionResult DeleteDraftItem(Guid quoteId, Guid itemId)
        // {
        //     var quote = _repository.GetQuote(quoteId);
        //     quote.DeleteDraftItem(itemId);
        //
        //     return new StatusCodeResult(200);
        // }


        // public Quote GetQuote(Guid id)
        // {
        //     return _repository.GetQuote(id);
        // }
    }
}