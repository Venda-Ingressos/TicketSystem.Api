using System.Threading.Tasks;
using TicketSystem.Api.Events.DTOs;
using TicketSystem.Api.Events.Interfaces;
using TicketSystem.Api.Events.ValueObjects;

// Usamos o Alias novamente para garantir que estamos criando a Entidade Rica (do Domínio)
using DomainEvent = TicketSystem.Api.Events.Entities.Event;

namespace TicketSystem.Api.Events.UseCases
{
    public class CreateEventUseCase
    {
        private readonly IEventRepository _repository;

        // Injeção de dependência da interface do repositório
        public CreateEventUseCase(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(CreateEventRequest request)
        {
            // 1. CONVERTE E VALIDA: Transforma o decimal em um Value Object (Money).
            // Se o valor for negativo, o construtor do Money vai estourar um erro aqui.
            var price = new Money(request.TicketPrice);

            // 2. CRIA A ENTIDADE RICA: Passa os dados para o domínio.
            // Aqui rodariam validações como "a data não pode ser no passado".
            var newEvent = new DomainEvent(
                request.Title,
                request.Description,
                request.Date,
                request.TotalCapacity,
                price
            );

            // 3. PERSISTE: Manda para o repositório.
            // O UseCase não faz ideia de que existe SQLite ou pasta Shared.
            await _repository.AddAsync(newEvent);
        }
    }
}