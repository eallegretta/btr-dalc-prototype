using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace System.Web.Mvc
{
    public static class ModelStateDictionaryExtensions
    {
        public static void AddValidationResults(this ModelStateDictionary modelState, IEnumerable<ValidationResult> validationResults)
        {
            foreach (var result in validationResults)
            {
                string memberName = result.MemberNames.FirstOrDefault();

                if (memberName == null)
                    memberName = Guid.NewGuid().ToString();

                modelState.AddModelError(memberName, result.ErrorMessage);
            }
        }
    }
}