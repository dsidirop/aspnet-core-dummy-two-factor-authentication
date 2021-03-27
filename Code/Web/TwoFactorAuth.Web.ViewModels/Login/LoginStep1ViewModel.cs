namespace TwoFactorAuth.Web.ViewModels.Login
{
    using System.ComponentModel.DataAnnotations;

    public class LoginStep1ViewModel
    {
        [Required]
        public string Password { get; set; }

        public string HiddenEncodedPassword { get; set; }
    }
}
