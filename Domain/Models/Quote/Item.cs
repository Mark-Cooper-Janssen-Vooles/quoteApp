using System;
using System.Runtime.Serialization;

namespace Domain.Models.Quote
{
    public class Item
    {
        public string Message { get; set; }
        public double Price { get; set; }
        public Guid Id { get; set; }

        public Item() { }

        public Item(string message, double price )
        {
            Id = Guid.NewGuid(); // when not using a DB
            Message = message;
            Price = price;
        }

        public void UpdatePrice(double newPrice)
        {
            Price = newPrice;
        }

        public void UpdateMessage(string newMessage)
        {
            Message = newMessage;
        }
    }
}