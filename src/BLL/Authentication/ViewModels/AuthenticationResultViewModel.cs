using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Authentication.ViewModels
{
    public class AuthenticationResultViewModel
    {
        public string Token { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
