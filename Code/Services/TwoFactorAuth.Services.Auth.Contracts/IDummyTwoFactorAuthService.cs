namespace TwoFactorAuth.Services.Auth.Contracts
{
    using System.Threading.Tasks;

    public interface IDummyTwoFactorAuthService
    {
        Task<bool> FirstStageSignInAsync(string firstPassword);
        Task<bool> SecondStageSignInAsync(string secondPassword, bool isPersistent = false);
    }
}
