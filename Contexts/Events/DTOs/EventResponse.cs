using System;

namespace TicketSystem.Api.Events.DTOs
{
    public class EventResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int TotalCapacity { get; set; }
        public decimal TicketPrice { get; set; }
    }
}