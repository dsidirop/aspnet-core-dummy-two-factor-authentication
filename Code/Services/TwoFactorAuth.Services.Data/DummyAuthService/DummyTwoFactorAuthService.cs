namespace TwoFactorAuth.Services.Data.DummyAuthService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Http;

    using TwoFactorAuth.Common;
    using TwoFactorAuth.Data.Common.Repositories;
    using TwoFactorAuth.Data.Models;

    public class DummyTwoFactorAuthService : IDummyTwoFactorAuthService
    {
        private readonly IRepository<ApplicationUser> _repository;

        public DummyTwoFactorAuthService(IRepository<ApplicationUser> repository)
        {
            _repository = repository;
        }
        
        #region signin methods

        public bool FirstStageSignIn(string firstPassword)
        {
            return firstPassword == GlobalConstants.DummyAuthUserSpecs.Passwords.First;
        }

        public async Task<bool> SecondStageSignInAsync(HttpContext httpContext, string secondPassword, bool isPersistent = false)
        {
            if (secondPassword != GlobalConstants.DummyAuthUserSpecs.Passwords.Second)
                return false;

            var dbUserData = await _repository.FindFirstNoTrackingAsync(dbQuery: x => x.Email == GlobalConstants.DummyAuthUserSpecs.Email.ToUpper());
            if (dbUserData == null)
                return false; //wops  how did this happen?  faulty db?

            var principal = new ClaimsPrincipal(new ClaimsIdentity(
                claims: GetUserClaims(dbUserData),
                authenticationType: "Identity.Application"
            ));

            await httpContext.SignInAsync( //todo   abstract this away
                scheme: "Identity.Application",
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

        static private IEnumerable<Claim> GetUserClaims(ApplicationUser user)
        {
            return GetBaseClaims(user).Concat(GetUserRoleClaims(user));
        }

        static private IEnumerable<Claim> GetBaseClaims(ApplicationUser user)
        {
            yield return new Claim(type: ClaimTypes.NameIdentifier, value: user.Id);
            yield return new Claim(type: ClaimTypes.Name, value: user.NormalizedUserName);
            yield return new Claim(type: ClaimTypes.Email, value: user.NormalizedEmail);
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
