using System;
using System.Text;
using System.Web.Mvc;

namespace TicketManagementSystem.Web.Data
{
    public class Pagination
    {
        public Pagination(PageInfo pageInfo, Func<int, string> pageUrl, int buttonsOnPage)
        {
            _currentPage = pageInfo.PageNumber;
            _totalPages = pageInfo.TotalPages;
            _buttonsOnPage = buttonsOnPage;
            _pageUrl = pageUrl;
        }

        private readonly int _buttonsOnPage;

        private int _currentPage;
        private int _totalPages;
        private Func<int, string> _pageUrl;

        public MvcHtmlString CreatePagination()
        {
            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            var interval = GetPaginationInterval();
            ul.InnerHtml = GetButtons(interval.Item1, interval.Item2);

            return MvcHtmlString.Create(ul.ToString());
        }

        public Tuple<int, int> GetPaginationInterval()
        {
            int start, end;
            var offset = _buttonsOnPage / 2;

            if (_currentPage == 1)
            {
                start = 1;
                end = _buttonsOnPage;

                if (end > _totalPages)
                    end = _totalPages;
            }
            else if (_currentPage == _totalPages)
            {
                end = _totalPages;
                start = _totalPages - _buttonsOnPage + 1;

                if (start < 1)
                    start = 1;
            }
            else
            {
                start = _currentPage - offset;

                if (start < 1)
                {
                    start = 1;
                    end = _buttonsOnPage;

                    if (end > _totalPages)
                        end = _totalPages;
                }
                else
                {
                    end = _currentPage + offset;

                    if (end > _totalPages)
                        end = _totalPages;

                    start = end - _buttonsOnPage + 1;
                }
            }

            return Tuple.Create(start, end);
        }

        private string GetButtons(int start, int end)
        {
            var buttonsBuilder = new StringBuilder();

            if (start > 1)
                buttonsBuilder.Append(FirstPageButton());

            for (int i = start; i <= end; i++)
            {
                buttonsBuilder.Append(PageButton(i, i == _currentPage));
            }

            if (end != _totalPages)
                buttonsBuilder.Append(LastPageButton());

            return buttonsBuilder.ToString();
        }

        private string PageButton(int page, bool isActive)
        {
            var a = new TagBuilder("a");
            a.MergeAttribute("href", _pageUrl(page));
            a.SetInnerText(page.ToString());

            var li = new TagBuilder("li");
            li.InnerHtml = a.ToString();

            if (isActive)
                li.AddCssClass("active");

            return li.ToString();
        }

        private string FirstPageButton()
        {
            var span = new TagBuilder("span");
            span.MergeAttribute("aria-hidden", "true");
            span.InnerHtml = "&laquo;";

            var a = new TagBuilder("a");
            a.MergeAttribute("href", _pageUrl(1));
            a.InnerHtml = span.ToString();

            var li = new TagBuilder("li");
            li.InnerHtml = a.ToString();

            return li.ToString();
        }

        private string LastPageButton()
        {
            var span = new TagBuilder("span");
            span.MergeAttribute("aria-hidden", "true");
            span.InnerHtml = "&raquo;";

            var a = new TagBuilder("a");
            a.MergeAttribute("href", _pageUrl(_totalPages));
            a.InnerHtml = span.ToString();

            var li = new TagBuilder("li");
            li.InnerHtml = a.ToString();

            return li.ToString();
        }
    }
}