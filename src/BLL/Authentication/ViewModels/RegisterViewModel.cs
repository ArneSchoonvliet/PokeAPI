using System.ComponentModel.DataAnnotations;

namespace BLL.Authentication.ViewModels
{
    public class RegisterViewModel : UserCredentialsViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
