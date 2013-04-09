using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using BlogTalkRadio.Common.Data;
using Web.Backend.DomainModel.Entities.OrmVersus;
using NHQueries = Web.Backend.Data.Queries.OrmVersus.NHibernate;
using EFQueries = Web.Backend.Data.Queries.OrmVersus.EntityFramework;
using DapperQueries = Web.Backend.Data.Queries.OrmVersus.Dapper;
namespace Web.Controllers
{
    public class OrmVersusController : Controller
    {
        private readonly IRepository<GenreEntityDapper> _dapperRepo;
        private readonly IRepository<GenreEntityEntityFramework> _efRepo;
        private readonly IRepository<GenreEntityNHibernate> _nhRepo;

        public OrmVersusController(IRepository<GenreEntityDapper> dapperRepo, IRepository<GenreEntityEntityFramework> efRepo, IRepository<GenreEntityNHibernate> nhRepo)
        {
            _dapperRepo = dapperRepo;
            _efRepo = efRepo;
            _nhRepo = nhRepo;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Timer = Stopwatch.StartNew();
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.Timer.Stop();
            base.OnActionExecuted(filterContext);
        }

        public ActionResult All(string orm, string queryType = "linq")
        {
            switch (orm)
            {
                case "nh":
                    IQuery<GenreEntityNHibernate> nhQuery;
                    switch (queryType)
                    {
                        case "linq":
                            nhQuery = new NHQueries.AllGenresLinq();
                            break;
                        case "native":
                            nhQuery = new NHQueries.AllGenresNative();
                            break;
                        case "sp":
                            nhQuery = new NHQueries.AllGenresSp();
                            break;
                        default:
                            throw new Exception("Must specify the query type");
                    }

                    return View("All", _nhRepo.Query(nhQuery));
                case "ef":
                    IQuery<GenreEntityEntityFramework> efQuery;
                    switch (queryType)
                    {
                        case "linq":
                            efQuery = new EFQueries.AllGenresLinq();
                            break;
                        case "native":
                            throw new Exception("Not supported query type");
                        case "sp":
                            efQuery = new EFQueries.AllGenresSp();
                            break;
                        default:
                            throw new Exception("Must specify the query type");
                    }

                    return View("All", _efRepo.Query(efQuery));
                case "dapper":
                    IQuery<GenreEntityDapper> dapperQuery;
                    switch (queryType)
                    {
                        case "linq":
                            throw new Exception("Not supported query type");
                        case "native":
                            dapperQuery = new DapperQueries.AllGenresNative();
                            break;
                        case "sp":
                            dapperQuery = new DapperQueries.AllGenresSp();
                            break;
                        default:
                            throw new Exception("Must specify the query type");
                    }

                    return View("All", _dapperRepo.Query(dapperQuery));
                default:
                    throw new Exception("Must specify the orm");
            }

        }
    }
}