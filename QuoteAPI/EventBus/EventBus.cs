using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Domain.Events;
using Newtonsoft.Json;

namespace QuoteAPI
{
    public interface IEventBus
    {
        Task Publish(QuoteSent quoteSent);
    }

    public class EventBus : IEventBus
    {
        private readonly ISqsClient _sqsClient;

        public EventBus(ISqsClient sqsClient)
        {
            _sqsClient = sqsClient;
        }

        public async Task Publish(QuoteSent quoteSent)
        {
            var request = new SendMessageRequest("https://sqs.ap-southeast-2.amazonaws.com/534833720216/QuoteAPI-Email", JsonConvert.SerializeObject(quoteSent));
            await _sqsClient.SendMessageAsync(request);
        }
    }
}