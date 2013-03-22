using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Controllers;

namespace Web.Mvc.ActionResults
{
    public class CrudOperationCompleteResult : ActionResult
    {
        private readonly string _message;
        private readonly Exception _exception;
        private readonly bool _refreshList;

        public CrudOperationCompleteResult(bool refreshList = true)
            : this("The operation has completed successfully", refreshList)
        {
        }

        public CrudOperationCompleteResult(string message, bool refreshList = true)
        {
            _message = message;
            _refreshList = refreshList;
        }

        public CrudOperationCompleteResult(Exception exception)
            : this("The operation threw the following exception: " + exception.Message, false)
        {
            _exception = exception;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "text/html";

            response.Write(@"<script type=""text/javascript"">");
            response.Write(string.Format(@"notify('{0}', '{1}', '{2}');", _message, (_exception != null ? "error" : "success"), "top"));
            response.Write("CrudViewModel.closeOpenedDialog();");
            if (_refreshList)
            {
                response.Write("location.hash = 'page/1';");
            }
            response.Write("</script>");
            response.End();
        }
    }
}