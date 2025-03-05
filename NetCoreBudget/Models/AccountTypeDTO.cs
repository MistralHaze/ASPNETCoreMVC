using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using NetCoreBudget.Validations;

//TODO: Is this really a DTO or just a model object? read more about it
public class AccountTypeDTO/*:IValidatableObject*/{
    

    public int Id{get;set;}
    [Required (ErrorMessage = "{0} field is required")]
    [FirstLetterMayus] //Uses custom attribute FirstLetterMayus
    [Remote(action:"VerifyAccountTypeExists", controller:"AccountTypes")] //Uses javascript to remotely call the action in the front. Used for validation
    public string Name{get; set;}
    public int UserId {get; set;}
    public int Order {get; set;}
/*
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(Name is not null &&  Name.Length>0){
            char firstLetter = Name[0];
            if(!char.IsUpper(firstLetter)){
                yield return new ValidationResult("first letter must be mayus", [nameof(Name)]);
            }
        } 
    }*/
    override public string ToString (){
        return @$"Id {Id},
                  Name {Name},
                  UserId {UserId},
                  Order {Order.ToString()}    
                ";

        
    }
}