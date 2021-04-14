namespace QuoteAPI
{
    public class DraftQuote
    {
        public string Message { get; }
        public DraftQuote(string message)
        {
            Message = message;
        }
    }
}