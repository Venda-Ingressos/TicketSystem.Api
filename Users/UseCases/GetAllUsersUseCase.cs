using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSystem.Api.Users.Entities;
using TicketSystem.Api.Users.Interfaces;

namespace TicketSystem.Api.Users.UseCases
{
    public class GetUserByEmailUseCase
    {
        private readonly IUserRepository _repository;

        public GetUserByEmailUseCase(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> ExecuteAsync(string email)
        {
            var user = await _repository.GetByEmailAsync(email);

            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            return user;
        }
    }
}
