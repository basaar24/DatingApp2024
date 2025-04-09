namespace API.Controllers;

using System.Globalization;
using API.Data;
using API.DataEntities;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class MessagesController
    (IMessageRepository messageRepository,
    IUserRepository userRepository,
    IMapper mapper) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<MessageResponse>> CreateMessage(MessageRequest request)
    {
        var username = User.GetUserName();

        if (username == request.RecipientUsername.ToLower(CultureInfo.InvariantCulture))
        {
            return BadRequest("You can't message yourself!");
        }

        var sender = await userRepository.GetByUsernameAsync(username);
        var recipient = await userRepository.GetByUsernameAsync(request.RecipientUsername);

        if (recipient == null || sender == null || sender.UserName == null || recipient.UserName == null)
        {
            return BadRequest("The message can't be sent right now");
        }

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = request.Content
        };

        messageRepository.Add(message);

        if (await messageRepository.SaveAllAsync())
        {
            return Ok(mapper.Map<MessageResponse>(message));
        }

        return BadRequest("Something went wrong!");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageResponse>>> GetMessagesForUser(
        [FromQuery] MessageParams messageParams)
    {
        messageParams.Username = User.GetUserName();
        var messages = await messageRepository.GetForUserAsync(messageParams);
        Response.AddPaginationHeader(messages);
        return messages;
    }

    [HttpGet("thread/{username}")]
    public async Task<ActionResult<IEnumerable<MessageResponse>>> GetMessageThread(string username)
    {
        var currentUsername = User.GetUserName();
        return Ok(await messageRepository.GetThreadAsync(currentUsername, username));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var username = User.GetUserName();
        var message = await messageRepository.GetAsync(id);

        if (message == null)
        {
            return BadRequest("Can't delete the message");
        }

        if (message.SenderUsername != username && message.RecipientUsername != username)
        {
            return Forbid();
        }

        if (message.SenderUsername == username)
        {
            message.SenderDeleted = true;
        }

        if (message.RecipientUsername == username)
        {
            message.RecipientDeleted = true;
        }

        if (message is { SenderDeleted: true, RecipientDeleted: true })
        {
            messageRepository.Remove(message);
        }

        if (await messageRepository.SaveAllAsync())
        {
            return Ok();
        }

        return BadRequest("There was an issue");
    }
}