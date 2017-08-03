using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public class TicketFilterModel : IEnumerable<TicketDetailsModel>
    {
        [Display(Name = "Перша цифра")]
        public int? FirstNumber { get; set; }

        [Display(Name = "Колір")]
        public int? ColorId { get; set; }

        [Display(Name = "Серія")]
        public int? SerialId { get; set; }

        public string ColorName { get; set; }
        public string SerialName { get; set; }

        public int Page { get; set; } = 1;

        public PageInfo PageInfo { get; set; }
        public IEnumerable<TicketDetailsModel> Tickets { get; set; } = new List<TicketDetailsModel>();

        public SelectList Colors { get; set; }
        public SelectList Series { get; set; }

        public IEnumerable<SelectListItem> Numbers
        {
            get
            {
                int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                return numbers.Select(n => new SelectListItem { Text = n.ToString(), Value = n.ToString() });
            }
        }

        public override string ToString()
        {
            var sBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(ColorName))
            {
                sBuilder.Append(ColorName);

                if (!string.IsNullOrEmpty(SerialName) || FirstNumber != null)
                    sBuilder.Append(", ");
            }

            if (!string.IsNullOrEmpty(SerialName))
            {
                sBuilder.Append(SerialName);

                if (FirstNumber != null)
                    sBuilder.Append(", ");
            }

            if (FirstNumber != null)
                sBuilder.Append($"Цифра: {FirstNumber}");

            return sBuilder.ToString();
        }

        public bool IsNull()
        {
            return FirstNumber == null
                && ColorId == null
                && SerialId == null;
        }

        public IEnumerator<TicketDetailsModel> GetEnumerator()
        {
            return Tickets.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Tickets.GetEnumerator();
        }
    }
}