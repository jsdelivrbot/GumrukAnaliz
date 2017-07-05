using Gumruk.DayaLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Gumruk.Repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>, IDisposable
        where TEntity : class, new()
    {
        private readonly GumrukDBContext _context;

        public RepositoryBase(bool ProxyCreationEnabled = true)
        {
            _context = new GumrukDBContext();
            _context.Configuration.LazyLoadingEnabled = true;
            _context.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;
        }

        public IQueryable<T> All<T>() where T : class
        {
            return _context.Set<T>();
        }

        public IQueryable<T> AllIncluding<T>(params Expression<Func<T, object>>[] include) where T : class
        {
            IQueryable<T> returnvalue = _context.Set<T>();

            foreach (var item in include)
            {
                returnvalue = returnvalue.Include(item);
            }

            return returnvalue;
        }

        //public int GetCount(Expression<Func<TEntity, int>> filter = null)
        //{
        //    _context.Set<TEntity>().Count<TEntity>(p => p. )

        //    return 0;
        //}

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter, bool tracking = true)
        {
            if (tracking)
                return _context.Set<TEntity>().FirstOrDefault(filter);
            else
                return _context.Set<TEntity>().AsNoTracking().FirstOrDefault(filter);
        }

        public TEntity Add(TEntity entity)
        {
            var status = _context.Entry(entity);
            status.State = EntityState.Added;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return entity;
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
                return _context.Set<TEntity>().Where(filter).ToList();
            else
                return _context.Set<TEntity>().ToList();
        }

        public bool Remove(TEntity entity)
        {
            var status = _context.Entry(entity);
            status.State = EntityState.Deleted;

            return _context.SaveChanges() > 0;
        }

        public TEntity Update(TEntity entity)
        {
            var status = _context.Entry(entity);
            status.State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return entity;
        }
    }
}
