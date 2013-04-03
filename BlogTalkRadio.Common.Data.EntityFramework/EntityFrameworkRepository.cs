using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using BlogTalkRadio.Common.Data.EntityFramework;
using BlogTalkRadio.Common.Data.FluentMapping;
using Cinchcast.Framework.Reflection;

namespace BlogTalkRadio.Common.Data.Orm.EntityFramework
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

        private readonly IDbContextFactorySelector _dbContextFactorySelector;

        public EntityFrameworkRepository(IDbContextFactorySelector dbContextFactorySelector, IEnumerable<IQueryHandler<T>> queryHandlers)
            : base(queryHandlers)
        {
            _dbContextFactorySelector = dbContextFactorySelector;
        }

        protected PrimaryKeyDescriptor GetPrimaryKey()
        {
            return _primaryKeys.GetOrAdd(typeof(T), t =>
                {
                    var set = ((IObjectContextAdapter)DbContext()).ObjectContext.CreateObjectSet<T>();
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

        private DbContext DbContext(bool forReading = true)
        {
            var dataSource = forReading
                             ? DataSourceMapper.GetDefaultReadingDataSourceForType<T>()
                             : DataSourceMapper.GetDefaultWritingDataSourceForType<T>();

            if (dataSource == null)
            {
                dataSource = DataSourceMapper.GetDataSourcesForType<T>().First();
            }

            return _dbContextFactorySelector.GetDbContextFactoryFor(dataSource).GetCurrentDbContext();
        }

        public override T Get(object id)
        {
            return DbContext().Set<T>().Find(id);
        }

        public override T SaveOrUpdate(T instance)
        {
            if (instance == null)
                return null;

            var dbContext = DbContext(false);

            var primaryKey = GetPrimaryKey();
            object primaryKeyValue = primaryKey.Value(instance);
            if (primaryKeyValue == null || primaryKeyValue == Activator.CreateInstance(primaryKey.Type))
            {
                dbContext.Set<T>().Add(instance);
            }
            else
            {
                dbContext.Set<T>().Attach(instance);
                dbContext.Entry(instance).State = EntityState.Modified;
            }

            return instance;
        }

        public override void Delete(object id)
        {
            DbContext(false).Set<T>().Remove(Get(id));
        }

        public override void DeleteAll()
        {
            throw new NotSupportedException();
        }
    }
}
