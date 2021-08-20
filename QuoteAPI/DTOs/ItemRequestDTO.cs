using System;

namespace QuoteAPI.DTOs
{
    public class ItemRequestDTO
    {
        public string Message { get; set; }
        public double Price { get; set; }

        public ItemRequestDTO() {}
    }
}