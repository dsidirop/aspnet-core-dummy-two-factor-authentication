using Microsoft.Extensions.Options;
using TwoFactorAuth.Common.Configuration;

namespace TwoFactorAuth.Services.Data.DummyAuthService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    using TwoFactorAuth.Common;
    using TwoFactorAuth.Data.Common.Repositories;
    using TwoFactorAuth.Data.Models;

    public class DummyTwoFactorAuthService : IDummyTwoFactorAuthService
    {
        private readonly IRepository<ApplicationUser> _repository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOptionsMonitor<AppDummyAuthSpecs> _dummyAuthSpecsOptionsMonitor;

        public DummyTwoFactorAuthService(
            IRepository<ApplicationUser> repository,
            SignInManager<ApplicationUser> signInManager,
            IOptionsMonitor<AppDummyAuthSpecs> dummyAuthSpecsOptionsMonitor
        )
        {
            _repository = repository;
            _signInManager = signInManager;
            _dummyAuthSpecsOptionsMonitor = dummyAuthSpecsOptionsMonitor;
        }

        #region signin methods

        public async Task<bool> FirstStageSignInAsync(string firstPassword)
        {
            var firstDummyAuthUser = await _repository.FindFirstNoTrackingAsync(
                dbQuery: x => x.NormalizedEmail == _dummyAuthSpecsOptionsMonitor.CurrentValue.EmailFirstDummyAuthUser.ToUpper()
            );

            var passwordVerdict = await _signInManager.CheckPasswordSignInAsync(
                user: firstDummyAuthUser,
                password: firstPassword,
                lockoutOnFailure: false
            );

            return passwordVerdict?.Succeeded ?? false;
        }

        public async Task<bool> SecondStageSignInAsync(HttpContext httpContext, string secondPassword, bool isPersistent = false)
        {
            var secondStageDummyUser = await _repository.FindFirstNoTrackingAsync(
                dbQuery: x => x.NormalizedEmail == _dummyAuthSpecsOptionsMonitor.CurrentValue.EmailEventualDummyAuthUser.ToUpper()
            );
            if (secondStageDummyUser == null)
                return false; //wops  how did this happen?  faulty db?

            var passwordVerdict = await _signInManager.CheckPasswordSignInAsync(
                user: secondStageDummyUser,
                password: secondPassword,
                lockoutOnFailure: false
            );
            if (!(passwordVerdict?.Succeeded ?? false))
                return false;
            
            var principal = new ClaimsPrincipal(new ClaimsIdentity(
                claims: GetUserClaims(secondStageDummyUser),
                authenticationType: IdentityApplication
            ));

            await httpContext.SignInAsync( //todo   abstract this away
                scheme: IdentityApplication,
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

        private const string IdentityApplication = "Identity.Application";
    }
}
