using BLL.Authentication.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Authentication.Interfaces
{
    public interface IAuthenticationManager
    {
        Task<AuthenticationResultViewModel> RegisterUser(RegisterViewModel credentials);
        Task<AuthenticationResultViewModel> LoginUser(UserCredentialsViewModel credentials);
    }
}
