using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Account> AddAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            return account;
        }

        public Task<Account> UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            return Task.FromResult(account);
        }
        public async Task<Account?> FindByICNumberAsync(string icNumber)
        {
            return await _context.Accounts.FirstOrDefaultAsync(c => c.ICNumber == icNumber);
        }
    }
}