using System;

namespace Domain.Models.Quote
{
    public class Item
    {
        // need to add Id for each item
        public string Message { get; }
        public double Price { get; private set; }
        public Guid Id { get; }

        public Item(Guid id, string message, double price )
        {
            Id = id;
            Message = message;
            Price = price;
        }

        public void UpdatePrice(double newPrice)
        {
            Price = newPrice;
        }
    }
}