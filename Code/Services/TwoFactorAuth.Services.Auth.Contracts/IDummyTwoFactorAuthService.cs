using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TwoFactorAuth.Services.Auth.Contracts
{
    public interface IDummyTwoFactorAuthService
    {
        Task<bool> FirstStageSignInAsync(string firstPassword);
        Task<bool> SecondStageSignInAsync(HttpContext httpContext, string secondPassword, bool isPersistent = false);
    }
}
