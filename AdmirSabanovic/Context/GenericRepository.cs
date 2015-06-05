using AdmirSabanovic.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmirSabanovic.Context
{
    public abstract class GenericRepository<C, T>
        where T : class
        where C : DBContext, new()
    {

        private C _entities = new C();
        public C Context
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = _entities.Set<T>();
            return query;
        }

        public virtual IQueryable<T> getAllWithInclude(String include)
        {
            IQueryable<T> query = _entities.Set<T>().Include(include);
            return query;
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _entities.Set<T>().AsNoTracking().Where(predicate);
            return query;
        }

        public virtual void Add(T entity)
        {
            _entities.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _entities.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = System.Data.EntityState.Modified;
        }

        public virtual List<T> GetAllInList()
        {
            var query = GetAll().ToList();
            return query;
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }
    }
}