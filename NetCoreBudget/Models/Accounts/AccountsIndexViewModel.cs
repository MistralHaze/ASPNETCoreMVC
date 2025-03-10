namespace NetCoreBudget.Models;

public class AccountsIndexViewModel{
    public string AccountType {get; set;}
    public IEnumerable<Account> Accounts{get; set;}
    public decimal Balance  {get {
        if(Accounts.Count()>0) 
            return Accounts.Sum(x=>x.Balance);
        return 0;
        }
    }
}