namespace TwoFactorAuth.Data.Common.Repositories
{
    using System.Collections.Generic;
    using TwoFactorAuth.Data.Common.Models;

    public interface IDeletableEntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
        IEnumerable<TEntity> AllWithDeleted();
        IEnumerable<TEntity> AllAsNoTrackingWithDeleted();

        void HardDelete(TEntity entity);
        void Undelete(TEntity entity);
    }
}
