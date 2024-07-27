using Chatapp.Enities;
using Chatapp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly ChatContext _context;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatController(ChatContext context, IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChatMessage>>> GetChatMessages()
    {
        return await _context.chatMessages.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<ChatMessage>> PostChatMessage(ChatMessage chatMessage)
    {
        chatMessage.Timestamp = DateTime.UtcNow;
        _context.chatMessages.Add(chatMessage);
        await _context.SaveChangesAsync();

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", chatMessage.User, chatMessage.Message);

        return CreatedAtAction(nameof(GetChatMessages), new { id = chatMessage.Id }, chatMessage);
    }
}
