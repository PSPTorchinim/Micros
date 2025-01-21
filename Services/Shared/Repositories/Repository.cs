using Shared.Data.Exceptions;
using Shared.Data.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Shared.Repositories
{
    public class Repository<T, C> : IRepository<T> where T : class where C : DbContext
    {
        public readonly C _context;
        private readonly ILogger<IRepository<T>> _logger;

        public Repository(C context, ILogger<IRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Create
        public virtual async Task<bool> Add(T entity)
        {
            return await ExceptionHandler.Handle(async () => {
                await _context.AddAsync(entity);
                return await _context.SaveChangesAsync() > 0;
            }, _logger);
        }

        public virtual async Task<bool> AddRange(List<T> entities)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                await _context.AddRangeAsync(entities);
                return await _context.SaveChangesAsync() > 0;
            }, _logger);
        }
        #endregion

        #region Count
        public async Task<int> Count(Expression<Func<T, bool>> expression)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                return (await Get(expression)).Count();
            }, _logger);
        }

        public async Task<int> Count(ISpecification<T> specification)
        {
            return await ExceptionHandler.Handle(async () => {
                return (await Get(specification)).Count();
            }, _logger);
        }
        #endregion

        #region Delete
        public virtual async Task<bool> Delete(T entity)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                _context.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }, _logger);
        }

        public virtual async Task<bool> DeleteRange(List<T> entities)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                _context.RemoveRange(entities);
                return await _context.SaveChangesAsync() > 0;
            }, _logger);
        }
        #endregion

        #region Empty
        public async Task<bool> Empty()
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var any = (await Get()).Count() > 0;
                return !any;
            }, _logger);
        }
        #endregion

        #region Exists
        public async Task<bool> Exists(Expression<Func<T, bool>> expression)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var any = (await Get(expression)).Count() > 0;
                return any;
            }, _logger);
        }

        public async Task<bool> Exists(ISpecification<T> specification)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var any = (await Get(specification)).Count() > 0;
                return any;
            }, _logger);
        }
        #endregion

        #region Read
        public virtual async Task<List<T>> Get()
        {
            return await ExceptionHandler.Handle(async () =>
            {
                return await _context.Set<T>().ToListAsync();
            }, _logger);
        }

        public virtual async Task<List<T>> Get(Expression<Func<T, bool>> expression)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                return await _context.Set<T>().Where(expression).ToListAsync();
            }, _logger);
        }

        public virtual async Task<List<T>> Get(ISpecification<T> specification)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                return await SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification).ToListAsync();
            }, _logger);
        }
        #endregion

        #region Update
        public virtual async Task<bool> Update(T entity)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                _context.Update(entity);
                return await _context.SaveChangesAsync() > 0;
            }, _logger);
        }

        public virtual async Task<bool> UpdateRange(List<T> entities)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                _context.UpdateRange(entities);
                return await _context.SaveChangesAsync() > 0;
            }, _logger);
        }
        #endregion

    }
}
