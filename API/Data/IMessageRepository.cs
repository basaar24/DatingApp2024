namespace API.Data;

using API.DataEntities;
using API.DTOs;
using API.Helpers;

public interface IMessageRepository
{
    public void Add(Message message);
    public void Remove(Message message);
    public Task<Message?> GetAsync(int id);
    public Task<PagedList<MessageResponse>> GetForUserAsync(MessageParams messageParams);
    public Task<IEnumerable<MessageResponse>> GetThreadAsync(string currentUsername, string recipientUsername);
    public Task<bool> SaveAllAsync();

}