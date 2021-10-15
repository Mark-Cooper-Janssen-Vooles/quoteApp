using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace QuoteAPI
{
    public interface ISqsClient
    {
        Task SendMessageAsync(SendMessageRequest request);
    }
}