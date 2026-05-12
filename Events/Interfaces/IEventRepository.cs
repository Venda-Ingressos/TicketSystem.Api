using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSystem.Api.Events.Entities;

namespace TicketSystem.Api.Events.Interfaces
{
    public interface IEventRepository
    {
        Task<Event> GetByIdAsync(Guid id);
        Task<IEnumerable<Event>> GetAllUpcomingEventsAsync(); 
        Task AddAsync(Event eventEntity);
        Task UpdateAsync(Event eventEntity);
    }
}