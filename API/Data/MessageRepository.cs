namespace API.Data;

using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using API.DataEntities;
using API.DTOs;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

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
            "inbox" => query.Where(m => m.Recipient.UserName == messageParams.Username
                && !m.RecipientDeleted),
            "outbox" => query.Where(m => m.Sender.UserName == messageParams.Username
                && !m.SenderDeleted),
            _ => query.Where(m => m.Recipient.UserName == messageParams.Username
                && m.DateRead == null
                && !m.RecipientDeleted)
        };

        var messages = query.ProjectTo<MessageResponse>(mapper.ConfigurationProvider);

        return await PagedList<MessageResponse>
            .CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageResponse>> GetThreadAsync(string currentUsername, string recipientUsername)
    {
        var messages = await context.Messages
            .Include(m => m.Sender).ThenInclude(p => p.Photos)
            .Include(m => m.Recipient).ThenInclude(p => p.Photos)
            .Where(m =>
                (m.RecipientUsername == currentUsername && !m.RecipientDeleted && m.SenderUsername == recipientUsername) ||
                (m.RecipientUsername == recipientUsername && !m.SenderDeleted && m.SenderUsername == currentUsername)
            )
            .OrderBy(m => m.MessageSent)
            .ToListAsync();

        var unreadMessages = messages
            .Where(m => m.DateRead == null && m.RecipientUsername == currentUsername)
            .ToList();

        if (unreadMessages.Count != 0)
        {
            unreadMessages.ForEach(m => m.DateRead = DateTime.UtcNow);
            await context.SaveChangesAsync();
        }

        return mapper.Map<IEnumerable<MessageResponse>>(messages);
    }

    public async Task<bool> SaveAllAsync() => await context.SaveChangesAsync() > 0;
}