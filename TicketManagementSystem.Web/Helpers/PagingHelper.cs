using System;
using System.Web.Mvc;
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
    }
}