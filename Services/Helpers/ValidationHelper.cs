using Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        public static void ModelValidation(object model)
        {            
            ValidationContext validationContext = new ValidationContext(model);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
            {
                string errorMessage = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                throw new ArgumentException(errorMessage);
            }
        }
    }
}
