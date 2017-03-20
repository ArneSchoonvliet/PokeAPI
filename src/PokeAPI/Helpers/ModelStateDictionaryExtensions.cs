using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;
using System.Linq;

namespace PokeAPI.Helpers
{
    public static class ModelStateDictionaryExtensions
    {
        public static Hashtable ToHashtable(this ModelStateDictionary modelState)
        {
            var errors = new Hashtable();
            foreach (var pair in modelState)
            {
                if (pair.Value.Errors.Count > 0)
                {
                    errors[pair.Key] = pair.Value.Errors.Select(error => error.ErrorMessage).ToList();
                }
            }
            return errors;
        }
    }
}
