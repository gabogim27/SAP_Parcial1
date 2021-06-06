namespace Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using SAP.Crypto.Repository.Interfaces;

    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext Context { get; }

        public Repository(DbContext context)
        {
            Context = context;
        }

        protected DbSet<T> Set
        {
            get
            {
                return Context.Set<T>();
            }
        }

        public void Delete(T entity)
        {
            Set.Remove(entity);
            Context.SaveChanges();
        }

        public void Delete(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                Set.Remove(entity);
                Context.SaveChanges();
            }
        }

        public T Read(params object[] keyValues)
        {
            return Set.Find(keyValues);
        }

        public void Save(T entity)
        {
            Set.Add(entity);
            Context.SaveChanges();
        }

        public void Save(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                Save(entity);
            }
        }

        public void Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }

        public void Update(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                Context.Update(entity);
                Context.SaveChanges();
            }
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return Set.Where(predicate);
        }

        public IEnumerable<T> List()
        {
            return Set;
        }
    }
}
