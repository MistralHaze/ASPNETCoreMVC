using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreBudget.Models;
using NetCoreBudget.Services.Interfaces;

namespace NetCoreBudget.Controllers;

public class AccountsController: Controller{
    private readonly IAccountTypeRepository accountTypeRepository;
    private readonly IUserService userService;

    public AccountsController(IAccountTypeRepository accountTypeRepository, IUserService userService)
    {
        this.accountTypeRepository = accountTypeRepository;
        this.userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Create(){
        int userId= userService.GetUserId();
        IEnumerable<AccountTypeDTO> accountTypes = await accountTypeRepository.Get(userId);
        var model= new AccountCreationViewModel();
        model.AccountTypes= accountTypes.Select(x=> new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(x.Name,x.Id.ToString()));
        return View(model);
    }
    
}