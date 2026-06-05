using Microsoft.EntityFrameworkCore;
using TicketSystem.Api.Events.Interfaces;
using TicketSystem.Api.Events.Repositories;
using TicketSystem.Api.Events.UseCases;
using TicketSystem.Api.Sales.Interfaces;
using TicketSystem.Api.Sales.Repositories;
using TicketSystem.Api.Sales.UseCases;
using TicketSystem.Api.Shared.Data;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<TicketContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));


// Repositories
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ITicketOrderRepository, TicketOrderRepository>();

// UseCases: Events
builder.Services.AddScoped<CreateEventUseCase>();
builder.Services.AddScoped<GetAllEventsUseCase>();
builder.Services.AddScoped<UpdateEventUseCase>();
builder.Services.AddScoped<DeleteEventUseCase>();

// UseCases: Sales
// criar venda
builder.Services.AddScoped<CreateTicketOrderUseCase>();
// obter venda por id
builder.Services.AddScoped<GetSaleByIdUseCase>();
// total de ingressos vendidos por evento
builder.Services.AddScoped<GetTotalTicketsSoldForEventUseCase>();
// obter vendas por usuário
builder.Services.AddScoped<GetSalesByUserIdUseCase>();
// mudar status da venda
builder.Services.AddScoped<ApproveSaleUseCase>();
builder.Services.AddScoped<RejectSaleUseCase>();
builder.Services.AddScoped<CancelSaleUseCase>();

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
