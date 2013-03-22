using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using MvcContrib.UI.Grid.Syntax;
using MvcContrib.UI.Pager;

namespace System.Web.Mvc
{
    public static class HtmlHelperCrudExtensions
    {
		public static MvcHtmlString CrudScripts<T>(this HtmlHelper<T> helper,
			string listAction = "List", string createAction = "Create", 
			string detailsAction = "Details", string editAction = "Edit", 
			string deleteAction = "Delete")
		{
			StringBuilder html = new StringBuilder();

			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			html.Append("<script type=\"text/javascript\" src=\"/Scripts/crud/crud-view-model.js\"></script>");
			html.AppendFormat(@"<script type=""text/javascript"">
							CrudViewModel.configure({{
							listUrl: '{0}',
							createUrl: '{1}',
							detailsUrl: '{2}',
							editUrl: '{3}',
							deleteUrl: '{4}'
						}});
						</script>", urlHelper.Action(listAction), urlHelper.Action(createAction), urlHelper.Action(detailsAction), 
								  urlHelper.Action(editAction), urlHelper.Action(deleteAction));

			return MvcHtmlString.Create(html.ToString());
		}

		public static MvcHtmlString CrudIndex<T>(this HtmlHelper<T> helper, string containerId, string createItemText = null, bool showCreateButton = true, string listAction = "List")
		{
			StringBuilder html = new StringBuilder();
			html.Append("<div class=\"crud crud-list-container\">");
			if (showCreateButton)
			{
				html.AppendFormat(@"
<p>
	<button data-bind=""click: create"">{0}</button>
</p>", string.IsNullOrWhiteSpace(createItemText) ? "Create" : createItemText);
			}

			html.AppendFormat("<div id=\"{0}\" class=\"crud-list\">", containerId);
			html.Append(helper.Raw(helper.Action(listAction).ToString()));
			html.Append("</div>");
			html.Append("</div>");

			return MvcHtmlString.Create(html.ToString());
		}

		public static MvcHtmlString CrudBeginEditor<T>(this HtmlHelper<T> helper, string title, bool includeEditorForModel = true, bool hasInputFile = false)
		{
			var html = new StringBuilder();

			html.Append("<div class=\"crud crud-editor-container\">");
			html.Append("<div class=\"dialog-title\">");
			html.Append(title);
			html.Append("</div>");
			html.AppendFormat("<form method=\"POST\" action=\"{0}\"{1}>", helper.ViewContext.HttpContext.Request.RawUrl,
				hasInputFile ? "enctype=\"multipart/form-data\"" : string.Empty);
			html.Append(helper.AntiForgeryToken());
			html.Append(helper.ValidationSummary(true));
			if (includeEditorForModel)
			{
				html.Append(helper.EditorForModel());
			}

			return MvcHtmlString.Create(html.ToString());

		}

		public static MvcHtmlString CrudEndEditor<T>(this HtmlHelper<T> helper, string submitText = "Save")
		{
			var html = new StringBuilder();

			html.Append("<p class=\"crud-commands\">");
			html.AppendFormat("<input type=\"submit\" value=\"{0}\" />", submitText);
			html.Append("</p>");
			html.Append("</form>");
			html.Append("</div>");

			return MvcHtmlString.Create(html.ToString());

		}

		public static MvcHtmlString CrudEditor<T>(this HtmlHelper<T> helper, string title, bool hasInputFile = false)
		{
			var html = new StringBuilder();
			html.Append(helper.CrudBeginEditor(title, hasInputFile: hasInputFile));
			html.Append(helper.CrudEndEditor());
			
			return MvcHtmlString.Create(html.ToString());
		}

		public static MvcHtmlString CrudBeginDetails<T>(this HtmlHelper<T> helper, string title, bool includeDisplayForModel = true)
		{
			var html = new StringBuilder();

			html.Append("<div class=\"crud crud-details-container\">");
			html.Append("<div class=\"dialog-title\">");
			html.Append(title);
			html.Append("</div>");
			if (includeDisplayForModel)
			{
				html.Append(helper.DisplayForModel());
			}

			return MvcHtmlString.Create(html.ToString());
		}

		public static MvcHtmlString CrudEndDetails<T>(this HtmlHelper<T> helper)
		{
			return MvcHtmlString.Create("</div>");
		}

		public static MvcHtmlString CrudDetails<T>(this HtmlHelper<T> helper, string title)
		{
			var html = new StringBuilder();

			html.Append(helper.CrudBeginDetails(title));
			html.Append(helper.CrudEndDetails());

			return MvcHtmlString.Create(html.ToString());
		}


		public static MvcHtmlString CrudBeginDelete<T>(this HtmlHelper<T> helper, string title, string questionText)
		{
			var html = new StringBuilder();

			html.Append("<div class=\"crud crud-delete-container\">");
			html.Append("<div class=\"dialog-title\">");
			html.Append(title);
			html.Append("</div>");
			html.Append(questionText);
			html.AppendFormat("<form method=\"POST\" action=\"{0}\">", helper.ViewContext.HttpContext.Request.RawUrl);
			html.Append(helper.AntiForgeryToken());
			html.Append(helper.ValidationSummary(true));

			return MvcHtmlString.Create(html.ToString());
		}

		public static MvcHtmlString CrudEndDelete<T>(this HtmlHelper<T> helper)
		{
			var html = new StringBuilder();

			html.Append("<p class=\"crud-commands\"><input type=\"submit\" value=\"Delete\" /></p>");
			html.Append("</div>");

			return MvcHtmlString.Create(html.ToString());
		}

		public static MvcHtmlString CrudDelete<T>(this HtmlHelper<T> helper, string title, string questionText)
		{
			var html = new StringBuilder();

			html.Append(helper.CrudBeginDelete(title, questionText));
			html.Append(helper.CrudEndDelete());

			return MvcHtmlString.Create(html.ToString());
		}


		public static IGridWithOptions<T> CrudGrid<T>(this HtmlHelper helper, IEnumerable<T> dataSource, Expression<Func<T, object>> idExpr) where T : class
		{
			return helper.Grid<T>(dataSource).Columns(c =>
				{
					c.For(idExpr).Format("<a href=\"#details/{0}\">Details</a>").Attributes(style => "width:60px").Encode(false).InsertAt(0).Named(string.Empty);
					c.For(idExpr).Format("<a href=\"#edit/{0}\">Edit</a>").Attributes(style => "width:60px").Encode(false).InsertAt(1).Named(string.Empty);
					c.For(idExpr).Format("<a href=\"#delete/{0}\">Delete</a>").Attributes(style => "width:60px").Encode(false).InsertAt(2).Named(string.Empty);
				});
		}

		public static MvcHtmlString CrudBeginFilters<T>(this HtmlHelper<T> helper)
		{
			return MvcHtmlString.Create("<div class=\"filters\"><form id=\"filters_form\">");
		}

		public static MvcHtmlString CrudEndFilters<T>(this HtmlHelper<T> helper)
		{
			return MvcHtmlString.Create("<button data-bind=\"click: filter\">Filter</button><button data-bind=\"click: clearFilters\">Clear</button></form></div>");
		}

		public static MvcHtmlString CrudPager<T>(this HtmlHelper helper, IPagination<T> pagination)
		{
			return MvcHtmlString.Create(helper.Pager(pagination).Link(page => "#page/" + page).ToString());
		}
	}
}