using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TwoFactorAuth.Common;
using TwoFactorAuth.Data.Models;

namespace TwoFactorAuth.Data.Seeding
{
    internal class BaselineUsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            await SeedDummyAuthUserAsync(userManager: userManager, email: GlobalConstants.DummyAuthUserSpecs.Email);
        }

        static private async Task SeedDummyAuthUserAsync(UserManager<ApplicationUser> userManager, string email)
        {
            var user = await userManager.FindByNameAsync(email);
            if (user != null)
                return;

            var result = await userManager.CreateAsync(new ApplicationUser
                {
                    Email = email,
                    UserName = email, //use the email as the username
                    EmailConfirmed = false,
                    NormalizedEmail = email.ToUpperInvariant(),
                    NormalizedUserName = email.ToUpperInvariant(),
                }
            );
            if (!result.Succeeded)
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }
    }
}