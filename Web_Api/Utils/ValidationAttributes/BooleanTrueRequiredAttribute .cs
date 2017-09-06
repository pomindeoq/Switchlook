using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApi.Utils.ValidationAttributes
{
    public class BooleanRequiredAttribute : ValidationAttribute
    {
        private bool _compareTo;
        public BooleanRequiredAttribute(bool compareTo)
        {
            _compareTo = compareTo;
        }


        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if(!(value is bool))
                throw new ArgumentException("Attribute can only be applied on booleans");

            if ((bool)value != _compareTo)
                return new ValidationResult(GetErrorMessage(validationContext));

            return ValidationResult.Success;
        }

        private string GetErrorMessage(ValidationContext validationContext)
        {
            // Message that was supplied
            if (!string.IsNullOrEmpty(this.ErrorMessage))
                return this.ErrorMessage;

            // Use generic message: i.e. The field {0} is invalid
            //return this.FormatErrorMessage(validationContext.DisplayName);

            // Custom message
            return $"{validationContext.DisplayName} is not valid";
        }
    }
}
