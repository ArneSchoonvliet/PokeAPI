using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Linq;

namespace BLL.Extensions.Dictionary
{
    public static class IdentityResultExtensions
    {
        public static Hashtable ToHashtable(this IdentityResult result)
        {
            var errors = new Hashtable();
            foreach (var error in result.Errors)
            {
                errors[error.Code] = result.Errors.Where(e => e.Code == error.Code).Select(e => e.Description).ToList();
            }
            return errors;
        }
    }
}
