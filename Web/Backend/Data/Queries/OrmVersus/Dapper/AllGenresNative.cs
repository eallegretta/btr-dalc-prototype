using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogTalkRadio.Common.Data.Dapper.Queries;
using DapperExtensions;
using Web.Backend.DomainModel.Entities.OrmVersus;

namespace Web.Backend.Data.Queries.OrmVersus.Dapper
{
    public class AllGenresNative: DapperQuery<GenreEntityDapper>
    {
        public override IPredicate Predicate
        {
            get { return null; }
        }
    }
}