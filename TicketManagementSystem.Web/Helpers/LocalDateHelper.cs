using System;
using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public static class LocalDateHelper
    {
        public static MvcHtmlString DisplayLocalDate(this HtmlHelper html, DateTime dateTime, string format)
        {
            var span = new TagBuilder("span");
            span.SetInnerText(dateTime.ToLocalTime().ToString(format));

            return MvcHtmlString.Create(span.ToString());
        }
    }
}