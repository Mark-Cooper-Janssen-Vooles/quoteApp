using Domain.Models.Quote;

namespace Domain.Events
{
    public class QuoteSent
    {
        public Quote Quote { get; }
        public string Email { get; }
        public QuoteSent(Quote quote, string email)
        {
            Quote = quote;
            Email = email;
        }
    }
}