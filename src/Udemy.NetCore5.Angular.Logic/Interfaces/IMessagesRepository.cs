using System.Collections.Generic;
using System.Threading.Tasks;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.DTOs;
using Udemy.NetCore5.Angular.Logic.Helpers;

namespace Udemy.NetCore5.Angular.Logic.Interfaces
{
    public interface IMessagesRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);

        Task<Message> GetMessage(int id);
        Task<PagedList<AppUserMessagesResponse>> GetMessagesForUser(MessageParams messageParams);

        Task<IEnumerable<AppUserMessagesResponse>> GetMessageThread(int currentUserId, int recipientId);

        Task<bool> SaveAllAsync();
    }
}