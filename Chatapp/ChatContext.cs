using Chatapp.Enities;
using Microsoft.EntityFrameworkCore;

namespace Chatapp
{
    public class ChatContext:DbContext
    {
        public ChatContext( DbContextOptions<ChatContext> options) : base(options) { }

        public DbSet<ChatMessage> chatMessages { get; set; }
    }
}
