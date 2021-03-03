using System.ComponentModel.DataAnnotations;

namespace TwoFactorAuth.Web.ViewModels.DummyAuthUserLogin
{
    public class LoginStep1ViewModel
    {
        [Required]
        public string Password { get; set; }

        public string HiddenEncodedPassword { get; set; }
    }
}
