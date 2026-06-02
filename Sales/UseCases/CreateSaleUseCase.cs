using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Api.Sales.Interfaces;

namespace TicketSystem.Api.Sales.UseCases
{
    public class CreateSaleUseCase
    {
        private readonly ITicketOrderRepository _repository;

        public CreateSaleUseCase(ITicketOrderRepository repository)
        {
            _repository = repository;
        }

    }
}