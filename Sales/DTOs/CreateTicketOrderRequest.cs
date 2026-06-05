using System;

namespace TicketSystem.Api.Sales.DTOs
{
    public class CreateTicketOrderRequest
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
    }
}
