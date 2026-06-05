using System;
using System.Threading.Tasks;
using TicketSystem.Api.Sales.Interfaces;

namespace TicketSystem.Api.Sales.UseCases
{
    public class CancelSaleUseCase
    {
        private readonly ITicketOrderRepository _ticketOrderRepository;

        public CancelSaleUseCase(ITicketOrderRepository ticketOrderRepository)
        {
            _ticketOrderRepository = ticketOrderRepository;
        }

        public async Task ExecuteAsync(Guid saleId)
        {
            var order = await _ticketOrderRepository.GetById(saleId);

            if (order == null)
                throw new KeyNotFoundException("Venda não encontrada.");

            order.CancelOrder();
            await _ticketOrderRepository.Update(order);
        }
    }
}
