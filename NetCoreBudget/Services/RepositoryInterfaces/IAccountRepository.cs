using NetCoreBudget.Models;

namespace NetCoreBudget.Services.Interfaces;

public interface IAccountRepository{
    public Task Create(Account account);
    public Task<IEnumerable<Account>> GetWithUserId(int userId);
}