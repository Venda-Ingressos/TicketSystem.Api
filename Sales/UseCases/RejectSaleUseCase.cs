using System;
using System.Threading.Tasks;
using TicketSystem.Api.Sales.Interfaces;

namespace TicketSystem.Api.Sales.UseCases
{
    public class RejectSaleUseCase
    {
        private readonly ITicketOrderRepository _ticketOrderRepository;

        public RejectSaleUseCase(ITicketOrderRepository ticketOrderRepository)
        {
            _ticketOrderRepository = ticketOrderRepository;
        }

        public async Task ExecuteAsync(Guid saleId)
        {
            var order = await _ticketOrderRepository.GetById(saleId);

            if (order == null)
                throw new KeyNotFoundException("Venda não encontrada.");

            order.RejectPayment();
            await _ticketOrderRepository.Update(order);
        }
    }
}
