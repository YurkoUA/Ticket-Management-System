using System;
using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public static class LocalDateHelper
    {
        public static MvcHtmlString DisplayDateTime(this HtmlHelper html, DateTime dateTime, string format)
        {
            var span = new TagBuilder("span");
            span.MergeAttribute("data-date", (dateTime.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds).ToString());
            span.SetInnerText(dateTime.ToString(format));
            return MvcHtmlString.Create(span.ToString());
        }
    }
}