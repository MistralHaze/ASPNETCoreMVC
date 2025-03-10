using Dapper;
using Microsoft.Data.SqlClient;
using NetCoreBudget.Models;
using NetCoreBudget.Services.Interfaces;

namespace NetCoreBudget.Services;

public class AccountRepository:IAccountRepository{
    private readonly string connectionString;

    public AccountRepository(IConfiguration configuration){
        connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task Create(Account account){
        using (SqlConnection connection= new SqlConnection(connectionString)){
            int generatedId= await connection.QuerySingleAsync<int>(@"
                INSERT INTO Accounts (Name, AccountTypeId, Balance, Description)
                VALUES(@Name, @AccountTypeId, @Balance, @Description)

                SELECT SCOPE_IDENTITY();
            ", account);
            account.Id=generatedId;
        }
    }

    public async Task<IEnumerable<Account>> GetWithUserId(int userId){
        using (SqlConnection connection = new SqlConnection(connectionString)){
            var accounts= await connection.QueryAsync<Account>(@"
                SELECT Accounts.id, Accounts.Name, AccTypes.Name As AccountType, Balance
                FROM Accounts
                INNER JOIN AccountTypes AccTypes On AccTypes.Id= Accounts.AccountTypeId
                Where AccTypes.UserId=@UserId
                Order by AccTypes.[Order]
            ", new{userId});
            return accounts;
        }    
    } 
}