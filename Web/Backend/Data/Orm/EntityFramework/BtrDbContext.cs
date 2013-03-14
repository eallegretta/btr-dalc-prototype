using System.Data.Entity;
using Microsoft.Practices.ServiceLocation;
using Web.Backend.Data.Orm.EntityFramework.Mappings;

namespace Web.Backend.Data.Orm.EntityFramework
{
    public class BtrDbContext: DbContext
    {
        public BtrDbContext(string connectionStringName) : base(connectionStringName)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new BtrDatabaseInitializer());

            Configuration.LazyLoadingEnabled = false;

            foreach (var mapping in ServiceLocator.Current.GetAllInstances<IEntityMapping>())
            {
                mapping.Apply(modelBuilder.Configurations);
            }
        }
    }
}