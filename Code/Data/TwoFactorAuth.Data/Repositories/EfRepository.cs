
namespace TwoFactorAuth.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TwoFactorAuth.Data.Common.Repositories;

    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public EfRepository(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = Context.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet { get; set; }

        protected ApplicationDbContext Context { get; set; }

        protected virtual IQueryable<TEntity> QAll() => DbSet; //                            keep these protected and never make them part of IRepository
        protected virtual IQueryable<TEntity> QAllNoTracking() => QAll().AsNoTracking(); //  keep these protected and never make them part of IRepository

        public virtual int AllCount() => DbSet.Count();

        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate) => QAll().Where(predicate).AsEnumerable();
        public virtual IEnumerable<TEntity> FindByNoTracking(Expression<Func<TEntity, bool>> predicate) => QAllNoTracking().Where(predicate).AsEnumerable();

        public virtual async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate) => await QAll().Where(predicate).ToArrayAsync();
        public virtual async Task<IEnumerable<TEntity>> FindByNoTrackingAsync(Expression<Func<TEntity, bool>> predicate) => await QAllNoTracking().Where(predicate).ToArrayAsync();

        public virtual async Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> predicate) => await QAll().Where(predicate).FirstOrDefaultAsync();
        public virtual async Task<TEntity> FindFirstNoTrackingAsync(Expression<Func<TEntity, bool>> dbQuery) => await QAllNoTracking().Where(dbQuery).FirstOrDefaultAsync();

        public virtual IEnumerable<TEntity> All() => QAll().ToArray();
        public virtual IEnumerable<TEntity> AllAsNoTracking() => QAllNoTracking().ToArray();

        public virtual Task AddAsync(TEntity entity) => DbSet.AddAsync(entity).AsTask();

        public virtual void Update(TEntity entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity) => DbSet.Remove(entity);

        public Task<int> SaveChangesAsync() => Context.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context?.Dispose();
            }
        }
    }
}
