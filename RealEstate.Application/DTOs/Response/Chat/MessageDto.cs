using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Chat
{
    public class MessageDto
    {
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        //public string SenderPhotoUrl { get; set; }
        public string ReceiverId { get; set; } // User or Agent ID
        public string ReceiverName { get; set; }
        //public string RecipientPhotoUrl { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime SentAt { get; set; } 
      
    }
}
