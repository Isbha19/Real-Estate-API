
namespace RealEstate.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public  User Sender { get; set; }
        public string ReceiverId { get; set; } 
        public  User Receiver { get; set; }

        public string Content { get; set; }
        public DateTimeOffset? DateRead { get; set; }
        public DateTimeOffset SentAt { get; set; } = DateTimeOffset.UtcNow;

        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }

    }
}
