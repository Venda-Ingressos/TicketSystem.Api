using System;

namespace TicketSystem.Api.Shared.Entities
{
    public class TicketOrder 
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }

        public int Quantity { get; set; }
        public int Status { get; set; }
    }
}