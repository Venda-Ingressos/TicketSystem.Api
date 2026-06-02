using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Api.Events.Interfaces;
using TicketSystem.Api.Events.ValueObjects;
using TicketSystem.Api.Shared.Data;

using DomainEvent = TicketSystem.Api.Events.Entities.Event;
using SharedEvent = TicketSystem.Api.Shared.Entities.Event;

namespace TicketSystem.Api.Events.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly TicketContext _context;

        public EventRepository(TicketContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task AddAsync(DomainEvent eventEntity)
        {
            var sharedEvent = new SharedEvent
            {
                // Como é uma criação nova, deixamos o banco gerar o Id,
                // ou usamos o Id que já veio na Entidade Rica (recomendado se você gerar o Id antes)
                Title = eventEntity.Title,
                Description = eventEntity.Description,
                Date = eventEntity.Date,
                TotalCapacity = eventEntity.TotalCapacity,
                TicketPrice = eventEntity.TicketPrice.Amount
            };

            await _context.Events.AddAsync(sharedEvent);
            await _context.SaveChangesAsync();
        }

        // READ (Apenas um)
        public async Task<DomainEvent> GetByIdAsync(Guid id)
        {
            var sharedEvent = await _context.Events.FindAsync(id);
            if (sharedEvent == null) return null;

            // Injetamos o Id que veio do banco na nossa Entidade Rica
            return new DomainEvent(
                sharedEvent.Title,
                sharedEvent.Description,
                sharedEvent.Date,
                sharedEvent.TotalCapacity,
                new Money(sharedEvent.TicketPrice)
            )
            {
                Id = sharedEvent.Id // <-- CORREÇÃO AQUI
            };
        }

        // READ (Todos)
        public async Task<IEnumerable<DomainEvent>> GetAllAsync()
        {
            var sharedEvents = await _context.Events.ToListAsync();

            // Traduz a lista anêmica do banco para uma lista rica do domínio e injeta o Id
            return sharedEvents.Select(sharedEvent => new DomainEvent(
                sharedEvent.Title,
                sharedEvent.Description,
                sharedEvent.Date,
                sharedEvent.TotalCapacity,
                new Money(sharedEvent.TicketPrice)
            )
            {
                Id = sharedEvent.Id // <-- CORREÇÃO AQUI
            });
        }

        // UPDATE
        public async Task UpdateAsync(DomainEvent eventEntity)
        {
            // Agora buscamos pelo ID correto e exato, e não mais pelo Título!
            var sharedEvent = await _context.Events.FindAsync(eventEntity.Id);

            if (sharedEvent != null)
            {
                // O Título também pode ser alterado, então adicionamos ele aqui
                sharedEvent.Title = eventEntity.Title;
                sharedEvent.Description = eventEntity.Description;
                sharedEvent.Date = eventEntity.Date;
                sharedEvent.TotalCapacity = eventEntity.TotalCapacity;
                sharedEvent.TicketPrice = eventEntity.TicketPrice.Amount;

                _context.Events.Update(sharedEvent);
                await _context.SaveChangesAsync();
            }
        }

        // DELETE
        public async Task DeleteAsync(Guid id)
        {
            var sharedEvent = await _context.Events.FindAsync(id);
            if (sharedEvent != null)
            {
                _context.Events.Remove(sharedEvent);
                await _context.SaveChangesAsync();
            }
        }
    }
}