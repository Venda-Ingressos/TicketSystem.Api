using System;
using System.Threading.Tasks;
using TicketSystem.Api.Events.Interfaces;

namespace TicketSystem.Api.Events.UseCases
{
    public class DeleteEventUseCase
    {
        private readonly IEventRepository _repository;

        public DeleteEventUseCase(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Guid id)
        {
            // Manda o repositório deletar direto pelo ID
            await _repository.DeleteAsync(id);
        }
    }
}