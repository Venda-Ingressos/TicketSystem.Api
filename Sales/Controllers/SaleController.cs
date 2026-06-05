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

        public SaleController(CreateTicketOrderUseCase createTicketOrderUseCase, GetSaleByIdUseCase getSaleByIdUseCase)
        {
            _createTicketOrderUseCase = createTicketOrderUseCase;
            _getSaleByIdUseCase = getSaleByIdUseCase;
        }

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
    }
}
