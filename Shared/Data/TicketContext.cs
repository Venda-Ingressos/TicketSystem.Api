using Microsoft.EntityFrameworkCore;
using TicketSystem.Api.Shared.Entities;
using TicketSystem.Api.Shered.Entities;

namespace TicketSystem.Api.Data
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