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
        //Using stored procedure: AccountTypes_Insert
         var id= await connection.QuerySingleAsync<int>("AccountTypes_Insert", 
                                                        new{Name=accountType.Name, UserId=accountType.UserId}, 
                                                        commandType: System.Data.CommandType.StoredProcedure);

        accountType.Id= id;
    }

    public async Task<IEnumerable<AccountTypeDTO>> Get(int UserId){
        using (SqlConnection dbConnection= new SqlConnection(connectionString)){
            IEnumerable<AccountTypeDTO> userAccountTypeData= 
                await dbConnection.QueryAsync<AccountTypeDTO>(@"
                    SELECT Name,  [Order], Id, UserId
                    FROM AccountTypes
                    WHERE UserId=@UserId
                    ORDER BY [ORDER]
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

    public async Task Order(IEnumerable<AccountTypeDTO> accountTypeOrdered){
        string query=@" UPDATE AccountTypes
                        SET [order]=@Order
                        WHERE id=@Id";
        using(SqlConnection connection= new SqlConnection(connectionString)){
            foreach(AccountTypeDTO account in accountTypeOrdered){
                await connection.ExecuteAsync(query, new{account.Order, account.Id});
            }
            //could be replaced without the foreach with:
            //await connection.ExecuteAsync(query, accountTypeOrdered);
            //since dapper would automatically make a call for each IEnumerable entry 
        }

    }



}