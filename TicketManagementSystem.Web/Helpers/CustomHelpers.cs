using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public static class CustomHelpers
    {
        public static MvcHtmlString DisplayAddButtonFor(this HtmlHelper html, string url, string tooltip, bool hiddenSmXs = true)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("add-button");
            div.MergeAttribute("tooltip", tooltip);
            div.MergeAttribute("placement", "left");
            div.MergeAttribute("container", "body");

            if (hiddenSmXs)
            {
                div.AddCssClass("hidden-sm");
                div.AddCssClass("hidden-xs");
            }

            var p = new TagBuilder("p");
            p.AddCssClass("plus");

            var a = new TagBuilder("a");
            a.MergeAttribute("href", url);
            a.SetInnerText("+");

            p.InnerHtml = a.ToString();
            div.InnerHtml = p.ToString();

            return MvcHtmlString.Create(div.ToString());
        }
    }
}