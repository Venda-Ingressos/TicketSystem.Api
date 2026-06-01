using System;

namespace TicketSystem.Api.Shared.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}