using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gumruk.Repository
{
    public interface IRepositoryBase<TEntity> where TEntity : class , new()
    {
        IQueryable<T> All<T>() where T : class;
        IQueryable<T> AllIncluding<T>(params Expression<Func<T, object>>[] include) where T : class;

        TEntity Get(Expression<Func<TEntity, bool>> filter,bool tracking=true);

        List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null);

        TEntity Add(TEntity entity);

        bool Remove(TEntity entity);

        TEntity Update(TEntity entity);
    }
}
