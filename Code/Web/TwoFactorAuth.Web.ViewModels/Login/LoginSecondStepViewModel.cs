namespace TwoFactorAuth.Web.ViewModels.Login
{
    using System.ComponentModel.DataAnnotations;

    public class LoginSecondStepViewModel
    {
        [Required]
        public string Password { get; set; }

        public string PasswordHintImagePath { get; set; }
    }
}
