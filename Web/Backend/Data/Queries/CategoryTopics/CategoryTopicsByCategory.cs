using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlogTalkRadio.Common.Data.Queries;
using Web.Backend.DomainModel.Entities;

namespace Web.Backend.Data.Queries.CategoryTopics
{
    public class CategoryTopicsByCategory : LinqPagedQuery<CategoryTopicEntity>
    {
        public string CategoryUrl { get; set; }

        public override IQueryable<CategoryTopicEntity> Apply(IQueryable<CategoryTopicEntity> queryable)
        {
            if (!string.IsNullOrWhiteSpace(CategoryUrl))
            {
                queryable = queryable.Where(x => x.Category.GenreUrl == CategoryUrl);    
            }

            return base.Apply(queryable.OrderBy(x => x.Description));
        }
    }
}