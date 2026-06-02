using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSystem.Api.Events.Entities;

namespace TicketSystem.Api.Events.Interfaces
{
    public interface IEventRepository
    {
        Task AddAsync(Event eventEntity);
        Task<Event> GetByIdAsync(Guid id);
        Task<IEnumerable<Event>> GetAllAsync(); // Read (Lista)
        Task UpdateAsync(Event eventEntity);     // Update
        Task DeleteAsync(Guid id);               // Delete
    }
}