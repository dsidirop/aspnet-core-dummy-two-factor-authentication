using System.ComponentModel.DataAnnotations;

namespace TwoFactorAuth.Web.ViewModels.Login
{
    public class LoginStep2ViewModel
    {
        [Required]
        public string Password { get; set; }
    }
}
