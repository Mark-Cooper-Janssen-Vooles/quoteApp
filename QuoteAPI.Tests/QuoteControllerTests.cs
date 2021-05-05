using System;
using System.Collections;
using System.Collections.Generic;
using DeepEqual.Syntax;
using QuoteAPI.Models;
using Xunit;

namespace QuoteAPI.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void GivenTheSystemHasNoQuotes_WhenGettingQuotes_ThenWeSeeNoQuotesReturned()
        {
            //arrange
            var QuoteList = new List<Quote>();
            var controller = new QuoteController(QuoteList);
            
            //act 
            var listOfQuotes = controller.GetQuotes();
            
            //assert
            Assert.Empty( listOfQuotes );
        }

        [Fact]
        public void GivenTheSystemHasQuotes_WhenGettingQuotes_ThenWeSeeQuotesReturned()
        {
            //arrange
            var QuoteList = new List<Quote> {new Quote(Guid.NewGuid(), new List<QuoteItem>())};
            var controller = new QuoteController(QuoteList);
            
            //act
            var listOfQuotes = controller.GetQuotes();
            
            //assert
            Assert.NotEmpty(listOfQuotes);
        }
        
        [Fact]
        public void GivenQuoteExits_WhenGetQuoteById_ThenReturnQuote()
        {
            //arrange
            var quote = new Quote(Guid.NewGuid(), new List<QuoteItem>());
            var controller = new QuoteController(new List<Quote>() {quote});
            
            //act
            var returnedQuote = controller.GetQuote(quote.Id);

            //assert
            Assert.Equal(returnedQuote, quote);
        }
        
        [Fact]
        public void GivenQuoteDoesntExist_WhenGetQuoteById_ThenReturnNull()
        {
            //arrange
            var controller = new QuoteController(new List<Quote>() {});
            
            //act
            var returnedQuote = controller.GetQuote(Guid.NewGuid());
            
            //assert
            Assert.Null(returnedQuote);
        }

        [Fact]
        public void GivenQuoteExists_WhenItemAddedToQuote_ThenQuoteShouldHaveItem()
        {
            //arrange
            var quoteGuid = Guid.NewGuid();
            var quote = new Quote(quoteGuid, new List<QuoteItem>() { new QuoteItem( "test", 10.00 ) } );
            var newQuoteItem = new QuoteItem("Labour", 10.00);
            var expectedQuote = new Quote(quoteGuid, new List<QuoteItem>() { new QuoteItem( "test", 10.00 ), newQuoteItem });
            
            var controller = new QuoteController(new List<Quote>() { quote });

            //act
            controller.AddItemToQuote(quote.Id, newQuoteItem);
            var actualQuote = controller.GetQuote(quote.Id);

            //assert
            Assert.True(expectedQuote.IsDeepEqual(actualQuote));
        }

        [Fact] public void GivenQuoteExists_WhenItemAddedToQuoteTwice_ThenQuoteShouldHaveOnlyOneItem()
        {
            //arrange
            var quoteGuid = Guid.NewGuid();
            var quote = new Quote(quoteGuid, new List<QuoteItem>() { new QuoteItem( "test", 10.00 ) } );
            var newQuoteItem = new QuoteItem("Labour", 10.00);
            var expectedQuote = new Quote(quoteGuid, new List<QuoteItem>() { new QuoteItem( "test", 10.00 ), newQuoteItem });
            
            var controller = new QuoteController(new List<Quote>() { quote });

            //act
            controller.AddItemToQuote(quote.Id, newQuoteItem);
            controller.AddItemToQuote(quote.Id, newQuoteItem);
            var actualQuote = controller.GetQuote(quote.Id);

            //assert
            Assert.True(expectedQuote.IsDeepEqual(actualQuote));
        }
        
        [Fact] public void GivenQuoteItemExists_WhenIUpdateThePrice_ThenQuoteItemHasTheNewPrice()
        {
            //arrange
            var quoteGuid = Guid.NewGuid();
            var quoteItem = new QuoteItem( "test", 10.00 );
            var quote = new Quote(quoteGuid, new List<QuoteItem>() {quoteItem} );
            var expectedQuote = new Quote(quoteGuid, new List<QuoteItem>() { new QuoteItem( "test", 20.00 ) });
            
            var controller = new QuoteController(new List<Quote>() { quote });

            //act
            controller.UpdateQuoteItemPrice(quote.Id, quoteItem.Message, 20.00);
            
            var actualQuote = controller.GetQuote(quote.Id);

            //assert
            Assert.True(expectedQuote.IsDeepEqual(actualQuote));
        }


        // ideas for more tests:
        
        // delete quote 

    }
}