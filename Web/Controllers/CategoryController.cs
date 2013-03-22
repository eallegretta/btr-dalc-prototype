using Web.Backend.DomainModel.Entities;
using Web.Backend.Services.Commands.Category;
using Web.Models;

namespace Web.Controllers
{
    public class CategoryController: CrudController<GenreEntity, CategoryModel, CreateCategoryCommand, UpdateCategoryCommand, DeleteCategoryCommand>
    {
    }
}