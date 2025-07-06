using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

public interface IRepository<TEntity> where TEntity : class
{
    // Sync methods (keep for backward compatibility)
    void Delete(TEntity entityToDelete);
    void Delete(object id);
    IEnumerable<TEntity> GetBy(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "");
    TEntity GetByID(object id);
    void Insert(TEntity entity);
    void Update(TEntity entityToUpdate);
    
    // Async methods for better performance
    Task<TEntity> GetByIDAsync(object id);
    Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null);
    Task<IEnumerable<TEntity>> GetByAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "");
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);
    
    // Bulk operations
    Task InsertAsync(TEntity entity);
    Task InsertRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entityToUpdate);
    Task DeleteAsync(object id);
    Task DeleteAsync(TEntity entityToDelete);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    
    // Unit of work pattern
    Task<int> SaveChangesAsync();
    int SaveChanges();
}