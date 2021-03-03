using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TwoFactorAuth.Services.Data.DummyAuthService
{
    public interface IDummyTwoFactorAuthService
    {
        bool FirstStageSignIn(string firstPassword);
        Task<bool> SecondStageSignInAsync(HttpContext httpContext, string secondPassword, bool isPersistent = false);
    }
}
