using System;
using System.Threading.Tasks;
using TicketSystem.Api.Sales.DTOs;
using TicketSystem.Api.Sales.Entities;
using TicketSystem.Api.Sales.Interfaces;

namespace TicketSystem.Api.Sales.UseCases
{
    public class CreateTicketOrderUseCase
    {
        private readonly ITicketOrderRepository _repository;

        public CreateTicketOrderUseCase(ITicketOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> ExecuteAsync(CreateTicketOrderRequest request)
        {
            var order = new TicketOrder(request.EventId, request.UserId, request.Quantity);

            await _repository.Add(order);
            return order.Id;
        }
    }
}
