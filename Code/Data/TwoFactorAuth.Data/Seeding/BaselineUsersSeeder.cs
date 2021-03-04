namespace TwoFactorAuth.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using TwoFactorAuth.Common;
    using TwoFactorAuth.Data.Models;

    internal class BaselineUsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedDummyAuthUserAsync(
                userManager: userManager,
                email: GlobalConstants.DummyAuthSpecs.Email,
                password: GlobalConstants.DummyAuthSpecs.Passwords.Second
            );
        }

        static private async Task SeedDummyAuthUserAsync(UserManager<ApplicationUser> userManager, string email, string password)
        {
            var user = await userManager.FindByNameAsync(email);
            if (user != null)
            {
                await userManager.DeleteAsync(user); //vital
            }

            var result = await userManager.CreateAsync(
                user: new ApplicationUser
                {
                    Email = email,
                    UserName = email, //0
                    EmailConfirmed = false,
                    NormalizedEmail = email.ToUpperInvariant(),
                    NormalizedUserName = email.ToUpperInvariant(),
                },
                password: password
            );

            if (!result.Succeeded)
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));

            //0 we use the email as the username
        }
    }
}