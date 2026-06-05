using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSystem.Api.Sales.Entities;

namespace TicketSystem.Api.Sales.Interfaces
{
    public interface ITicketOrderRepository
    {
        Task<TicketOrder> GetById(Guid id); // feito
        Task<IEnumerable<TicketOrder>> GetByUserId(Guid userId); // feito
        Task Add(TicketOrder order); // feito
        Task Update(TicketOrder order);
        Task<int> GetTotalTicketsSoldForEvent(Guid eventId); // feito
    }
}
