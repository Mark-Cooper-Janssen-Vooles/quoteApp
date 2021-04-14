namespace QuoteAPI
{
    public class Quote
    {
        public string Message { get; }

        public Quote(string message)
        {
            Message = message;
        }
    }
}