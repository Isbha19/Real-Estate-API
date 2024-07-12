

namespace RealEstate.Application.DTOs.Request.Chat
{
    public class MessageParams
    {
        public string UserId { get; set; }
        public string Container { get; set; } = "Unread";
    }
}
