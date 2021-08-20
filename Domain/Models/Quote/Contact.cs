using System;

namespace Domain.Models.Quote
{
    public class Contact
    {
        public string Name { set; get; }
        public string Email { set; get; }

        public Contact() { }

        public Contact(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}