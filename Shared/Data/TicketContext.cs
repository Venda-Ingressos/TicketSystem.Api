using Microsoft.EntityFrameworkCore;
using TicketSystem.Api.Shared.Entities;

namespace TicketSystem.Api.Shared.Data
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options)
        {
        }

        
        public DbSet<Event> Events { get; set; }
        public DbSet<TicketOrder> TicketOrders { get; set; }
        public DbSet<User> Users { get; set; }
    }
}