using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Web.Backend.DomainModel.Contracts;
using IQuery = NHibernate.IQuery;

namespace Web.Backend.Data.Orm.Nhibernate
{
    public class NhibernateRepository<T> : BaseRepository<T> where T : class, new()
    {
        private readonly ISessionFactory _sessionFactory;

        public NhibernateRepository(ISessionFactory sessionFactory, IEnumerable<IQueryHandler<T>> queryHandlers): base(queryHandlers)
        {
            _sessionFactory = sessionFactory;
        }

        protected ISession Session
        {
            get { return _sessionFactory.GetCurrentSession(); }
        }

        public override T Get(object id)
        {
            return Session.Get<T>(id);
        }

        public override T SaveOrUpdate(T instance)
        {
            Session.SaveOrUpdate(instance);
            return instance;
        }

        public override void Delete(object id)
        {
            Session.Delete(id);
            Session.Flush();
        }

        public override void DeleteAll()
        {
            Session.Delete("from " + typeof(T).Name + " e");
            Session.Flush();
        }
    }
}
