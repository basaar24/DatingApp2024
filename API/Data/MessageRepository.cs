namespace API.Data;

using System.Collections.Generic;
using System.Threading.Tasks;
using API.DataEntities;
using API.DTOs;
using API.Helpers;

public class MessageRepository(DataContext context) : IMessageRepository
{
    public void Add(Message message) => context.Messages.Add(message);

    public void Remove(Message message) => context.Messages.Remove(message);

    public async Task<Message?> GetAsync(int id) => await context.Messages.FindAsync(id);

    public Task<PagedList<MessageResponse>> GetForUserAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MessageResponse>> GetThreadAsync(string currentUsername, string recipientUsername)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync() => await context.SaveChangesAsync() > 0;
}