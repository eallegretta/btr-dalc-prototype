using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.Pagination;
using Web.Backend.DomainModel.Entities;

namespace Web.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<GenreEntity> Categories { get; private set; }
        public IPagination<CategoryTopicEntity> CategoryTopics { get; private set; }

        public HomeIndexViewModel(IEnumerable<GenreEntity> categories, IPagination<CategoryTopicEntity> categoryTopics)
        {
            Categories = categories;
            CategoryTopics = categoryTopics;
        }
    }
}