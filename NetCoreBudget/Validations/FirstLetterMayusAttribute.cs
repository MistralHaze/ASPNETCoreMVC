using System.ComponentModel.DataAnnotations;

namespace NetCoreBudget.Validations{

    public class FirstLetterMayusAttribute:ValidationAttribute{

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            if(value is null) return ValidationResult.Success;
            if(!(value is string)) return new ValidationResult("Not a name");
            else {
            string name= value as string;
            char firstLetter=name[0];
            bool IsUpper= char.IsUpper(firstLetter);
            if(!IsUpper) { return new ValidationResult("Not starting on a mayus");}
            }
            return ValidationResult.Success;
             
        }
    }
}