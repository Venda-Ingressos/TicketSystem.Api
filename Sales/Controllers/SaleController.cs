using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Api.Sales.DTOs;
using TicketSystem.Api.Sales.UseCases;

namespace TicketSystem.Api.Sales.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly CreateTicketOrderUseCase _createTicketOrderUseCase;
        private readonly GetSaleByIdUseCase _getSaleByIdUseCase;
        private readonly GetTotalTicketsSoldForEventUseCase _getTotalTicketsSoldForEventUseCase;
        private readonly GetSalesByUserIdUseCase _getSalesByUserIdUseCase;
        private readonly ApproveSaleUseCase _approveSaleUseCase;
        private readonly RejectSaleUseCase _rejectSaleUseCase;
        private readonly CancelSaleUseCase _cancelSaleUseCase;

        public SaleController(
            CreateTicketOrderUseCase createTicketOrderUseCase,
            GetSaleByIdUseCase getSaleByIdUseCase,
            GetTotalTicketsSoldForEventUseCase getTotalTicketsSoldForEventUseCase,
            GetSalesByUserIdUseCase getSalesByUserIdUseCase,
            ApproveSaleUseCase approveSaleUseCase,
            RejectSaleUseCase rejectSaleUseCase,
            CancelSaleUseCase cancelSaleUseCase)
        {
            _createTicketOrderUseCase = createTicketOrderUseCase;
            _getSaleByIdUseCase = getSaleByIdUseCase;
            _getTotalTicketsSoldForEventUseCase = getTotalTicketsSoldForEventUseCase;
            _getSalesByUserIdUseCase = getSalesByUserIdUseCase;
            _approveSaleUseCase = approveSaleUseCase;
            _rejectSaleUseCase = rejectSaleUseCase;
            _cancelSaleUseCase = cancelSaleUseCase;
        }
        //nova venda
        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateTicketOrderRequest request)
        {
            try
            {
                var saleId = await _createTicketOrderUseCase.ExecuteAsync(request);
                return Ok(new
                {
                    id = saleId,
                    message = "Venda realizada com sucesso!"
                });
            }
            catch (ArgumentException ex)
            {
                // 400
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // 500 ou 400 dependendo do tipo de erro
                return BadRequest(new { error = ex.Message });
            }
        }
        // ingresso por id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleById(Guid id)
        {
            try
            {
                var order = await _getSaleByIdUseCase.ExecuteAsync(id);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                // 404
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // 500 ou 400 dependendo do tipo de erro
                return BadRequest(new { error = ex.Message });
            }
        }

        // total de ingressos vendidos para um evento
        [HttpGet("event/{eventId}/total-sold")]
        public async Task<IActionResult> GetTotalTicketsSoldForEvent(Guid eventId)
        {
            try
            {
                var totalSold = await _getTotalTicketsSoldForEventUseCase.ExecuteAsync(eventId);

                return Ok(new
                {
                    eventId = eventId,
                    totalTicketsSold = totalSold
                });
            }
            catch (ArgumentException ex)
            {
                // 400
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // 500 ou 400 dependendo do tipo de erro
                return BadRequest(new { error = ex.Message });
            }
        }

        // vendas por usuário
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetSalesByUserId(Guid userId)
        {
            try
            {
                var sales = await _getSalesByUserIdUseCase.ExecuteAsync(userId);
                return Ok(sales);
            }
            catch (ArgumentException ex)
            {
                // 400
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // 500 ou 400 dependendo do tipo de erro
                return BadRequest(new { error = ex.Message });
            }
        }

        // Aprovar venda
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveSale(Guid id)
        {
            try
            {
                await _approveSaleUseCase.ExecuteAsync(id);
                return Ok(new { message = "Venda aprovada com sucesso!" });
            }
            catch (KeyNotFoundException ex)
            {
                // 404
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // 400
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // 500 ou 400 dependendo do tipo de erro
                return BadRequest(new { error = ex.Message });
            }
        }

        // Rejeitar venda
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectSale(Guid id)
        {
            try
            {
                await _rejectSaleUseCase.ExecuteAsync(id);
                return Ok(new { message = "Venda rejeitada com sucesso!" });
            }
            catch (KeyNotFoundException ex)
            {
                // 404
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // 400
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // 500 ou 400 dependendo do tipo de erro
                return BadRequest(new { error = ex.Message });
            }
        }

        // Cancelar venda
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelSale(Guid id)
        {
            try
            {
                await _cancelSaleUseCase.ExecuteAsync(id);
                return Ok(new { message = "Venda cancelada com sucesso!" });
            }
            catch (KeyNotFoundException ex)
            {
                // 404
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // 400
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // 500 ou 400 dependendo do tipo de erro
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
