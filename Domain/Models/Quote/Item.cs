namespace Domain.Models.Quote
{
    public class Item
    {
        public string Message { get; }
        public double Price { get; private set; }

        public Item(string message, double price )
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