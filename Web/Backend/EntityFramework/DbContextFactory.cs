using System;
using System.Data.Entity;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Web.Backend.EntityFramework
{
    public interface IDbContextFactory
    {
        string ConnectionStringName { get; set; }
        DbContext OpenDbContext();
        DbContext GetCurrentDbContext();
    }

    public class DbContextFactory: IDbContextFactory
    {
        private readonly string _uid;

        public DbContextFactory()
        {
            _uid = Guid.NewGuid().ToString();
        }

        public string ConnectionStringName { get; set; }

        public DbContext OpenDbContext()
        {
            if (ConnectionStringName == null) return null;

            CurrentDbContext = new BtrDbContext(ConnectionStringName);
            return CurrentDbContext;
        }

        public DbContext GetCurrentDbContext()
        {
            return CurrentDbContext;
        }

        private DbContext CurrentDbContext
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Items[_uid] as DbContext;
                }
                return CallContext.GetData(_uid) as DbContext;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[_uid] = value;
                }
                else
                {
                    CallContext.SetData(_uid, value);
                }
            }
        }
    }
}