namespace NetCoreBudget.Services.Interfaces;

public interface IAccountTypeRepository{
     public Task Create(AccountTypeDTO accountType);
     public Task<bool> Exists(string name, int UserId);
     public Task<IEnumerable<AccountTypeDTO>> Get(int UserId);
     public  Task Update(AccountTypeDTO accountType);
     public Task<AccountTypeDTO> GetAccountTypeById(int id, int userId);
     public Task Delete(int id);
     public Task Order(IEnumerable<AccountTypeDTO> accountTypeOrdered);
}