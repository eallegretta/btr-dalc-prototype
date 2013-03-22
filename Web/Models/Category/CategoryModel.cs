
using System.Collections.Generic;

namespace Web.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public IList<CategoryModel> ChildCategories { get; set; }
    }
}