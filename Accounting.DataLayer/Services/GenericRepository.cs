using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer.Services
{
    public class GenericRepository<T> where T : class
    {
        private Accounting_DBEntities2 _db;
        private DbSet<T> _dbSet;

        public GenericRepository(Accounting_DBEntities2 db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public void insert (T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual T GetByID (object Id)
        {
            return _dbSet.Find(Id);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            if (_db.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }
        public virtual void Delete(object Id)
        {
            var entity = GetByID(Id);
            Delete(entity);
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> where = null)
        {
            IQueryable<T> query = _dbSet;
            if(where != null)
            {
                query = query.Where(where);
            }
            return query.ToList();
        }
    }
}
