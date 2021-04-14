using System;
using System.Collections;
using System.Collections.Generic;
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
            var controller = new QuoteController(QuoteList, new List<DraftQuote>());
            
            //act 
            var listOfQuotes = controller.GetQuotes();
            
            //assert
            Assert.Empty( listOfQuotes );
        }

        [Fact]
        public void GivenTheSystemHasQuotes_WhenGettingQuotes_ThenWeSeeQuotesReturned()
        {
            //arrange
            var QuoteList = new List<Quote> {new Quote("test")};
            var controller = new QuoteController(QuoteList, new List<DraftQuote>());
            
            //act
            var listOfQuotes = controller.GetQuotes();
            
            //assert
            Assert.NotEmpty(listOfQuotes);
        }

        [Fact]
        public void Given_WhenDraftQuoteCreated_ThenShouldBeSaved()
        {
            //arrange
            var draftQuote = new DraftQuote("Draft Quote");
            var quoteList = new List<DraftQuote>();
            var controller = new QuoteController(new List<Quote>(), quoteList);
            
            //act
            controller.SaveDraftQuote(draftQuote);
            
            //assert
            var listOfQuotes = controller.GetDraftQuotes();
            Assert.Contains(draftQuote, listOfQuotes);
        }
        
        // homework: think about API for when draft quote finalised to normal quote
        [Fact]
        public void GivenDraftQuoteIsFinalised_WhenAddedAsNormalQuote_ThenShouldBeSaved()
        {
            //arrange
            var draftQuote = new DraftQuote("finalised draft Quote");
            var controller = new QuoteController(new List<Quote>(), new List<DraftQuote>() {draftQuote} );

            //act
            controller.AddQuote(draftQuote);

            //assert
            var listOfDraftQuotes = controller.GetDraftQuotes();
            var listOfQuotes = controller.GetQuotes();
            Assert.DoesNotContain(draftQuote, listOfDraftQuotes);
            Assert.Empty(listOfDraftQuotes);
            Assert.NotEmpty(listOfQuotes);
        }
        
        // ideas for more tests:
        
        // add draft quote? 
        // edit existing quote 
        // edit existing draft quote
        // delete quote 
        // delete draft quote
    }
}