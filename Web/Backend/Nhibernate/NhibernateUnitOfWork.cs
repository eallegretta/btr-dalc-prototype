using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using Web.Backend.Contracts;

namespace Web.Backend.Nhibernate
{
    /// <summary>
    /// Taken from http://nhforge.org/blogs/nhibernate/archive/2011/03/03/effective-nhibernate-session-management-for-web-apps.aspx
    /// </summary>
    public class NhibernateUnitOfWork : IUnitOfWork
    {
        public void Begin()
        {
            foreach (var sessionFactory in GetSessionFactories())
            {
                var localFactory = sessionFactory;

                LazySessionContext.Bind(new Lazy<ISession>(() => BeginSession(localFactory)), sessionFactory);
            }
        }

        public void End()
        {
            foreach (var sessionfactory in GetSessionFactories())
            {
                var session = LazySessionContext.UnBind(sessionfactory);
                if (session == null) continue;
                EndSession(session);
            }
        }

        private static ISession BeginSession(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            session.BeginTransaction();
            return session;
        }

        private static void EndSession(ISession session, bool commitTransaction = true)
        {
            try
            {
                if (session.Transaction != null && session.Transaction.IsActive)
                {
                    if (commitTransaction)
                    {
                        try
                        {
                            session.Transaction.Commit();
                        }
                        catch
                        {
                            session.Transaction.Rollback();
                            throw;
                        }
                    }
                    else
                    {
                        session.Transaction.Rollback();
                    }
                }
            }
            finally
            {
                if (session.IsOpen)
                    session.Close();

                session.Dispose();
            }
        }

        /// <summary>
        /// Retrieves all ISessionFactory instances via IoC
        /// </summary>
        private IEnumerable<ISessionFactory> GetSessionFactories()
        {
            var sessionFactories = ServiceLocator.Current.GetAllInstances<ISessionFactory>();

            if (sessionFactories == null || !sessionFactories.Any())
                throw new TypeLoadException("At least one ISessionFactory has not been registered with IoC");

            return sessionFactories;
        }
    }
}