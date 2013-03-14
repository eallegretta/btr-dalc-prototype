using System;
using System.Linq;
using System.Linq.Expressions;
using Web.Backend.DomainModel.Entities;

namespace Web.Backend.Data.Queries.CategoryTopics
{
    public class CategoryTopicsByCategory : LinqPagedQuery<CategoryTopicEntity>
    {
        private readonly string _categoryUrl;

        public CategoryTopicsByCategory(string categoryUrl, int skip = 0, int take = 1000): base(skip, take)
        {
            _categoryUrl = categoryUrl;
        }

        public override void Apply(IQueryable<CategoryTopicEntity> queryable)
        {
            queryable = queryable.Where(x => x.Category.GenreUrl == _categoryUrl);

            base.Apply(queryable);
        }
    }
}