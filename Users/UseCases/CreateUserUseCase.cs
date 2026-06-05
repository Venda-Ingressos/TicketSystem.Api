using System;
using System.Threading.Tasks;
using TicketSystem.Api.Users.DTOs;
using TicketSystem.Api.Users.Interfaces;
using TicketSystem.Api.Users.ValueObjects;

using DomainUser = TicketSystem.Api.Users.Entities.User;

namespace TicketSystem.Api.Users.UseCases
{
    public class CreateUserUseCase
    {
        private readonly IUserRepository _repository;

        public CreateUserUseCase(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> ExecuteAsync(CreateUserRequest request)
        {
            var email = new Email(request.Email);
            var user = new DomainUser(request.Name, email);

            await _repository.AddAsync(user);
            return user.Id;
        }
    }
}
