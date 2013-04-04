using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BlogTalkRadio.Common.Data.FluentMapping
{
    public class DataSourceMapping : IEquatable<DataSourceMapping>
    {
        private readonly Guid _mappingId = Guid.NewGuid();

        private readonly IList<Type> _mappedTypes = new List<Type>();
        internal static IDictionary<Type, IList<DataSourceMapping>> AllMappings = new Dictionary<Type, IList<DataSourceMapping>>();


        internal DataSource DefaultReadingDataSource { get; private set; }
        internal DataSource DefaultWritingDataSource { get; private set; }

        internal DataSourceMapping(params DataSource[] dataSources)
        {
            DataSources = dataSources;
        }

        internal DataSource[] DataSources { get; private set; }

        public DataSourceMapping Default(DataSource dataSource)
        {
            VerifyDataSourceWasAdded(dataSource);
            DefaultReadingDataSource = DefaultWritingDataSource = dataSource;
            return this;
        }

        public DataSourceMapping DefaultForReading(DataSource dataSource)
        {
            VerifyDataSourceWasAdded(dataSource);
            DefaultReadingDataSource = dataSource;
            return this;
        }

        public DataSourceMapping DefaultForWriting(DataSource dataSource)
        {
            VerifyDataSourceWasAdded(dataSource);
            DefaultWritingDataSource = dataSource;
            return this;
        }

        private void VerifyDataSourceWasAdded(DataSource dataSource)
        {
            if (!DataSources.Contains(dataSource))
            {
                throw new Exception("The dataSource specified does not belong to this mapping");
            }
        }

        public DataSourceMapping ToType<T>()
        {
            ToType(typeof(T));
            return this;
        }

        public DataSourceMapping ToTypes(params Type[] types)
        {
            foreach (var type in types)
            {
                ToType(type);
            }

            return this;
        }

        public DataSourceMapping ToType(Type type)
        {
            if (type == null)
            {
                throw new Exception("The type to be mapped cannot be null");
            }

            _mappedTypes.Add(type);


            if (!AllMappings.ContainsKey(type))
            {
                AllMappings.Add(type, new List<DataSourceMapping>());
            }

            var mappings = AllMappings[type];

            if (!mappings.Contains(this))
            {
                mappings.Add(this);
            }

            return this;
        }


        public DataSourceMapping ToQueries(params Type[] queryTypes)
        {
            foreach (var type in queryTypes)
            {
                ToQuery(type);
            }

            return this;
        }

        public DataSourceMapping ToQuery(Type queryType)
        {
            if (queryType == null)
            {
                throw new Exception("The queryType to be mapped cannot be null");
            }

            ToType(queryType);

            return this;
        }

        public DataSourceMapping ToNamespace<T>()
        {
            ToNamespace(typeof(T));
            return this;
        }

        public DataSourceMapping ToNamespace(Type type)
        {
            if (type == null)
            {
                throw new Exception("The type to be mapped is required");
            }

            ToNamespace(type.Namespace, type.Assembly);
            return this;
        }

        public DataSourceMapping ToNamespace(string @namespace, Assembly assembly = null)
        {
            if (string.IsNullOrWhiteSpace(@namespace))
            {
                throw new Exception("The namespace to be mapped is required");
            }

            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }

            var types = from type in assembly.GetTypes()
                        where type.Namespace == @namespace
                        select type;

            foreach (var type in types)
            {
                ToType(type);
            }

            return this;
        }

        public DataSourceMapping Exclude<T>()
        {
            return Exclude(typeof (T));
        }

        public DataSourceMapping ExcludeQuery(Type queryType)
        {
            return Exclude(queryType);
        }

        public DataSourceMapping Exclude(Type type)
        {
            if (_mappedTypes.Contains(type))
            {
                _mappedTypes.Remove(type);
            }

            if (AllMappings.ContainsKey(type) && AllMappings[type].Contains(this))
            {
                AllMappings[type].Remove(this);
            }

            return this;
        }

        public bool IsMappedToType<T>()
        {
            return IsMappedToType(typeof(T));
        }

        public bool IsMappedToType(Type type)
        {
            return _mappedTypes.Contains(type);
        }

        public bool IsMappedToQuery(Type queryType)
        {
            return IsMappedToType(queryType);
        }

        public bool Equals(DataSourceMapping other)
        {
            return Equals((object)other);
        }

        public override bool Equals(object obj)
        {
            var other = obj as DataSourceMapping;

            if (other == null)
            {
                return false;
            }

            return GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _mappingId.GetHashCode();
        }
    }
}