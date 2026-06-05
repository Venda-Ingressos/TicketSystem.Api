using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Api.Sales.Interfaces;

namespace TicketSystem.Api.Sales.UseCases
{
    public class GetTotalTicketsSoldForEventUseCase
    {
        private readonly ITicketOrderRepository _ticketOrderRepository;

        public GetTotalTicketsSoldForEventUseCase(ITicketOrderRepository ticketOrderRepository)
        {
            _ticketOrderRepository = ticketOrderRepository;
        }

        public async Task<int> ExecuteAsync(Guid eventId)
        {
            return await _ticketOrderRepository.GetTotalTicketsSoldForEvent(eventId);
        }
    }
}