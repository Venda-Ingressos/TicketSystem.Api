using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Api.Sales.Entities;
using TicketSystem.Api.Sales.Interfaces;

namespace TicketSystem.Api.Sales.UseCases
{
    public class GetSalesByUserIdUseCase
    {
        private readonly ITicketOrderRepository _ticketOrderRepository;

        public GetSalesByUserIdUseCase(ITicketOrderRepository ticketOrderRepository)
        {
            _ticketOrderRepository = ticketOrderRepository;
        }

        public async Task<IEnumerable<TicketOrder>> ExecuteAsync(Guid userId)
        {
            return await _ticketOrderRepository.GetByUserId(userId);
        }
    }
}