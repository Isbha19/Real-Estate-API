using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Request.Chat
{
    public class CreateMessageDto
    {
        public string RecipientId { get; set; }


        public string Content { get; set; }
    }
}
