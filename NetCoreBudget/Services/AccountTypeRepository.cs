using Dapper;
using Microsoft.Data.SqlClient;
using NetCoreBudget.Services.Interfaces;

namespace NetCoreBudget.Services;
public class AccountTypeRepository:IAccountTypeRepository{
    private readonly string connectionString;
    public AccountTypeRepository(IConfiguration configuration)
    {
        connectionString= configuration.GetConnectionString("DefaultConnection");
    }

    public async Task Create(AccountTypeDTO accountType){
        using var connection= new SqlConnection(connectionString);
         var id= await connection.QuerySingleAsync<int>($@"
            INSERT INTO AccountTypes (Name,UserId, [Order])
            Values (@Name, @UserId, 0);
            SELECT SCOPE_IDENTITY();
        ", accountType);

        accountType.Id= id;
    }

    public async Task<IEnumerable<AccountTypeDTO>> Get(int UserId){
        using (SqlConnection dbConnection= new SqlConnection(connectionString)){
            IEnumerable<AccountTypeDTO> userAccountTypeData= 
                await dbConnection.QueryAsync<AccountTypeDTO>(@"
                    SELECT Name,  [Order], Id, UserId
                    FROM AccountTypes
                    WHERE UserId=@UserId
                ", new {UserId});

                return userAccountTypeData;
        }
    }

    public async Task<bool> Exists(string name, int UserId){
        using (SqlConnection connection=new SqlConnection(connectionString)){
            int exists= await connection.QueryFirstOrDefaultAsync<int>(
                @"Select 1 
                  from AccountTypes
                  WHERE NAME = @Name AND UserId = @UserId;", new {name, UserId}
                );
            return exists == 1;
        }
    }

    public async Task Update(AccountTypeDTO accountType){
        using (SqlConnection connection=new SqlConnection(connectionString)){
            int exists= await connection.ExecuteAsync(
                @"
                UPDATE AccountTypes
                SET Name= @Name
                WHERE Id=@Id
                ", accountType);
            return;
        }
    }

    public async Task<AccountTypeDTO> GetAccountTypeById(int id, int userId){
        using (SqlConnection connection=new SqlConnection(connectionString)){
            AccountTypeDTO accountTypeSearched= await connection.QueryFirstOrDefaultAsync<AccountTypeDTO>(
                @"
                Select Id, Name ,[Order]
                From AccountTypes
                Where UserId= @UserId AND Id=@Id
                ", new {userId, id }
            );
            return accountTypeSearched;

        };
    }

    public async Task Delete(int id){
        using (SqlConnection connection= new SqlConnection(connectionString)){
            var delete= await connection.ExecuteAsync(@"
                DELETE FROM AccountTypes
                Where id=@Id
            ", new {id});
        }
    }



}