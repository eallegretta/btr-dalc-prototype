using Cinchcast.Framework.Collections;
using Web.Backend.DomainModel.Entities;

namespace Web.Backend.DomainModel.Contracts.Services
{
    public interface ICategoryTopicService
    {
        CategoryTopicEntity Get(int id);
        PagedList<CategoryTopicEntity> GetAll(int skip, int take);
        PagedList<CategoryTopicEntity> GetAllByCategory(string category, int skip, int take);
    }
}