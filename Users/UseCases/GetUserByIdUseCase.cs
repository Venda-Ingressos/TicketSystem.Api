using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSystem.Api.Users.Entities;
using TicketSystem.Api.Users.Interfaces;

namespace TicketSystem.Api.Users.UseCases
{
    public class GetUserByIdUseCase
    {
        private readonly IUserRepository _repository;

        public GetUserByIdUseCase(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> ExecuteAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            return user;
        }
    }
}
