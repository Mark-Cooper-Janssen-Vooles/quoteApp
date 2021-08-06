using System;
using System.Runtime.Serialization;

namespace Domain.Models.Quote
{
    [DataContract]
    public class Item
    {
        [DataMember]
        public string Message { get; }
        [DataMember]
        public double Price { get; private set; }
        [DataMember]
        public Guid Id { get; }

        public Item(Guid id, string message, double price )
        {
            Id = Guid.NewGuid(); // when not using a DB
            Message = message;
            Price = price;
        }

        public void UpdatePrice(double newPrice)
        {
            Price = newPrice;
        }
    }
}