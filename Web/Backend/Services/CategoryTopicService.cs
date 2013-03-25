using Cinchcast.Framework.Collections;
using Web.Backend.Data;
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
        private readonly IRepository<CategoryTopicEntity> _categoryTopicRepo;

        public CategoryTopicService(IRepository<CategoryTopicEntity> categoryTopicRepo)
        {
            _categoryTopicRepo = categoryTopicRepo;
        }

        public CategoryTopicEntity Get(int id)
        {
            return _categoryTopicRepo.Get(id);
        }

        public PagedList<CategoryTopicEntity> GetAll(int skip, int take)
        {
            int count = _categoryTopicRepo.Count();

            return new PagedList<CategoryTopicEntity>(count, _categoryTopicRepo.GetAll(skip, take));
        }

        public PagedList<CategoryTopicEntity> GetAllByCategory(string category, int skip, int take)
        {
            var query = new CategoryTopicsByCategory { CategoryUrl = category, Skip = skip, Take = take };

            int count = _categoryTopicRepo.Count(query);

            return new PagedList<CategoryTopicEntity>(count, _categoryTopicRepo.Query(query));
        }
    }
}