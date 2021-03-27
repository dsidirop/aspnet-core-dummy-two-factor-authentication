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

        public virtual int AllCount()
        {
            return DbSet.Count();
        }

        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return QAll().Where(predicate).AsEnumerable();
        }

        public virtual IEnumerable<TEntity> FindByNoTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return QAllNoTracking().Where(predicate).AsEnumerable();
        }

        public virtual async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await QAll().Where(predicate).ToArrayAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindByNoTrackingAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await QAllNoTracking().Where(predicate).ToArrayAsync();
        }

        public virtual async Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await QAll().Where(predicate).FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> FindFirstNoTrackingAsync(Expression<Func<TEntity, bool>> dbQuery)
        {
            return await QAllNoTracking().Where(dbQuery).FirstOrDefaultAsync();
        }

        public virtual IEnumerable<TEntity> All()
        {
            return QAll().ToArray();
        }

        public virtual IEnumerable<TEntity> AllAsNoTracking()
        {
            return QAllNoTracking().ToArray();
        }

        public virtual Task AddAsync(TEntity entity)
        {
            return DbSet.AddAsync(entity).AsTask();
        }

        public virtual void Update(TEntity entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual IQueryable<TEntity> QAll()
        {
            return DbSet; //                            keep these protected and never make them part of IRepository
        }

        protected virtual IQueryable<TEntity> QAllNoTracking()
        {
            return QAll().AsNoTracking(); //  keep these protected and never make them part of IRepository
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
