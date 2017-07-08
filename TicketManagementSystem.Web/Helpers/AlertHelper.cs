using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public static class AlertHelper
    {
        public static MvcHtmlString AlertSuccess(this HtmlHelper html, string text, bool dismissible = false)
        {
            return Alert("success", text, dismissible);
        }

        public static MvcHtmlString AlertInfo(this HtmlHelper html, string text, bool dismissible = false)
        {
            return Alert("info", text, dismissible);
        }

        public static MvcHtmlString AlertWarning(this HtmlHelper html, string text, bool dismissible = false)
        {
            return Alert("warning", text, dismissible);
        }

        public static MvcHtmlString AlertDanger(this HtmlHelper html, string text, bool dismissible = false)
        {
            return Alert("danger", text, dismissible);
        }

        private static MvcHtmlString Alert(string alertType, string text, bool dismissible)
        {
            TagBuilder closeButton = null;

            if (dismissible)
            {
                var spanClose = new TagBuilder("span");
                spanClose.MergeAttribute("aria-hidden", "true");
                spanClose.InnerHtml = "&times;";

                closeButton = new TagBuilder("button");
                closeButton.AddCssClass("close");
                closeButton.MergeAttribute("type", "button");
                closeButton.MergeAttribute("data-dismiss", "alert");
                closeButton.MergeAttribute("aria-label", "Close");

                closeButton.InnerHtml = spanClose.ToString();
            }

            var alert = new TagBuilder("div");

            alert.AddCssClass("alert");
            alert.AddCssClass($"alert-{alertType}");
            alert.MergeAttribute("role", "alert");

            if (dismissible)
            {
                alert.AddCssClass("alert-dismissible");
                alert.InnerHtml = closeButton.ToString() + "<br>" + text;
            }
            else
            {
                alert.SetInnerText(text);
            }
            return MvcHtmlString.Create(alert.ToString());
        }
    }
}