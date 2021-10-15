using Domain.Models.Quote;

namespace Domain.Events
{
    public class ItemSent
    {
        public Item DraftItem { get; }
        public string Email { get; }
        public ItemSent(Item draftItem, string email)
        {
            DraftItem = draftItem;
            Email = email;
        }
    }
}