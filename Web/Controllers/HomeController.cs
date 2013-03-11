﻿using System.Collections.Generic;
using System.Web.Mvc;
using Cinchcast.Framework.Collections;
using Web.Backend.Contracts.Services;
using MvcContrib.Pagination;
using Web.Backend.DomainModel;
using Web.Backend.DomainModel.Queries;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryTopicService _categoryTopicService;

        public HomeController(ICategoryTopicService categoryTopicService)
        {
            _categoryTopicService = categoryTopicService;
        }

        public ActionResult Index(int page = 1, string category = null)
        {
            int take = 15;
            int skip = (page - 1) * take;

            PagedList<CategoryTopicEntity> topics;

            if (!string.IsNullOrWhiteSpace(category))
                topics = _categoryTopicService.GetAll(new CategoryTopicsByCategory(category), skip, take);
            else
            {
                topics = _categoryTopicService.GetAll(null, skip, take);
            }

            var customPagination = new CustomPagination<CategoryTopicEntity>(topics, page, take, topics.TotalItems);

            return View(customPagination);
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