using System;

namespace TicketSystem.Api.Users.ValueObjects
{
    public record Email
    {
        public string Address { get; private set; }

        protected Email() { }

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address) || !address.Contains("@"))
                throw new ArgumentException("Endereço de e-mail inválido.", nameof(address));

            Address = address;
        }
    }
}