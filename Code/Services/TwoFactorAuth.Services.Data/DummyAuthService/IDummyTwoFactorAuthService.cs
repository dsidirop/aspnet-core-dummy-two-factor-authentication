using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TwoFactorAuth.Services.Data.DummyAuthService
{
    public interface IDummyTwoFactorAuthService
    {
        Task<bool> SecondStageSignInAsync(HttpContext httpContext, string secondPassword, bool isPersistent = false);
        bool FirstStageSignIn(string firstPassword);
    }
}
