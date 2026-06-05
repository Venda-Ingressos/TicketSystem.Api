using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Api.Shared.Data;
using TicketSystem.Api.Users.Interfaces;
using TicketSystem.Api.Users.ValueObjects;

using DomainUser = TicketSystem.Api.Users.Entities.User;
using SharedUser = TicketSystem.Api.Shared.Entities.User;

namespace TicketSystem.Api.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TicketContext _context;

        public UserRepository(TicketContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DomainUser user)
        {
            var sharedUser = new SharedUser
            {
                Id = user.Id == Guid.Empty ? Guid.NewGuid() : user.Id,
                Name = user.Name,
                Email = user.Email.Address
            };

            await _context.Users.AddAsync(sharedUser);
            await _context.SaveChangesAsync();
            user.Id = sharedUser.Id;
        }

        public async Task<DomainUser> GetByIdAsync(Guid id)
        {
            var sharedUser = await _context.Users.FindAsync(id);
            if (sharedUser == null) return null;

            return new DomainUser(sharedUser.Name, new Email(sharedUser.Email))
            {
                Id = sharedUser.Id
            };
        }

        public async Task<DomainUser> GetByEmailAsync(string email)
        {
            var sharedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (sharedUser == null) return null;

            return new DomainUser(sharedUser.Name, new Email(sharedUser.Email))
            {
                Id = sharedUser.Id
            };
        }

        public async Task UpdateAsync(DomainUser user)
        {
            var sharedUser = await _context.Users.FindAsync(user.Id);

            if (sharedUser != null)
            {
                sharedUser.Name = user.Name;
                sharedUser.Email = user.Email.Address;

                _context.Users.Update(sharedUser);
                await _context.SaveChangesAsync();
            }
        }
    }
}

