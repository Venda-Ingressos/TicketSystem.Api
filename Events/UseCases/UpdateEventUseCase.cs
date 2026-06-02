using System;
using System.Threading.Tasks;
using TicketSystem.Api.Events.DTOs;
using TicketSystem.Api.Events.Interfaces;
using TicketSystem.Api.Events.ValueObjects;

using DomainEvent = TicketSystem.Api.Events.Entities.Event;

namespace TicketSystem.Api.Events.UseCases
{
    public class UpdateEventUseCase
    {
        private readonly IEventRepository _repository;

        public UpdateEventUseCase(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Guid id, UpdateEventRequest request)
        {
            // 1. Converte o preço primitivo no Value Object, validando a regra de negócio
            var price = new Money(request.TicketPrice);

            // 2. Instancia a Entidade Rica com os novos dados
            // As validações (ex: data no passado) vão rodar automaticamente no construtor
            var eventToUpdate = new DomainEvent(
                request.Title,
                request.Description,
                request.Date,
                request.TotalCapacity,
                price
            )
            {
                Id = id // Injetamos o Id aqui para o repositório saber quem atualizar!
            };

            // 3. Manda para o repositório atualizar no banco
            await _repository.UpdateAsync(eventToUpdate);
        }
    }
}