using System;
using System.Collections.Generic;
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

        public override IQueryable<CategoryTopicEntity> Apply(IQueryable<CategoryTopicEntity> queryable)
        {
            queryable = queryable.Where(x => x.Category.GenreUrl == _categoryUrl);

            return base.Apply(queryable);
        }
    }

    public class CategoryTopicsByCategoryInAComplexMannerThatIJustCreated : StoredProcedureQuery<CategoryTopicEntity>
    {
        private readonly string _category;

        public CategoryTopicsByCategoryInAComplexMannerThatIJustCreated(string category)
        {
            _category = category;
        }

        public override string StoredProcedure
        {
            get { return "p_CategoryTopics_Sel"; }
        }

        public override IDictionary<string, object> Parameters
        {
            get
            {
                return new Dictionary<string, object>
                    {
                        { "GenreURL", _category }
                    };
            }
        }
    }
}