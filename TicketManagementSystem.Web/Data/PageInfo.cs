using System;

namespace TicketManagementSystem.Web.Data
{
    public class PageInfo
    {
        public const int PAGE_SIZE = 10;

        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);

        public PageInfo(int number, int total, int size = PAGE_SIZE)
        {
            PageNumber = number;
            TotalItems = total;
            PageSize = size;
        }
    }
}