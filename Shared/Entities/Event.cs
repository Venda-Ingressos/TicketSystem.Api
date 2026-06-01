using System;

namespace TicketSystem.Api.Shared.Entities
{
    public class Event : EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int TotalCapacity { get; set; }
        public decimal TicketPrice { get; set; }
    }
}