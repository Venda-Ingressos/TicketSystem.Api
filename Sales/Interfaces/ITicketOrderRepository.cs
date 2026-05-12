using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSystem.Api.Sales.Entities;

namespace TicketSystem.Api.Sales.Interfaces
{
    public interface ITicketOrderRepository
    {
        Task<TicketOrder> GetByIdAsync(Guid id);
        Task<IEnumerable<TicketOrder>> GetByUserIdAsync(Guid userId);
        Task AddAsync(TicketOrder order);
        Task UpdateAsync(TicketOrder order);
        Task<int> GetTotalTicketsSoldForEventAsync(Guid eventId);
    }
}