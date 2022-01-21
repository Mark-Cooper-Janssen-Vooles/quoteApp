using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace QuoteAPI
{
    public class SqsClient : ISqsClient
    {
        private readonly AmazonSQSClient _amazonSqsClient;

        public SqsClient()
        {
            _amazonSqsClient = new AmazonSQSClient(RegionEndpoint.APSoutheast2);
        }

        public Task SendMessageAsync(SendMessageRequest request)
        {
            return _amazonSqsClient.SendMessageAsync(request);
        }
    }
}