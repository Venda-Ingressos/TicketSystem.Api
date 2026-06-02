using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Api.Events.DTOs;
using TicketSystem.Api.Events.UseCases;

namespace TicketSystem.Api.Events.Controllers
{
    // Define que esta classe é um Controller de API e configura a rota principal (/api/Event)
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly CreateEventUseCase _createEventUseCase;
        private readonly GetAllEventsUseCase _getAllEventsUseCase;
        private readonly UpdateEventUseCase _updateEventUseCase;
        private readonly DeleteEventUseCase _deleteEventUseCase;

        // Injeção de dependência de todos os UseCases do CRUD
        public EventController(
            CreateEventUseCase createEventUseCase,
            GetAllEventsUseCase getAllEventsUseCase,
            UpdateEventUseCase updateEventUseCase,
            DeleteEventUseCase deleteEventUseCase)
        {
            _createEventUseCase = createEventUseCase;
            _getAllEventsUseCase = getAllEventsUseCase;
            _updateEventUseCase = updateEventUseCase;
            _deleteEventUseCase = deleteEventUseCase;
        }

        // CREATE (POST)
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
        {
            try
            {
                await _createEventUseCase.ExecuteAsync(request);
                return Ok(new { message = "Evento criado com sucesso!" });
            }
            catch (Exception ex)
            {
                // Devolve erro 400 se as regras de negócio do Domínio falharem
                return BadRequest(new { error = ex.Message });
            }
        }

        // READ (GET)
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            // Busca todos os eventos formatados como EventResponse
            var events = await _getAllEventsUseCase.ExecuteAsync();
            return Ok(events); // Devolve Status 200 com a lista no corpo da resposta
        }

        // UPDATE (PUT) - Recebe o ID pela URL e os dados pelo JSON
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] UpdateEventRequest request)
        {
            try
            {
                await _updateEventUseCase.ExecuteAsync(id, request);
                return Ok(new { message = "Evento atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE (DELETE) - Recebe o ID pela URL
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            // No caso do delete, assumimos que se não estourar erro, a exclusão ocorreu com sucesso
            await _deleteEventUseCase.ExecuteAsync(id);
            return Ok(new { message = "Evento excluído com sucesso!" });
        }
    }
}