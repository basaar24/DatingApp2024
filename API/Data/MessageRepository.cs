namespace API.Data;

using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using API.DataEntities;
using API.DTOs;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;

public class MessageRepository(DataContext context, IMapper mapper) : IMessageRepository
{
    public void Add(Message message) => context.Messages.Add(message);

    public void Remove(Message message) => context.Messages.Remove(message);

    public async Task<Message?> GetAsync(int id) => await context.Messages.FindAsync(id);

    public async Task<PagedList<MessageResponse>> GetForUserAsync(MessageParams messageParams)
    {
        var query = context.Messages.OrderByDescending(m => m.MessageSent)
            .AsQueryable();

        query = messageParams.Container.ToLower(CultureInfo.InvariantCulture) switch
        {
            "inbox" => query.Where(m => m.Recipient.UserName == messageParams.Username),
            "outbox" => query.Where(m => m.Sender.UserName == messageParams.Username),
            _ => query.Where(m => m.Recipient.UserName == messageParams.Username
                && m.DateRead == null)
        };

        var messages = query.ProjectTo<MessageResponse>(mapper.ConfigurationProvider);

        return await PagedList<MessageResponse>
            .CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
    }

    public Task<IEnumerable<MessageResponse>> GetThreadAsync(string currentUsername, string recipientUsername)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync() => await context.SaveChangesAsync() > 0;
}