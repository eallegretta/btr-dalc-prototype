using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Cinchcast.Framework.Reflection;
using Web.Backend.Data.Queries;
using Web.Backend.DomainModel;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.Data.Orm.EntityFramework
{
    public class EntityFrameworkRepository<T> : BaseRepository<T> where T : class, new()
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

        public EntityFrameworkRepository(IDbContextFactory dbContextFactory, IEnumerable<IQueryInterpreter<T>> queryInterpreters)
            : base(queryInterpreters)
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

        public override T Get(object id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public override T SaveOrUpdate(T instance)
        {
            if (instance == null)
                return null;

            var primaryKey = GetPrimaryKey();
            object primaryKeyValue = primaryKey.Value(instance);
            if (primaryKeyValue == null || primaryKeyValue == Activator.CreateInstance(primaryKey.Type))
            {
                DbContext.Set<T>().Add(instance);
            }
            else
            {
                DbContext.Set<T>().Attach(instance);
                DbContext.Entry(instance).State = EntityState.Modified;
            }

            return instance;
        }

        public override void Delete(object id)
        {
            DbContext.Set<T>().Remove(Get(id));
        }

        public override void DeleteAll()
        {
            throw new NotSupportedException();
        }
    }
}
