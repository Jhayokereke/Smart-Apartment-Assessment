using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace SmartApartment.Application.Commons
{
    public class ValidationException : Exception
    {
        public IList<string> ValidationErrors { get; set; }

        public ValidationException(ValidationResult validationResult)
        {
            ValidationErrors = new List<string>();

            foreach (var validationError in validationResult.Errors)
            {
                ValidationErrors.Add(validationError.ErrorMessage);
            }
        }
    }
}
