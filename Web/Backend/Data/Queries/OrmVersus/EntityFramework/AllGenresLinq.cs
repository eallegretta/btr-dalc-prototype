using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogTalkRadio.Common.Data.Queries;
using Web.Backend.DomainModel.Entities.OrmVersus;

namespace Web.Backend.Data.Queries.OrmVersus.EntityFramework
{
    public class AllGenresLinq: LinqQuery<GenreEntityEntityFramework>
    {
        public override IQueryable<GenreEntityEntityFramework> Apply(IQueryable<GenreEntityEntityFramework> queryable)
        {
            return queryable;
        }
    }
}