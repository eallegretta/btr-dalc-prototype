using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BlogTalkRadio.Common.Data.Orm.EntityFramework.Mappings;
using Cinchcast.Framework.DependencyInjection.Autofac;
using Microsoft.Practices.ServiceLocation;

namespace BlogTalkRadio.Common.Data.Orm.EntityFramework
{
    public class BtrDbContext: DbContext
    {
        private readonly string _connectionStringName;
        private static readonly Type[] _mappingTypes = GetMappingTypes();

        private static Type[] GetMappingTypes()
        {
            var query = from assembly in Ioc.Instance.GetAssemblies()
                        from type in assembly.GetTypes()
                        where typeof(IEntityMapping).IsAssignableFrom(type)
                        select type;

            return query.ToArray();
        }

        private static Func<string, Type, bool> _mappingTypesForDatabaseEvaluator;

        public BtrDbContext(string connectionStringName) : base(connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        public static void SetMappingTypesForDatabaseEvaluator(Func<string, Type, bool> evaluator)
        {
            _mappingTypesForDatabaseEvaluator = evaluator;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new BtrDatabaseInitializer());

            Configuration.LazyLoadingEnabled = false;

            foreach (var mapping in GetMappingTypesForDatabase(_connectionStringName))
            {
                var mappingInstance = Activator.CreateInstance(mapping) as IEntityMapping;

                mappingInstance.Apply(modelBuilder.Configurations);
            }
        }

        private static IEnumerable<Type> GetMappingTypesForDatabase(string connectionStringName)
        {
            return _mappingTypes.Where(x => _mappingTypesForDatabaseEvaluator(connectionStringName, x));
        }
    }
}