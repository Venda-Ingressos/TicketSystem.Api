using System;
using TicketSystem.Api.Events.ValueObjects;
using TicketSystem.Api.Shared.Entities;

namespace TicketSystem.Api.Events.Entities
{
    public class Event : EntityBase
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime Date { get; private set; }
        public int TotalCapacity { get; private set; }
        public Money TicketPrice { get; private set; }

        protected Event() { }

        public Event(string title, string description, DateTime date, int totalCapacity, Money ticketPrice)
        {
            ValidateDomain(title, date, totalCapacity);
            Title = title;
            Description = description;
            Date = date;
            TotalCapacity = totalCapacity;
            TicketPrice = ticketPrice;
        }

        // Valida se o evento já aconteceu (impede a compra)
        public bool IsPastEvent()
        {
            // Compara a data do evento com a data atual (UTC para evitar problemas de fuso)
            return Date < DateTime.UtcNow;
        }

        // Valida se ainda há capacidade disponível para a quantidade solicitada
        public bool HasAvailableCapacity(int ticketsAlreadySold, int requestedQuantity)
        {
            return (ticketsAlreadySold + requestedQuantity) <= TotalCapacity;
        }

        private void ValidateDomain(string title, DateTime date, int totalCapacity)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("O título do evento é obrigatório.");

            if (totalCapacity <= 0)
                throw new ArgumentException("A capacidade total deve ser maior que zero.");

            if (date < DateTime.UtcNow)
                throw new ArgumentException("A data do evento deve ser no futuro.");
        }
    }
}