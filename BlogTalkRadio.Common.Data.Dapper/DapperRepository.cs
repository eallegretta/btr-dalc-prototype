using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cinchcast.Framework.Reflection;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;

namespace BlogTalkRadio.Common.Data.Dapper
{
    public class DapperRepository<T> : BaseRepository<T> where T : class, new()
    {
        private readonly IDapperConnection _dapperConnection;

        private static ConcurrentDictionary<Type, Getter> _idGetters = new ConcurrentDictionary<Type, Getter>();

        public DapperRepository(IDapperConnection dapperConnection, IEnumerable<IQueryHandler<T>> queryHandlers)
            : base(queryHandlers)
        {
            _dapperConnection = dapperConnection;
        }

        public override T Get(object id)
        {
            using (var cn = _dapperConnection.For<T>())
            {
                return cn.Get<T>(id);
            }
        }

        public override T SaveOrUpdate(T instance)
        {
            if (instance == null)
            {
                throw new Exception("The instance to be saved cannot be null");
            }

            using (var cn = _dapperConnection.For<T>())
            {
                var idGetter = _idGetters.GetOrAdd(typeof(T), GetTypeGetter());

                var id = idGetter(instance);

                if (id == null || id == Activator.CreateInstance(id.GetType()))
                {
                    cn.Insert(instance);
                }
                else
                {
                    cn.Update(instance);
                }

                return instance;
            }
        }

        public override void Delete(object id)
        {
            using (var cn = _dapperConnection.For<T>())
            {
                cn.Delete(Get(id));
            }
        }

        public override void DeleteAll()
        {
            throw new NotSupportedException();
        }

        private static Getter GetTypeGetter()
        {
            var idProperty = GetIdProperty();

            var getter = DynamicMethodFactory.CreateGetter(idProperty.PropertyInfo);

            return getter;
        }

        private static IPropertyMap GetIdProperty()
        {
            var type = typeof (T);

            var classMap = DapperExtensions.DapperExtensions.GetMap<T>();

            var idProperty = classMap.Properties.Where(x => x.KeyType != KeyType.NotAKey && !x.Ignored).FirstOrDefault();

            if (idProperty == null)
            {
                throw new Exception(string.Format("The type {0} does not have defined an id property to be used with the Dapper Repository", type));
            }

            return idProperty;
        }
    }
}
