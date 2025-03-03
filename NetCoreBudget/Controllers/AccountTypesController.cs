
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NetCoreBudget.Models;
using NetCoreBudget.Services;
using NetCoreBudget.Services.Interfaces;

namespace NetCoreBudget.Controllers;

public class AccountTypesController: Controller{
    private readonly ILogger<AccountTypesController> _logger;
    private readonly string connectionString;
    private readonly IAccountTypeRepository accountTypeRepository;
    private readonly IUserService userService;
    public AccountTypesController(ILogger<AccountTypesController> logger,
     IAccountTypeRepository accountTypeRepository,
     IUserService userService){
        _logger = logger;
        this.accountTypeRepository = accountTypeRepository;
        this.userService= userService;
    }

    //Index name is normally used for a view that is going to show a list of elements
    public async Task<IActionResult> Index(){
        int userId=userService.GetUserId();
        IEnumerable<AccountTypeDTO> accountTypes= await accountTypeRepository.Get(userId);
        return View(accountTypes);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AccountTypeDTO accountType){
        if(!ModelState.IsValid) return View(accountType);

        accountType.UserId=userService.GetUserId();
        Debug.WriteLine(accountType.ToString());

        bool alreadyInDatabase=await accountTypeRepository.Exists(accountType.Name, accountType.UserId);
        if(alreadyInDatabase)
        {
            ModelState.AddModelError(nameof(accountType.Name), $"Data {accountType.Name} already in database");
            return View(accountType);
        }
        await accountTypeRepository.Create(accountType);
        
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> VerifyAccountTypeExists(string name){
        var UserId=userService.GetUserId();
        bool alreadyInDatabase=await accountTypeRepository.Exists(name, UserId);
        if(alreadyInDatabase){
            return Json($"Name {name} already exists");
        }

        return Json(true);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id){
        int userId= userService.GetUserId();
        AccountTypeDTO accountType= await accountTypeRepository.GetAccountTypeById(id,userId);

        if(accountType==null){
            //User does not have permit or access 
            return RedirectToAction("Not found", "Home");
        }

        return View(accountType);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AccountTypeDTO accountType)
    {
        //We are always using the userService because the User can always send malicious data. We cannot trust the user Data
        int userId= userService.GetUserId();
        AccountTypeDTO accountExists= await accountTypeRepository.GetAccountTypeById(userId, accountType.UserId);

        if(accountExists==null){
             //User does not have permit or access 
            return RedirectToAction("Not found", "Home");
        }
        await accountTypeRepository.Update(accountType);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id){
        
        _logger.LogDebug("entro a delete");
        int userId= userService.GetUserId();
         AccountTypeDTO accountType= await accountTypeRepository.GetAccountTypeById(id, userId);

        if(accountType==null){
             //User does not have permit or access 
            return RedirectToAction("Not found", "Home");
        }
        return View(accountType);
    }

    [HttpPost]
     public async Task<IActionResult> DeleteAccountType(int id){
        int userId= userService.GetUserId();
         AccountTypeDTO accountExists= await accountTypeRepository.GetAccountTypeById(id, userId);

        if(accountExists==null){
             //User does not have permit or access 
            return RedirectToAction("Not found", "Home");
        }
        await accountTypeRepository.Delete(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult OrderRows([FromBody]int[] value){
        _logger.LogInformation("si llega! Ids is " + value);
        if(value==null) return Error();
        for(int i=0;i<value.Length;i++){
            _logger.LogInformation(value.ToString());
        }
        return Ok();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(){
        return View( new ErrorViewModel{
            RequestId= Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}