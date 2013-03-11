using Cinchcast.Framework.Collections;
using Web.Backend.DomainModel;

namespace Web.Backend.Contracts.Services
{
    public interface ICategoryTopicService
    {
        CategoryTopicEntity Get(int id);
        PagedList<CategoryTopicEntity> GetAll(IQuery<CategoryTopicEntity> query, int skip, int take);
    }
}