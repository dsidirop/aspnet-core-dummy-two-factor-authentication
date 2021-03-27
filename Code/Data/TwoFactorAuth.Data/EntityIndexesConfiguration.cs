namespace TwoFactorAuth.Data
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using TwoFactorAuth.Data.Common.Models;

    static internal class EntityIndexesConfiguration
    {
        static public void Configure(ModelBuilder modelBuilder)
        {
            // IDeletableEntity.IsDeleted index
            var deletableEntityTypes = modelBuilder.Model
                .GetEntityTypes()
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                modelBuilder.Entity(deletableEntityType.ClrType).HasIndex(nameof(IDeletableEntity.IsDeleted));
            }
        }
    }
}
