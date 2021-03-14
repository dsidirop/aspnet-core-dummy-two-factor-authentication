namespace TwoFactorAuth.Services.Auth.DummyAuth
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using TwoFactorAuth.Common.Contracts.Configuration;
    using TwoFactorAuth.Data.Common.Repositories;
    using TwoFactorAuth.Data.Models;
    using TwoFactorAuth.Services.Auth.Contracts;

    public class DummyTwoFactorAuthService : IDummyTwoFactorAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<ApplicationUser> _repository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<DummyTwoFactorAuthService> _logger;
        private readonly IOptionsMonitor<AppDummyAuthSpecs> _dummyAuthSpecsOptionsMonitor;

        public DummyTwoFactorAuthService(
            IHttpContextAccessor httpContextAccessor,
            IRepository<ApplicationUser> repository,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DummyTwoFactorAuthService> logger,
            IOptionsMonitor<AppDummyAuthSpecs> dummyAuthSpecsOptionsMonitor)
        {
            _logger = logger;
            _repository = repository;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _dummyAuthSpecsOptionsMonitor = dummyAuthSpecsOptionsMonitor;
        }

        #region signin methods

        public async Task<bool> FirstStageSignInAsync(string firstPassword)
        {
            var firstDummyAuthUser = await _repository.FindFirstNoTrackingAsync(
                dbQuery: x => x.NormalizedEmail == _dummyAuthSpecsOptionsMonitor.CurrentValue.DummyUsers.First.Email.ToUpper()
            );
            if (firstDummyAuthUser == null)
            {
                _logger.LogCritical($"[DTFAS.FSSIA01] [BUG] Wops! Failed to get hold of the first dummy-auth user with email '{_dummyAuthSpecsOptionsMonitor.CurrentValue.DummyUsers.First.Email}' (Did db-migrations run properly on startup?)");
                return false;
            }

            var passwordVerdict = await _signInManager.CheckPasswordSignInAsync(
                user: firstDummyAuthUser,
                password: firstPassword,
                lockoutOnFailure: false
            );

            return passwordVerdict?.Succeeded ?? false;
        }

        public async Task<bool> SecondStageSignInAsync(string secondPassword, bool isPersistent = false)
        {
            var secondStageDummyUser = await _repository.FindFirstNoTrackingAsync(
                dbQuery: x => x.NormalizedEmail == _dummyAuthSpecsOptionsMonitor.CurrentValue.DummyUsers.Second.Email.ToUpper()
            );
            if (secondStageDummyUser == null)
            {
                _logger.LogCritical($"[DTFAS.SSSIA01] [BUG] Wops! Failed to get hold of the second dummy-auth user with email '{_dummyAuthSpecsOptionsMonitor.CurrentValue.DummyUsers.Second.Email}' (Did db-migrations run properly on startup?)");
                return false;
            }

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

            await _httpContextAccessor.HttpContext.SignInAsync(
                scheme: IdentityApplication,
                principal: principal,
                properties: new AuthenticationProperties {IsPersistent = true}
            );

            return true;
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
