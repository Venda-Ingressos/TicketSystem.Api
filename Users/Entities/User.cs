using System;
using TicketSystem.Api.Shared.Entities;
using TicketSystem.Api.Users.ValueObjects;

namespace TicketSystem.Api.Users.Entities
{
    public class User : EntityBase
    {
        public string Name { get; private set; }
        public Email Email { get; private set; }
        protected User() { }

        public User(string name, Email email)
        {
            ValidateDomain(name, email);
            Name = name;
            Email = email;
        }

        // Método para alterar dados, garantindo que a entidade sempre seja válida (DDD)
        public void UpdateInfo(string name, Email email)
        {
            ValidateDomain(name, email);
            Name = name;
            Email = email;
            UpdateTimestamp(); 
        }

        // Regra de negócio/validação dentro da Entidade
        private void ValidateDomain(string name, Email email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do usuário não pode ser vazio.");

            if (email == null)
                throw new ArgumentException("O e-mail é obrigatório.");

            if (name.Length < 3)
                throw new ArgumentException("O nome deve ter pelo menos 3 caracteres.");
        }
    }
}