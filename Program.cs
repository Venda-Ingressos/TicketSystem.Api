using Microsoft.EntityFrameworkCore;
using TicketSystem.Api.Events.Interfaces;
using TicketSystem.Api.Events.Repositories;
using TicketSystem.Api.Events.UseCases; // <-- Adicionamos esse using!
using TicketSystem.Api.Shared.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuração do banco de dados SQLite usando o seu TicketContext
builder.Services.AddDbContext<TicketContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 1. Registro do Repositório
builder.Services.AddScoped<IEventRepository, EventRepository>();

// 2. Registro de TODOS os UseCases do CRUD
builder.Services.AddScoped<CreateEventUseCase>();
builder.Services.AddScoped<GetAllEventsUseCase>();
builder.Services.AddScoped<UpdateEventUseCase>();
builder.Services.AddScoped<DeleteEventUseCase>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();