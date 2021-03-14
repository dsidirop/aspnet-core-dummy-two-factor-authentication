namespace TwoFactorAuth.Services.Auth.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IDummyTwoFactorAuthService
    {
        Task<bool> FirstStageSignInAsync(string firstPassword);
        Task<bool> SecondStageSignInAsync(HttpContext httpContext, string secondPassword, bool isPersistent = false);
    }
}
