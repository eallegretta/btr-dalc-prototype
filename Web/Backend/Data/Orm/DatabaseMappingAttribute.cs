using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Backend.Data.Orm
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DatabaseMappingAttribute: Attribute
    {
        public DatabaseMappingAttribute(params string[] connectionStringNames)
        {
            ConnectionStringNames = connectionStringNames;
        }

        public string[] ConnectionStringNames { get; set; }

    }
}