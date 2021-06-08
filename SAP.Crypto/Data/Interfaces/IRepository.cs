namespace SAP.Crypto.Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IRepository<T> where T : class
    {
        void Save(T entity);

        void Save(IList<T> entities);

        T Read(params object[] keyValues);

        IEnumerable<T> List();

        bool Update(T entity);

        void Update(IList<T> entities);

        void Delete(T entity);

        void Delete(IList<T> entities);

        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);

        T FirstOrDefault(Expression<Func<T, bool>> predicate);
    }
}
