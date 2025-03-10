using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using NetCoreBudget.Models;
using NetCoreBudget.Services.Interfaces;

namespace NetCoreBudget.Controllers;

public class AccountsController: Controller{
    private readonly IAccountTypeRepository accountTypeRepository;
    private readonly IUserService userService;
    private readonly IAccountRepository accountRepository;

    public AccountsController(IAccountTypeRepository accountTypeRepository, IUserService userService, IAccountRepository accountRepository)
    {
        this.accountTypeRepository = accountTypeRepository;
        this.userService = userService;
        this.accountRepository = accountRepository;
    }

    public async Task<IActionResult> Index(){
        int userId=userService.GetUserId();
        IEnumerable<Account> accountWithAccountType= await accountRepository.GetWithUserId(userId);

        IEnumerable<AccountsIndexViewModel> indexModel= accountWithAccountType.GroupBy(acc=>acc.AccountType)
                                                                .Select(accountGrouped=>{
                                                                    return new AccountsIndexViewModel{
                                                                        AccountType= accountGrouped.Key,
                                                                        Accounts= accountGrouped.AsEnumerable()
                                                                    };
                                                                }).ToList();
        return View(indexModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create(){
        int userId= userService.GetUserId();
        var model= new AccountCreationViewModel();
        model.AccountTypes= await GetAccountTypesListItem(userId);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AccountCreationViewModel account){

        Debug.WriteLine("Received");
        Debug.WriteLine(account.ToString());         
        
        int userId= userService.GetUserId();
        AccountTypeDTO accountTypes= await accountTypeRepository.GetAccountTypeById(account.AccountTypeId,userId);

        if(accountTypes is null)
        {
            return RedirectToAction("Not found", "Home");
        }

        //If not valid we need to resend the account types select items.
        if(!ModelState.IsValid){
            account.AccountTypes= await GetAccountTypesListItem(userId);
            return View(account);
        }

        await accountRepository.Create(account);
        return RedirectToAction("Index");
    }

    private async Task<IEnumerable<SelectListItem>> GetAccountTypesListItem(int userId){
        IEnumerable<AccountTypeDTO> accountTypes = await accountTypeRepository.Get(userId);
        return  accountTypes.Select(x=> new SelectListItem(x.Name,x.Id.ToString()));
    }
    
}