using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogTalkRadio.Common.Data.FluentMapping
{
    public static class DataSourceMapper
    {
        private static readonly IList<DataSourceMapping> _mappings = new List<DataSourceMapping>();

        public static DataSourceMapping Map(params DataSource[] dataSources)
        {
            var mapping = new DataSourceMapping(dataSources);

            _mappings.Add(mapping);

            return mapping;
        }

        #region GetDataSourcesFor
        public static DataSource[] GetDataSourcesForType<T>()
        {
            return GetDataSourcesForType(typeof(T));
        }

        public static DataSource[] GetDataSourcesForType(Type type)
        {
            return GetMappingsForType(type).SelectMany(x => x.DataSources).ToArray();
        }

        public static DataSource[] GetDataSourcesForQuery<T>(IQuery<T> query) where T : class, new()
        {
            VerifyQueryIsNotNull(query);
            return GetDataSourcesForType(query.GetType());
        }
        #endregion

        #region GetDefaultWritingDataSourcesFor
        public static DataSource GetDefaultWritingDataSourceForQuery<T>(IQuery<T> query) where T: class, new()
        {
            VerifyQueryIsNotNull(query);

            return GetDefaultWritingDataSourceForType(query.GetType());
        }

        public static DataSource GetDefaultWritingDataSourceForType<T>()
        {
            return GetDefaultWritingDataSourceForType(typeof (T));
        }

        public static DataSource GetDefaultWritingDataSourceForType(Type type)
        {
            var mapping = GetMappingsForType(type).FirstOrDefault();

            if (mapping == null)
            {
                return null;
            }

            return mapping.DefaultWritingDataSource ?? mapping.DataSources.First();
        }
        #endregion

        #region GetDefaultReadingDataSourcesFor
        public static DataSource GetDefaultReadingDataSourceForQuery<T>(IQuery<T> query) where T : class, new()
        {
            VerifyQueryIsNotNull(query);

            return GetDefaultReadingDataSourceForType(query.GetType());
        }

        public static DataSource GetDefaultReadingDataSourceForType<T>()
        {
            return GetDefaultWritingDataSourceForType(typeof(T));
        }

        public static DataSource GetDefaultReadingDataSourceForType(Type type)
        {
            var mapping = GetMappingsForType(type).FirstOrDefault();

            if (mapping == null)
            {
                return null;
            }

            return mapping.DefaultReadingDataSource ?? mapping.DataSources.First();
        }
        #endregion

        private static void VerifyQueryIsNotNull<T>(IQuery<T> query) where T : class, new()
        {
            if (query == null)
            {
                throw new Exception("The query cannot be null");
            }
        }

        private static IList<DataSourceMapping> GetMappingsForType(Type type)
        {
            bool containsMappings = DataSourceMapping.AllMappings.ContainsKey(type);

            if (containsMappings == false)
            {
                return new List<DataSourceMapping>();
            }

            return DataSourceMapping.AllMappings[type];
        }
    }
}