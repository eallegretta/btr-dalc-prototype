using Cinchcast.Framework.Collections;
using Web.Backend.Contracts;
using Web.Backend.Contracts.Services;
using Web.Backend.DomainModel;
using System.Linq;

namespace Web.Backend.Services
{
    public class CategoryTopicService: ICategoryTopicService
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

        public PagedList<CategoryTopicEntity> GetAll(IQuery<CategoryTopicEntity> query, int skip, int take)
        {
            int count = _categoryTopicRepo.Count(query);


            var queryResult = _categoryTopicRepo.Query().EagerLoad(x => x.Category).Where(query).OrderBy(x => x.Name).Skip(skip).Take(take).ToList();

            return new PagedList<CategoryTopicEntity>(count, queryResult);
        }
    }
}