using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BlogTalkRadio.Common.Data.Dapper;
using BlogTalkRadio.Common.Data.Queries;
using Cinchcast.Framework.Reflection;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;

namespace BlogTalkRadio.Common.Data.NHibernate.QueryHandlers
{
    public class DapperStoredProcedureQueryHandler<T> : IQueryHandler<T> where T : class, new()
    {
        private readonly IDapperConnection _dapperConnection;
        private static readonly ConcurrentDictionary<Type, IDictionary<string, Setter>> _entityPropertyGetters = new ConcurrentDictionary<Type, IDictionary<string, Setter>>(); 

        public DapperStoredProcedureQueryHandler(IDapperConnection dapperConnection)
        {
            _dapperConnection = dapperConnection;
        }

        public bool CanHandle(IQuery<T> query)
        {
            return query is StoredProcedureQuery<T>;
        }

        public int Count(IQuery<T> query = null)
        {
            var spQuery = query as StoredProcedureQuery<T>;

            using (var cn = _dapperConnection.For(query))
            {
                return (int)cn.ExecuteScalar(spQuery.StoredProcedure, spQuery.Parameters);
            }
        }

        public T Get(IQuery<T> query)
        {
            return Query(query).FirstOrDefault();
        }

        public List<T> Query(IQuery<T> query)
        {
            var spQuery = query as StoredProcedureQuery<T>;

            var classMap = DapperExtensions.DapperExtensions.GetMap<T>();

            var propertySetters = EnsurePropertySetters(classMap);

            var returnEntities = new List<T>();


            using (var cn = _dapperConnection.For(query))
            {
                var entities = cn.Query(spQuery.StoredProcedure, spQuery.Parameters,
                                        commandType: CommandType.StoredProcedure);

                foreach (var entity in entities)
                {
                    var entityData = new Dictionary<string, object>(entity as IDictionary<string, object>, StringComparer.InvariantCultureIgnoreCase);

                    var returnEntity = new T();

                    foreach (var propertyNameAndSetter in propertySetters)
                    {
                        propertyNameAndSetter.Value(returnEntity, entityData[propertyNameAndSetter.Key]);
                    }

                    returnEntities.Add(returnEntity);
                }

                return returnEntities;
            }
        }

        private IDictionary<string, Setter> EnsurePropertySetters(IClassMapper classMap)
        {
            return _entityPropertyGetters.GetOrAdd(classMap.EntityType, type =>
                {
                    var setters = new Dictionary<string, Setter>(StringComparer.InvariantCultureIgnoreCase);

                    foreach (var property in classMap.Properties.Where(p => !p.Ignored))
                    {
                        var setter = DynamicMethodFactory.CreateSetter(property.PropertyInfo);

                        setters.Add(property.ColumnName, setter);
                    }

                    return setters;
                });
        }
    }
}