using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Web.Backend.Data.Orm.EntityFramework.Mappings;

namespace Web.Backend.Data.Orm.EntityFramework
{
    public class BtrDbContext: DbContext
    {
        private readonly string _connectionStringName;

        public BtrDbContext(string connectionStringName) : base(connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new BtrDatabaseInitializer());

            Configuration.LazyLoadingEnabled = false;

            foreach (var mapping in GetMappingsForDatabase(_connectionStringName))
            {
                mapping.Apply(modelBuilder.Configurations);
            }
        }

        private static IEnumerable<IEntityMapping> GetMappingsForDatabase(string connectionStringName)
        {
            return ServiceLocator.Current.GetAllInstances<IEntityMapping>().Where(x =>
                {
                    var type = x.GetType();
                var dbMappingAttribute =
                    type.GetCustomAttributes(typeof(DatabaseMappingAttribute), true)
                     .Cast<DatabaseMappingAttribute>()
                     .FirstOrDefault();

                if (dbMappingAttribute == null)
                {
                    return true;
                }

                return dbMappingAttribute.ConnectionStringNames.Contains(connectionStringName, StringComparer.OrdinalIgnoreCase);
            });
        }
    }
}