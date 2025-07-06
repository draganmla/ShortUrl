using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShortUrl.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly UriDbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public BaseRepository(UriDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.dbSet = context.Set<TEntity>();
        }

        #region Sync Methods (Legacy Support)
        
        public virtual IEnumerable<TEntity> GetBy(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            if (entityToDelete != null)
            {
                Delete(entityToDelete);
            }
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (entityToDelete == null) throw new ArgumentNullException(nameof(entityToDelete));
            
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            context.SaveChanges();
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            if (entityToUpdate == null) throw new ArgumentNullException(nameof(entityToUpdate));
            
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
            context.SaveChanges();
        }

        #endregion

        #region Async Methods (Recommended)

        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetByAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            return await dbSet.AnyAsync(filter);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter == null)
            {
                return await dbSet.CountAsync();
            }
            return await dbSet.CountAsync(filter);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await dbSet.AddAsync(entity);
            // Note: SaveChanges is not called here to allow for unit of work pattern
        }

        public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            await dbSet.AddRangeAsync(entities);
            // Note: SaveChanges is not called here to allow for unit of work pattern
        }

        public virtual Task UpdateAsync(TEntity entityToUpdate)
        {
            if (entityToUpdate == null) throw new ArgumentNullException(nameof(entityToUpdate));
            
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
            // Note: SaveChanges is not called here to allow for unit of work pattern
            return Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = await dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                await DeleteAsync(entityToDelete);
            }
        }

        public virtual Task DeleteAsync(TEntity entityToDelete)
        {
            if (entityToDelete == null) throw new ArgumentNullException(nameof(entityToDelete));
            
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            // Note: SaveChanges is not called here to allow for unit of work pattern
            return Task.CompletedTask;
        }

        public virtual Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            dbSet.RemoveRange(entities);
            // Note: SaveChanges is not called here to allow for unit of work pattern
            return Task.CompletedTask;
        }

        #endregion

        #region Unit of Work Pattern

        public virtual async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public virtual int SaveChanges()
        {
            return context.SaveChanges();
        }

        #endregion
    }
}
