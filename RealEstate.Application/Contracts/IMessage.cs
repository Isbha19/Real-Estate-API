using RealEstate.Application.DTOs.Request.Chat;
using RealEstate.Application.DTOs.Response.Chat;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.signalr;

namespace RealEstate.Application.Contracts
{
    public interface IMessage
    {
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
        Task<Group> GetGroup(string groupName);
        Task<Group> GetGroupForConnection(string connectionId);
        Task<IEnumerable<UserChatsDto>> GetChatUsersAsync();

        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int messageId);
        Task<IEnumerable<MessageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string recipientId);
        Task<bool> SaveAllAsync();
    }
}
