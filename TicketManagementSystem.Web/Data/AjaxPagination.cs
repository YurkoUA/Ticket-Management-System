using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace TicketManagementSystem.Web.Data
{
    public class AjaxPagination : Pagination
    {
        private AjaxOptions _ajaxOptions;

        public AjaxPagination(PageInfo pageInfo, Func<int, string> pageUrl, int buttonsOnPage, AjaxOptions ajaxOptions) : base(pageInfo, pageUrl, buttonsOnPage)
        {
            _ajaxOptions = ajaxOptions;
        }

        protected override string PageButton(int page, bool isActive)
        {
            var a = new TagBuilder("a");

            a.MergeAttribute("href", _pageUrl(page));
            ConfigureAjax(a);
            a.SetInnerText(page.ToString());

            var li = new TagBuilder("li")
            {
                InnerHtml = a.ToString()
            };

            if (isActive)
                li.AddCssClass("active");

            return li.ToString();
        }

        protected override string FirstPageButton()
        {
            var span = new TagBuilder("span");
            span.MergeAttribute("aria-hidden", "true");
            span.InnerHtml = "&laquo;";

            var a = new TagBuilder("a");

            a.MergeAttribute("href", _pageUrl(1));
            ConfigureAjax(a);
            a.InnerHtml = span.ToString();

            var li = new TagBuilder("li")
            {
                InnerHtml = a.ToString()
            };

            return li.ToString();
        }

        protected override string LastPageButton()
        {
            var span = new TagBuilder("span");
            span.MergeAttribute("aria-hidden", "true");
            span.InnerHtml = "&raquo;";

            var a = new TagBuilder("a");

            a.MergeAttribute("href", _pageUrl(_totalPages));
            ConfigureAjax(a);
            a.InnerHtml = span.ToString();

            var li = new TagBuilder("li")
            {
                InnerHtml = a.ToString()
            };

            return li.ToString();
        }

        private void ConfigureAjax(TagBuilder a)
        {
            a.MergeAttribute("data-ajax", "true");
            a.MergeAttribute("data-ajax-method", _ajaxOptions.HttpMethod);
            a.MergeAttribute("data-ajax-mode", _ajaxOptions.InsertionMode.ToString());
            a.MergeAttribute("data-ajax-update", "#" + _ajaxOptions.UpdateTargetId);

            if (!string.IsNullOrEmpty(_ajaxOptions.LoadingElementId))
            {
                a.MergeAttribute("data-ajax-loading", "#" + _ajaxOptions.LoadingElementId);
            }

            if (!string.IsNullOrEmpty(_ajaxOptions.OnBegin))
            {
                a.MergeAttribute("data-ajax-begin", _ajaxOptions.OnBegin);
            }

            if (!string.IsNullOrEmpty(_ajaxOptions.OnComplete))
            {
                a.MergeAttribute("data-ajax-complete", _ajaxOptions.OnComplete);
            }

            if (!string.IsNullOrEmpty(_ajaxOptions.OnFailure))
            {
                a.MergeAttribute("data-ajax-failure", _ajaxOptions.OnFailure);
            }

            if (!string.IsNullOrEmpty(_ajaxOptions.OnSuccess))
            {
                a.MergeAttribute("data-ajax-success", _ajaxOptions.OnSuccess);
            }
        }
    }
}