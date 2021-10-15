using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Domain.Events;
using Domain.Models.Quote;
using Xunit;

namespace QuoteAPI.Tests
{
    public class EventBusTests
    {
        [Fact]
        public async Task GivenSqsIsAvailable_WhenQuoteEventSentToEventBus_ThenQuoteSent()
        {
            // given
            var sqsClientDouble = new SqsClientDouble();
            var eventBus = new EventBus(sqsClientDouble);
            var quote = new Quote(Guid.NewGuid(), new List<Item>(), new Contact("test", "example@example.com"));
            var quoteSent = new QuoteSent(quote, quote.Contact.Email);
            // when
            await eventBus.Publish(quoteSent);
            // then
            Assert.NotEmpty(sqsClientDouble.Items);
        }
    }

    public class SqsClientDouble : ISqsClient
    {
        public SqsClientDouble()
        {
            Items = new List<SendMessageRequest>();
        }

        public List<SendMessageRequest> Items { get; }

        public Task SendMessageAsync(SendMessageRequest request)
        {
            return Task.Run(() => Items.Add(request));
        }
    }
}