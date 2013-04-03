using BlogTalkRadio.Common.Data;
using BlogTalkRadio.Common.Data.Queries;
using Cinchcast.Framework.Collections;
using Web.Backend.Data;
using Web.Backend.Data.Queries;
using Web.Backend.Data.Queries.CategoryTopics;
using Web.Backend.DomainModel;
using System.Linq;
using Web.Backend.DomainModel.Contracts;
using Web.Backend.DomainModel.Contracts.Services;
using Web.Backend.DomainModel.Entities;

namespace Web.Backend.Services
{
    public class CategoryTopicService : ICategoryTopicService
    {
        private readonly IRepository<CategoryTopicEntity> _categoryTopicRepository;

        public CategoryTopicService(IRepository<CategoryTopicEntity> categoryTopicRepository)
        {
            _categoryTopicRepository = categoryTopicRepository;
        }

        public CategoryTopicEntity Get(int id)
        {
            return _categoryTopicRepository.Get(id);
        }

        public PagedList<CategoryTopicEntity> GetAll(int skip, int take)
        {
            int count = _categoryTopicRepository.Count();

            var allQuery = new LinqAdHocSortedQuery<CategoryTopicEntity, int>(null, x => x.Id)
                {
                    Skip = skip,
                    Take = take
                };

            return new PagedList<CategoryTopicEntity>(count, _categoryTopicRepository.Query(allQuery));
        }

        public PagedList<CategoryTopicEntity> GetAllByCategory(string category, int skip, int take)
        {
            var query = new CategoryTopicsByCategory { CategoryUrl = category, Skip = skip, Take = take };

            int count = _categoryTopicRepository.Count(query);

            return new PagedList<CategoryTopicEntity>(count, _categoryTopicRepository.Query(query));
        }
    }
}