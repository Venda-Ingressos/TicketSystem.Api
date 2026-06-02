using System;
using System.Threading.Tasks;
using TicketSystem.Api.Users.Entities;

namespace TicketSystem.Api.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}