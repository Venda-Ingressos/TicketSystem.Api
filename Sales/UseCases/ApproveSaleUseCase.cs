using System;
using System.Threading.Tasks;
using TicketSystem.Api.Sales.Interfaces;

namespace TicketSystem.Api.Sales.UseCases
{
    public class ApproveSaleUseCase
    {
        private readonly ITicketOrderRepository _ticketOrderRepository;

        public ApproveSaleUseCase(ITicketOrderRepository ticketOrderRepository)
        {
            _ticketOrderRepository = ticketOrderRepository;
        }

        public async Task ExecuteAsync(Guid saleId)
        {
            var order = await _ticketOrderRepository.GetById(saleId);

            if (order == null)
                throw new KeyNotFoundException("Venda não encontrada.");

            order.ApprovePayment();
            await _ticketOrderRepository.Update(order);
        }
    }
}
