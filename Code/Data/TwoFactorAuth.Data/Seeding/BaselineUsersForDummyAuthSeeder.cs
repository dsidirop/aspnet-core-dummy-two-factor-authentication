﻿namespace TwoFactorAuth.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using TwoFactorAuth.Common.Contracts.Configuration;
    using TwoFactorAuth.Data.Models;

    internal class BaselineUsersForDummyAuthSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dummyAuthSpecs = serviceProvider.GetRequiredService<IOptionsMonitor<AppDummyAuthSpecs>>();

            await SeedDummyAuthUserAsync(
                userManager,
                dummyAuthSpecs.CurrentValue.DummyUsers.First.Email,
                dummyAuthSpecs.CurrentValue.DummyUsers.First.Password
            );

            await SeedDummyAuthUserAsync(
                userManager,
                dummyAuthSpecs.CurrentValue.DummyUsers.Second.Email,
                dummyAuthSpecs.CurrentValue.DummyUsers.Second.Password
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
                new ApplicationUser
                {
                    Email = email,
                    UserName = email, //0
                    EmailConfirmed = false,
                    NormalizedEmail = email.ToUpperInvariant(),
                    NormalizedUserName = email.ToUpperInvariant(),
                },
                password
            );

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }

            //0 we use the email as the username
        }
    }
}
