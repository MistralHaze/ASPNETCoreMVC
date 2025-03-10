using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetCoreBudget.Models;

public class AccountCreationViewModel: Account{
    public IEnumerable<SelectListItem> AccountTypes{ get; set; }
}