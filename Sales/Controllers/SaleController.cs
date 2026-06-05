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

        public SaleController(
            CreateTicketOrderUseCase createTicketOrderUseCase,
            GetSaleByIdUseCase getSaleByIdUseCase,
            GetTotalTicketsSoldForEventUseCase getTotalTicketsSoldForEventUseCase,
            GetSalesByUserIdUseCase getSalesByUserIdUseCase,
            ApproveSaleUseCase approveSaleUseCase,
            RejectSaleUseCase rejectSaleUseCase)
        {
            _createTicketOrderUseCase = createTicketOrderUseCase;
            _getSaleByIdUseCase = getSaleByIdUseCase;
            _getTotalTicketsSoldForEventUseCase = getTotalTicketsSoldForEventUseCase;
            _getSalesByUserIdUseCase = getSalesByUserIdUseCase;
            _approveSaleUseCase = approveSaleUseCase;
            _rejectSaleUseCase = rejectSaleUseCase;
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
            catch (Exception ex)
            {
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
            catch (Exception ex)
            {
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
            catch (Exception ex)
            {
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
            catch (Exception ex)
            {
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
            catch (Exception ex)
            {
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
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
