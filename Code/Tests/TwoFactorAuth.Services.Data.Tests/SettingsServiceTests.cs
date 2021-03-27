namespace TwoFactorAuth.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using TwoFactorAuth.Data;
    using TwoFactorAuth.Data.Common.Repositories;
    using TwoFactorAuth.Data.Models;
    using TwoFactorAuth.Data.Repositories;
    using TwoFactorAuth.Services.Data.Settings;
    using Xunit;

    public class SettingsServiceTests
    {
        [Fact]
        public void GetCountShouldReturnCorrectNumber()
        {
            var repository = new Mock<IDeletableEntityRepository<Setting>>();

            repository
                .Setup(r => r.All())
                .Returns(
                    new List<Setting>
                    {
                        new(),
                        new(),
                        new(),
                    }.AsQueryable()
                );

            var service = new SettingsService(repository.Object);

            Assert.Equal(3, service.GetCount());

            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task GetCountShouldReturnCorrectNumberUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SettingsTestDb")
                .Options;

            await using var dbContext = new ApplicationDbContext(options);
            await dbContext.Settings.AddAsync(new Setting());
            await dbContext.Settings.AddAsync(new Setting());
            await dbContext.Settings.AddAsync(new Setting());

            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Setting>(dbContext);
            var service = new SettingsService(repository);

            Assert.Equal(3, service.GetCount());
        }
    }
}
