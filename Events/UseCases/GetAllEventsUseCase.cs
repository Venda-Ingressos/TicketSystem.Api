using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Api.Events.DTOs;
using TicketSystem.Api.Events.Interfaces;

namespace TicketSystem.Api.Events.UseCases
{
    public class GetAllEventsUseCase
    {
        private readonly IEventRepository _repository;

        public GetAllEventsUseCase(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EventResponse>> ExecuteAsync()
        {
            var events = await _repository.GetAllAsync();

            // Transforma a Entidade Rica de volta em DTO para o Controller
            return events.Select(e => new EventResponse
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                TotalCapacity = e.TotalCapacity,
                TicketPrice = e.TicketPrice.Amount
            });
        }
    }
}