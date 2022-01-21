using System;

namespace Domain.Models.Quote
{
    public class Contact
    {
        public string Name { set; get; }
        public string Email { set; get; }

        public Contact(string name, string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new Exception("Please provide email");
            
            Name = name;
            Email = email;
        }
    }
}