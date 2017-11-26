using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TicketManagementSystem.Web.Data;

namespace TicketManagementSystem.Web
{
    public static class PagingHelper
    {
        private const int BUTTON_ON_PAGE = 7;

        public static MvcHtmlString PageLinks(this HtmlHelper html, PageInfo pageInfo, Func<int, string> pageUrl)
        {
            var pagination = new Pagination(pageInfo, pageUrl, BUTTON_ON_PAGE);
            return pagination.CreatePagination();
        }

        public static MvcHtmlString PageLinks(this AjaxHelper ajax, PageInfo pageInfo, Func<int, string> pageUrl, AjaxOptions ajaxOptions)
        {
            var pagination = new AjaxPagination(pageInfo, pageUrl, BUTTON_ON_PAGE, ajaxOptions);
            return pagination.CreatePagination();
        }
    }
}