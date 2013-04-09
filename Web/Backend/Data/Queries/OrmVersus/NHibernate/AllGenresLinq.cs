using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogTalkRadio.Common.Data.Queries;
using Web.Backend.DomainModel.Entities.OrmVersus;

namespace Web.Backend.Data.Queries.OrmVersus.NHibernate
{
    public class AllGenresLinq: LinqQuery<GenreEntityNHibernate>
    {
        public override IQueryable<GenreEntityNHibernate> Apply(IQueryable<GenreEntityNHibernate> queryable)
        {
            return queryable;
        }
    }
}