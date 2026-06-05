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

// Use Cases: Events
builder.Services.AddScoped<CreateEventUseCase>();
builder.Services.AddScoped<GetAllEventsUseCase>();
builder.Services.AddScoped<UpdateEventUseCase>();
builder.Services.AddScoped<DeleteEventUseCase>();

// Use Cases: Sales
builder.Services.AddScoped<CreateTicketOrderUseCase>();
builder.Services.AddScoped<GetSaleByIdUseCase>();
builder.Services.AddScoped<GetTotalTicketsSoldForEventUseCase>();

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
