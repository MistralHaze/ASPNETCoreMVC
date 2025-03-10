using System.ComponentModel.DataAnnotations;
using NetCoreBudget.Validations;

namespace NetCoreBudget.Models;

public class Account{
    public int Id { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [StringLength(maximumLength:50)]
    [FirstLetterMayus]
    public string Name { get; set; }
    [Display(Name ="Account type")]
    public int AccountTypeId { get; set; }
    public string AccountType { get; set; }
    public decimal Balance { get; set; }

    [StringLength(maximumLength:1000)]
    public string Description { get; set; }

     override public string ToString (){
        return @$"Id {Id},
                  Name {Name},
                  AccountTypeId {AccountTypeId},
                  Balance {Balance},
                  Description {Description}    
                ";

        
    }
}