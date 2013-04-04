using System.Collections.Generic;
using System.Web.Mvc;
using BlogTalkRadio.Common.Data;
using Cinchcast.Framework.Collections;
using MvcContrib.Pagination;
using Web.Backend.Data;
using Web.Backend.Data.Queries.Category;
using Web.Backend.Data.Queries.CategoryTopics;
using Web.Backend.DomainModel;
using Web.Backend.DomainModel.Contracts;
using Web.Backend.DomainModel.Contracts.Services;
using Web.Backend.DomainModel.Entities;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<GenreEntity> _categoryRepo;
        private readonly ICategoryTopicService _categoryTopicService;

        public HomeController(IRepository<GenreEntity> categoryRepo, ICategoryTopicService categoryTopicService)
        {
            _categoryRepo = categoryRepo;
            _categoryTopicService = categoryTopicService;
        }

        public ActionResult Index(int page = 1, string category = null)
        {
            var customPagination = GetCategoryTopics(page, category);

            return View(new HomeIndexViewModel(_categoryRepo.Query(new AllCategoriesQuery()), customPagination));
        }

        private CustomPagination<CategoryTopicEntity> GetCategoryTopics(int page, string category)
        {
            int take = 15;
            int skip = (page - 1) * take;

            PagedList<CategoryTopicEntity> topics;

            if (!string.IsNullOrWhiteSpace(category))
            {
                topics = _categoryTopicService.GetAllByCategory(category, skip, take);
            }
            else
            {
                topics = _categoryTopicService.GetAll(skip, take);
            }

            var customPagination = new CustomPagination<CategoryTopicEntity>(topics, page, take, topics.TotalItems);
            return customPagination;
        }

        public ActionResult List(int page = 1, string category = null)
        {
            var customPagination = GetCategoryTopics(page, category);

            return PartialView(customPagination);
        }

        public ActionResult Edit(int id)
        {
            var topic = _categoryTopicService.Get(id);

            if (topic == null)
            {
                return HttpNotFound("The topic you are trying to edit does not exist");
            }

            return View(topic);
        }
    }
}