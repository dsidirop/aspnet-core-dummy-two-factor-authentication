namespace TwoFactorAuth.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using TwoFactorAuth.Data.Common.Models;
    using TwoFactorAuth.Data.Common.Repositories;

    public class EfDeletableEntityRepository<TEntity> : EfRepository<TEntity>, IDeletableEntityRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
        public EfDeletableEntityRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public override IEnumerable<TEntity> All() => base.QAll().Where(x => !x.IsDeleted);
        public override IEnumerable<TEntity> AllAsNoTracking() => base.QAllNoTracking().Where(x => !x.IsDeleted);

        public IQueryable<TEntity> AllWithDeleted() => base.QAll().IgnoreQueryFilters();
        public IQueryable<TEntity> AllAsNoTrackingWithDeleted() => base.QAllNoTracking().IgnoreQueryFilters();

        public void HardDelete(TEntity entity) => base.Delete(entity);

        public void Undelete(TEntity entity)
        {
            entity.IsDeleted = false;
            entity.DeletedOn = null;
            Update(entity);
        }

        public override void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            Update(entity);
        }
    }
}
