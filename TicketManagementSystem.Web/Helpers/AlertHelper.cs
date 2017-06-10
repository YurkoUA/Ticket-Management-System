using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public static class AlertHelper
    {
        public static MvcHtmlString AlertSuccess(this HtmlHelper html, string text)
        {
            return Alert("success", text);
        }

        public static MvcHtmlString AlertInfo(this HtmlHelper html, string text)
        {
            return Alert("info", text);
        }

        public static MvcHtmlString AlertWarning(this HtmlHelper html, string text)
        {
            return Alert("warning", text);
        }

        public static MvcHtmlString AlertDanger(this HtmlHelper html, string text)
        {
            return Alert("danger", text);
        }

        private static MvcHtmlString Alert(string alertType, string text)
        {
            var alert = new TagBuilder("div");

            alert.AddCssClass("alert");
            alert.AddCssClass($"alert-{alertType}");
            alert.MergeAttribute("role", "alert");
            alert.SetInnerText(text);

            return MvcHtmlString.Create(alert.ToString());
        }
    }
}