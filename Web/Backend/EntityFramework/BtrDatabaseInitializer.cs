using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Web.Backend.EntityFramework
{
    public class BtrDatabaseInitializer: IDatabaseInitializer<BtrDbContext>
    {
        public void InitializeDatabase(BtrDbContext context)
        {
        }
    }
}