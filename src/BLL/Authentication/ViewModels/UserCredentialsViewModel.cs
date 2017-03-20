using System.ComponentModel.DataAnnotations;

namespace BLL.Authentication.ViewModels
{
    public class UserCredentialsViewModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
