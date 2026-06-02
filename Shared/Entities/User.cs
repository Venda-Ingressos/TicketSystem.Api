using System;

namespace TicketSystem.Api.Shared.Entities
{
    public class User 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}