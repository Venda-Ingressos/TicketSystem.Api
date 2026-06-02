using System;

namespace TicketSystem.Api.Events.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; private set; }
        protected Money() { }

        public Money(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("O valor não pode ser negativo.", nameof(amount));

            Amount = amount;
        }
    }
}