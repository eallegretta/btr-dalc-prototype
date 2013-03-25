using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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