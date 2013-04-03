using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BlogTalkRadio.Common.Data;
using BlogTalkRadio.Common.Data.Queries;
using Cinchcast.Framework.Commands;
using Microsoft.Practices.ServiceLocation;
using MvcContrib.Pagination;
using Web.Backend.Data;
using Web.Backend.Data.Queries;
using Web.Backend.DomainModel.Contracts;
using Web.Backend.Services.Commands;
using Web.Mvc.ActionResults;

namespace Web.Controllers
{
    public abstract class CrudController<TEntity, TModel, TCreateCommand, TUpdateCommand, TDeleteCommand> : Controller
        where TEntity : class,new()
        where TModel : new()
        where TCreateCommand : Command
        where TUpdateCommand : CrudUpdateCommand<TEntity>
        where TDeleteCommand : CrudDeleteCommand<TEntity>
    {
        public IMappingEngine Mapper { get; set; }
        public IRepository<TEntity> Repository { get; set; }

        public ICommandProcessor CommandProcessor
        {
            get;
            set;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View();
        }

        protected virtual List<TEntity> ListQuery(IQuery<TEntity> query)
        {
            return Repository.Query(query);
        }

        protected virtual int CountQuery(IQuery<TEntity> query = null)
        {
            return Repository.Count(query);
        }

        protected virtual TEntity GetQuery(int entityId, CrudAction action)
        {
            return Repository.Get(entityId);
        }

        public virtual ActionResult List(int page = 1)
        {
            int skip = (page - 1)*15;

            var items = ListQuery(new LinqPagedQuery<TEntity> { Skip = skip, Take = 15 });

            var customPagination = new CustomPagination<TEntity>(items, page, 15, CountQuery());

            return PartialView(customPagination);
        }

        public virtual ActionResult Details(int id)
        {
            var instance = GetQuery(id, CrudAction.Details);

            if (instance == null)
            {
                return new CrudOperationCompleteResult("The " + typeof(TEntity).Name + " you are trying to edit with id " + id + " does not exist");
            }

            var model = Mapper.Map<TModel>(instance);

            return PartialView(model);
        }

        public virtual ActionResult Create()
        {
            return PartialView(new TModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public virtual ActionResult Create(TModel model)
        {
            if (ModelState.IsValid)
            {
                var createCommand = ServiceLocator.Current.GetInstance<TCreateCommand>();

                createCommand = Mapper.Map(model, createCommand);

                CommandProcessor.Process(createCommand);

                if (createCommand.IsValid)
                {
                    return new CrudOperationCompleteResult("The " + typeof(TEntity).Name + " has been succesfully created");
                }
                
                ModelState.AddValidationResults(createCommand.ValidationResults());
            }

            return PartialView(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var category = GetQuery(id, CrudAction.Edit);

            if (category == null)
                return new CrudOperationCompleteResult("The " + typeof(TEntity).Name + " you are trying to edit with id " + id + " does not exist");

            var model = Mapper.Map<TModel>(category);

            return PartialView(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public virtual ActionResult Edit(int id, TModel model)
        {
            if (ModelState.IsValid)
            {
                var updateCommand = ServiceLocator.Current.GetInstance<TUpdateCommand>();

                updateCommand = Mapper.Map(model, updateCommand);
                updateCommand.Id = id;
                CommandProcessor.Process(updateCommand);

                if (updateCommand.IsValid)
                {
                    return new CrudOperationCompleteResult("The " + typeof(TEntity).Name + " has been succesfully edited");
                }
                else
                {
                    ModelState.AddValidationResults(updateCommand.ValidationResults());
                }
            }

            return PartialView(model);
        }
        
        public virtual ActionResult Delete(int id)
        {
            var category = GetQuery(id, CrudAction.Delete);

            if (category == null)
                return new CrudOperationCompleteResult("The " + typeof(TEntity).Name + " you are trying to delete with id " + id + " does not exist");

            var model = Mapper.Map<TModel>(category);

            return PartialView(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public virtual ActionResult Delete(int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var deleteCommand = ServiceLocator.Current.GetInstance<TDeleteCommand>();
                deleteCommand.Id = id;

                CommandProcessor.Process(deleteCommand);

                if (deleteCommand.IsValid)
                {
                    return new CrudOperationCompleteResult("The " + typeof(TEntity).Name + " has been succesfully deleted");
                }
                else
                {
                    ModelState.AddValidationResults(deleteCommand.ValidationResults());
                }
            }

            return RedirectToAction("Delete", new { id });
        }
    }
}