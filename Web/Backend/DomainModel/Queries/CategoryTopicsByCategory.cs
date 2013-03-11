using System;
using System.Linq.Expressions;
using Web.Backend.Contracts;

namespace Web.Backend.DomainModel.Queries
{
    public class CategoryTopicsByCategory : IQuery<CategoryTopicEntity>
    {
        public CategoryTopicsByCategory(string categoryUrl)
        {
            MatchingCriteria = x => x.Category.GenreUrl == categoryUrl;
        }

        public Expression<Func<CategoryTopicEntity, bool>> MatchingCriteria { get; private set; }
    }
}