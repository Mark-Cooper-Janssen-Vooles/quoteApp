using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeepEqual.Syntax;
using Domain.Events;
using Domain.Models.Quote;
using Microsoft.AspNetCore.DataProtection.Repositories;
using QuoteAPI.DataAccessLayer;
using Xunit;

namespace QuoteAPI.Tests
{
    public class FakeEventBus : IEventBus
    {
        public async Task Publish(QuoteSent quoteSent) { }
    }

    public class UnitTest1
    {
        [Fact]
        public void GivenTheSystemHasNoQuotes_WhenGettingQuotes_ThenWeSeeNoQuotesReturned()
        {
            //arrange
            var quoteRepository = new Repository();
            var controller = new QuoteController(quoteRepository, new FakeEventBus());

            //act
            var listOfQuotes = controller.GetQuotes();

            //assert
            Assert.Empty( listOfQuotes );
        }

        [Fact]
        public void GivenTheSystemHasQuotes_WhenGettingQuotes_ThenWeSeeQuotesReturned()
        {
            //arrange
            var quoteRepository = new Repository(new List<Quote>
                {new Quote(Guid.NewGuid(), new List<Item>(), new Contact("test", "example@example.com"))});
            var controller = new QuoteController(quoteRepository, new FakeEventBus());

            //act
            var listOfQuotes = controller.GetQuotes();

            //assert
            Assert.NotEmpty(listOfQuotes);
        }

        [Fact]
        public void GivenQuoteExits_WhenGetQuoteById_ThenReturnQuote()
        {
            //arrange
            var quote = new Quote(Guid.NewGuid(), new List<Item>(), new Contact("test", "example@example.com"));
            var controller = new QuoteController(new Repository(new List<Quote>() {quote}), new FakeEventBus());

            //act
            var returnedQuote = controller.GetQuote(quote.GetQuoteId());

            //assert
            Assert.Equal(returnedQuote, quote);
        }

        [Fact]
        public void GivenQuoteDoesntExist_WhenGetQuoteById_ThenReturnNull()
        {
            //arrange
            var controller = new QuoteController(new Repository(), new FakeEventBus());

            //act
            var fakeQuoteId = Guid.NewGuid();
            var returnedQuote = controller.GetQuote(fakeQuoteId);

            //assert
            Assert.NotEqual(returnedQuote.Id, fakeQuoteId);
        }

        [Fact]
        public void GivenQuoteExists_WhenItemAddedToQuote_ThenQuoteShouldHaveItem()
        {
            //arrange
            var quoteGuid = Guid.NewGuid();
            var quote = new Quote(quoteGuid, new List<Item>() { new Item( quoteGuid, "test", 10.00 ) }, new Contact("test", "example@example.com") );
            var newQuoteItem = new Item(quoteGuid, "Labour", 10.00);
            var expectedQuote = new Quote(quoteGuid, new List<Item>() { new Item( quoteGuid, "test", 10.00 ), newQuoteItem }, new Contact("test", "example@example.com"));

            var controller = new QuoteController(new Repository(new List<Quote>() { quote }), new FakeEventBus());

            //act
            controller.AddItemToQuote(quote.GetQuoteId(), newQuoteItem);
            var actualQuote = controller.GetQuote(quote.GetQuoteId());

            //assert
            Assert.True(expectedQuote.IsDeepEqual(actualQuote));
        }

        [Fact]
        public void GivenQuoteExists_WhenItemAddedToQuoteTwice_ThenQuoteShouldHaveOnlyOneItem()
        {
            //arrange
            var quoteGuid = Guid.NewGuid();
            var quote = new Quote(quoteGuid, new List<Item>() { new Item( quoteGuid, "test", 10.00 ) }, new Contact("test", "example@example.com") );
            var newQuoteItem = new Item(quoteGuid, "Labour", 10.00);
            var expectedQuote = new Quote(quoteGuid, new List<Item>() { new Item( quoteGuid, "test", 10.00 ), newQuoteItem }, new Contact("test", "example@example.com"));

            var controller = new QuoteController(new Repository(new List<Quote>() { quote }), new FakeEventBus());

            //act
            controller.AddItemToQuote(quote.GetQuoteId(), newQuoteItem);
            controller.AddItemToQuote(quote.GetQuoteId(), newQuoteItem);
            var actualQuote = controller.GetQuote(quote.GetQuoteId());

            //assert
            Assert.True(expectedQuote.IsDeepEqual(actualQuote));
        }

        [Fact]
        public void GivenQuoteItemExists_WhenIUpdateThePrice_ThenQuoteItemHasTheNewPrice()
        {
            //arrange
            var quoteGuid = Guid.NewGuid();
            var quoteItem = new Item( quoteGuid, "test", 10.00 );
            var quote = new Quote(quoteGuid, new List<Item>() {quoteItem}, new Contact("test", "example@example.com") );
            var expectedQuote = new Quote(quoteGuid, new List<Item>() { new Item( quoteGuid, "test", 20.00 ) }, new Contact("test", "example@example.com"));

            var controller = new QuoteController(new Repository(new List<Quote>() { quote }), new FakeEventBus());

            //act
            controller.UpdateQuoteItemPrice(quote.GetQuoteId(), quoteItem.Message, 20.00);

            var actualQuote = controller.GetQuote(quote.GetQuoteId());

            //assert
            Assert.True(expectedQuote.IsDeepEqual(actualQuote));
        }

        // ideas for more tests:

        // delete quote

    }
}