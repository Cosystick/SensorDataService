using System;
using System.ComponentModel.DataAnnotations;
using SensorService.UI.Pages;

namespace SensorService.UI.Validations
{
    public class PasswordCheck: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(validationContext.ObjectInstance is IPasswordModel model))
            {
                throw new ArgumentException("Validator needs to implement IPasswordModel");
            }

            if (!string.IsNullOrEmpty(model.Password) && model.Password != model.PasswordCheck)
            {
                return new ValidationResult("Passwords does not match");
            }

            return ValidationResult.Success;
        }
    }
}
