namespace QuoteAPI.Models
{
    public class QuoteItem
    {
        public string Message { get; }
        public double Price { get; private set; }

        public QuoteItem(string message, double price)
        {
            Message = message;
            Price = price;
        }

        public void UpdatePrice(double newPrice)
        {
            Price = newPrice;
        }
    }
}