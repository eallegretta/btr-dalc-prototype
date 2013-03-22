using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Backend.DomainModel.Entities;

namespace Web.Backend.Data.Queries.Category
{
    public class AllCategoriesQuery: LinqPagedQuery<GenreEntity>
    {
        public override IQueryable<GenreEntity> Apply(IQueryable<GenreEntity> queryable)
        {
            queryable = queryable.OrderBy(x => x.GenreDescription);

            return base.Apply(queryable);
        }
    }
}