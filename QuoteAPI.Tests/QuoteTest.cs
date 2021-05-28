using System;
using System.Collections.Generic;
using System.Linq;
using DeepEqual.Syntax;
using Domain.Models.Quote;
using Xunit;

namespace QuoteAPI.Tests
{
    public class QuoteTest
    {
        [Fact]
        public void GivenQuote_WhenAddItem_ThenHasItem()
        {
            //arrange
            var quote = new Quote(Guid.NewGuid(), new List<Item>());
            var quoteItem = new Item("test", 10.00);

            //act
            quote.AddQuoteItem(quoteItem);

            //assert
            Assert.True(quoteItem.IsDeepEqual(quote.GetAllItems().First()));
        }

        [Fact]
        public void GivenQuoteWithValidItem_WhenAddItem_ThenHaveOnlyOneItem()
        {
            //arrange
            var quoteItem = new Item("test", 10.00);
            var quote = new Quote(Guid.NewGuid(), new List<Item>());
            quote.AddQuoteItem(quoteItem);

            //act
            quote.AddQuoteItem(quoteItem);
            var items = quote.GetAllItems().ToList();

            //assert
            Assert.True(quote.GetAllItems().ToList().Count() == 1);
            quote.AddQuoteItem(quoteItem);
        }

        [Fact]
        public void GivenQuoteWithValidItem_WhenUpdatePrice_ThenHaveNewPrice()
        {
            //arrange
            var quoteItem = new Item("test", 10.00);
            var quote = new Quote(Guid.NewGuid(), new List<Item>() { quoteItem });

            //act
            quote.UpdatePriceOnQuoteItem(quoteItem.Message, 14.00);

            //assert
            Assert.True(quote.GetAllItems().First().Price == 14.00);
        }

        [Fact]
        public void GivenQuote_WhenAddItem_ThenHaveDraftItem()
        {
            //arrange
            var quoteItem = new Item("test", 10.00);
            var quote = new Quote(Guid.NewGuid(), new List<Item>());

            //act
            quote.AddQuoteItem(quoteItem);

            //assert
            Assert.True(quote.HasDraftItems());
        }

        [Fact]
        public void GivenQuote_WhenAddItem_ThenHasNoFinalisedItem()
        {
            //arrange
            var quoteItem = new Item("test", 10.00);
            var quote = new Quote(Guid.NewGuid(), new List<Item>());

            //act
            quote.AddQuoteItem(quoteItem);

            //assert
            Assert.False(quote.HasFinalisedItems());
        }

        [Fact]
        public void GivenQuoteWithDraftItem_WhenFinaliseItem_ItemIsNowFinalisedAndNotDraft()
        {
            //arrange
            var item = new Item("test", 10.00);
            var quote = new Quote(Guid.NewGuid(), new List<Item>());
            quote.AddQuoteItem(item);

            //act
            quote.FinaliseDraftItem(item);

            //assert
            Assert.True(quote.HasFinalisedItems());
            Assert.False(quote.HasDraftItems());
        }
    }
}