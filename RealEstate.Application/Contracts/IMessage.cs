using RealEstate.Application.DTOs.Request.Chat;
using RealEstate.Application.DTOs.Response.Chat;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Contracts
{
    public interface IMessage
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int messageId);
        Task<IEnumerable<MessageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string recipientId);
        Task<bool> SaveAllAsync();
    }
}
