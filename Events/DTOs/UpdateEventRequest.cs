using System;

namespace TicketSystem.Api.Events.DTOs
{
    public class UpdateEventRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int TotalCapacity { get; set; }
        public decimal TicketPrice { get; set; }
    }
}