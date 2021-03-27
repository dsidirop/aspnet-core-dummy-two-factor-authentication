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

        public override IEnumerable<TEntity> All()
        {
            return base.QAll().Where(x => !x.IsDeleted);
        }

        public override IEnumerable<TEntity> AllAsNoTracking()
        {
            return base.QAllNoTracking().Where(x => !x.IsDeleted);
        }

        public IEnumerable<TEntity> AllWithDeleted()
        {
            return base.QAll().IgnoreQueryFilters().ToArray();
        }

        public IEnumerable<TEntity> AllAsNoTrackingWithDeleted()
        {
            return base.QAllNoTracking().IgnoreQueryFilters().ToArray();
        }

        public void HardDelete(TEntity entity)
        {
            base.Delete(entity);
        }

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
