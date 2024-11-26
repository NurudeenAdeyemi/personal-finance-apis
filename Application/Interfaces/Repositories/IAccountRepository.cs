using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> AddAsync(Account account);
        Task<Account?> FindByICNumberAsync(string icNumber);
    }
}