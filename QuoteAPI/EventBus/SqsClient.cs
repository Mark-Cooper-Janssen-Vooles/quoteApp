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
            // note: will not send emails to all addresses, need to be AWS verified because my account is in the sandbox
            // not worth applying to get it out
            // details here: https://docs.aws.amazon.com/ses/latest/DeveloperGuide/request-production-access.html?icmpid=docs_ses_console
        }
    }
}