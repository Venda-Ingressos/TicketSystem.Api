using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSystem.Api.Users.DTOs;
using TicketSystem.Api.Users.Interfaces;
using TicketSystem.Api.Users.ValueObjects;

namespace TicketSystem.Api.Users.UseCases
{
    public class UpdateUserUseCase
    {
        private readonly IUserRepository _repository;

        public UpdateUserUseCase(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Guid id, UpdateUserRequest request)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            var email = new Email(request.Email);
            user.UpdateInfo(request.Name, email);

            await _repository.UpdateAsync(user);
        }
    }
}
