namespace TwoFactorAuth.Services.Data.DummyAuthService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Http;

    using TwoFactorAuth.Data.Common.Repositories;
    using TwoFactorAuth.Data.Models;

    public class DummyTwoFactorAuthService : IDummyTwoFactorAuthService
    {
        private IRepository<ApplicationUser> _repository;

        static private class Passwords
        {
            internal const string First = "123";
            internal const string Second = "123";
        }

        public DummyTwoFactorAuthService(IRepository<ApplicationUser> repository)
        {
            _repository = repository;
        }

        
        #region signin methods

        public bool FirstStageSignIn(string firstPassword)
        {
            return firstPassword == Passwords.First;
        }

        public async Task<bool> SecondStageSignInAsync(HttpContext httpContext, string secondPassword, bool isPersistent = false)
        {
            if (secondPassword != Passwords.Second)
                return false;

            var dbUserData = new ApplicationUser(); //todo  get this from the db

            var principal = new ClaimsPrincipal(new ClaimsIdentity(
                claims: GetUserClaims(dbUserData),
                authenticationType: CookieAuthenticationDefaults.AuthenticationScheme
            ));

            await httpContext.SignInAsync(
                scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                principal: principal,
                properties: new AuthenticationProperties {IsPersistent = true}
            );

            return true;
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        #endregion signinmethods
        

        #region helpers

        private IEnumerable<Claim> GetUserClaims(ApplicationUser user)
        {
            return GetBaseClaims(user).Concat(GetUserRoleClaims(user));
        }

        static private IEnumerable<Claim> GetBaseClaims(ApplicationUser user)
        {
            yield return new Claim(type: ClaimTypes.NameIdentifier, value: user.Id);
            yield return new Claim(type: ClaimTypes.Name, value: user.NormalizedUserName);
            yield return new Claim(type: ClaimTypes.Email, value: user.Email);
        }

        static private IEnumerable<Claim> GetUserRoleClaims(ApplicationUser user)
        {
            return user
                .Roles
                .Select(
                    x => new Claim(
                        type: ClaimTypes.Role,
                        value: x.RoleId
                    )
                );
        }

        #endregion helpers
    }
}
