using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Web.Backend.Contracts;
using Web.Backend.DomainModel.Queries;

namespace Web.Backend.Nhibernate
{
    public class NhibernateRepository<T> : IRepository<T> where T : class, new()
    {
        private readonly ISessionFactory _sessionFactory;

        public NhibernateRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        protected ISession Session
        {
            get { return _sessionFactory.GetCurrentSession(); }
        }

        public T Get(object id)
        {
            return Session.Get<T>(id);
        }

        public T Get(string field, object value)
        {
            return Session.Query<T>().Where(Id.For<T>(field, value).MatchingCriteria).FirstOrDefault();
        }

        public T Get(System.Linq.Expressions.Expression<System.Func<T, object>> property, object value)
        {
            return Session.Query<T>().Where(Id.For(property, value).MatchingCriteria).FirstOrDefault();
        }

        public T Get(IQuery<T> query)
        {
            return Session.Query<T>().Where(query.MatchingCriteria).FirstOrDefault();
        }

        public int Count(IQuery<T> query = null)
        {
            if (query != null && query.MatchingCriteria != null)
                return Session.Query<T>().Where(query.MatchingCriteria).Count();

            return Session.Query<T>().Count();
        }

        public List<T> GetAll(int skip = 0, int take = 1000)
        {
            return Session.Query<T>().Skip(skip).Take(take).ToList();
        }

        public IQueryable<T> Query()
        {
            return Session.Query<T>();
        }

        public T SaveOrUpdate(T instance)
        {
            Session.SaveOrUpdate(instance);
            return instance;
        }

        public void Delete(object id)
        {
            Session.Delete(id);
            Session.Flush();
        }

        public void DeleteAll()
        {
            Session.Delete("from " + typeof (T).Name + " e");
            Session.Flush();
        }
    }
}
