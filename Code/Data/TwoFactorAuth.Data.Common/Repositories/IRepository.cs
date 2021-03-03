namespace TwoFactorAuth.Data.Common.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        int AllCount();

        IEnumerable<TEntity> All();
        IEnumerable<TEntity> AllAsNoTracking();

        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> FindByNoTracking(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindByNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindFirstNoTrackingAsync(Expression<Func<TEntity, bool>> dbQuery);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}
