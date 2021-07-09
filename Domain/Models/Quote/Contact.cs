using System;

namespace Domain.Models.Quote
{
    public class Contact
    {
        public string Name;
        public string Email;

        public Contact(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}