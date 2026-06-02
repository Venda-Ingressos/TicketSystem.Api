using System;

namespace TicketSystem.Api.Shared.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int TotalCapacity { get; set; }
        public decimal TicketPrice { get; set; }
    }
}