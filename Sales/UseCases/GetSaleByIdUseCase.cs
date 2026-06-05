using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Api.Sales.Interfaces;
using TicketSystem.Api.Sales.Entities;

namespace TicketSystem.Api.Sales.UseCases
{
    public class GetSaleByIdUseCase
    {
        readonly ITicketOrderRepository _ticketOrderRepository;

        public GetSaleByIdUseCase(ITicketOrderRepository ticketOrderRepository)
        {
            _ticketOrderRepository = ticketOrderRepository;
        }

        public async Task<TicketOrder> ExecuteAsync(Guid saleId)
        {
            var order = await _ticketOrderRepository.GetById(saleId);
            if (order == null)
                throw new Exception("Venda não encontrada.");

            return order;
        }
    }
}