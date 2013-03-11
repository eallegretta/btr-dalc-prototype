using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using Cinchcast.Framework.Reflection;
using Web.Backend.Contracts;
using Web.Backend.DomainModel;
using Web.Backend.DomainModel.Queries;

namespace Web.Backend.EntityFramework
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class, new()
    {
        protected class PrimaryKeyDescriptor
        {
            public string Name { get; set; }
            public Type Type { get; set; }
            public Getter Value { get; set; }
            public Expression<Func<T, object>> Id { get; set; } 
        }

        private static readonly ConcurrentDictionary<Type, PrimaryKeyDescriptor> _primaryKeys = new ConcurrentDictionary<Type, PrimaryKeyDescriptor>();

        private readonly IDbContextFactory _dbContextFactory;

        public EntityFrameworkRepository(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        protected PrimaryKeyDescriptor GetPrimaryKey()
        {
            return _primaryKeys.GetOrAdd(typeof(T), t =>
                {
                    var set = ((IObjectContextAdapter)DbContext).ObjectContext.CreateObjectSet<T>();
                    var entitySet = set.EntitySet;

                    return entitySet.ElementType.KeyMembers.Select(k =>
                        {
                            var property = typeof(T).GetProperty(k.Name);

                            return new PrimaryKeyDescriptor
                                {
                                    Name = k.Name,
                                    Type = property.PropertyType,
                                    Value = DynamicMethodFactory.CreateGetter(property),
                                    Id = PropertyExpression.For<T>(k.Name)
                                };
                        }).FirstOrDefault();
                });
        }

        protected DbContext DbContext
        {
            get { return _dbContextFactory.GetCurrentDbContext(); }   
        }

        public T Get(object id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public T Get(string field, object value)
        {
            return DbContext.Set<T>().Where(Id.For<T>(field, value).MatchingCriteria).FirstOrDefault();
        }

        public T Get(Expression<Func<T, object>> property, object value)
        {
            return DbContext.Set<T>().Where(Id.For(property, value).MatchingCriteria).FirstOrDefault();
        }

        public T Get(IQuery<T> query)
        {
            return DbContext.Set<T>().Where(query.MatchingCriteria).FirstOrDefault();
        }

        public int Count(IQuery<T> query = null)
        {
            if (query != null && query.MatchingCriteria != null)
                return DbContext.Set<T>().Where(query.MatchingCriteria).Count();

            return DbContext.Set<T>().Count();
        }

        public List<T> GetAll(int skip = 0, int take = 1000)
        {
            if (skip > 0)
            {
                return DbContext.Set<T>().OrderBy(GetPrimaryKey().Id).Skip(skip).Take(take).ToList();
            }

            return DbContext.Set<T>().Take(take).ToList();
        }

        public IQueryable<T> Query()
        {
            return DbContext.Set<T>();
        }

        public T SaveOrUpdate(T instance)
        {
            if (instance == null)
                return null;

            var primaryKey = GetPrimaryKey();
            object primaryKeyValue = primaryKey.Value(instance);
            if (primaryKeyValue == null || primaryKeyValue == Activator.CreateInstance(primaryKey.Type))
            {
                DbContext.Set<T>().Add(instance);
            }

            return instance;
        }

        public void Delete(object id)
        {
            DbContext.Set<T>().Remove(Get(id));
        }

        public void DeleteAll()
        {
            throw new NotSupportedException();
        }


        public List<T> NativeQuery(string query, params object[] inputParameters)
        {
            var dbQuery = DbContext.Set<T>().SqlQuery(query, inputParameters);

            return dbQuery.ToList();
        }
    }
}
