using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using TicketManagementSystem.Business.Enums;
using TicketManagementSystem.Business.Extensions;

namespace TicketManagementSystem.Web
{
    public class PackageFilterModel : IEnumerable<PackageDetailsModel>
    {
        [Display(Name = "Колір")]
        public int? ColorId { get; set; }

        [Display(Name = "Серія")]
        public int? SerialId { get; set; }

        [Display(Name = "Перша цифра")]
        public int? FirstNumber { get; set; }

        [Display(Name = "Статус")]
        public PackageStatusFilter Status { get; set; }
        
        public string ColorName { get; set; }
        public string SerialName { get; set; }

        public SelectList Colors { get; set; }
        public SelectList Series { get; set; }

        public IEnumerable<PackageDetailsModel> Packages { get; set; } = new List<PackageDetailsModel>();

        public IEnumerable<SelectListItem> Numbers
        {
            get
            {
                int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                return numbers.Select(n => new SelectListItem { Text = n.ToString(), Value = n.ToString() });
            }
        }

        public bool IsNull()
        {
            return ColorId == null
                && SerialId == null
                && FirstNumber == null;
        }

        public override string ToString()
        {
            if (IsNull())
                return null;

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

            if (Status != PackageStatusFilter.None)
                sBuilder.Append($", {Status.GetDisplayName()}");

            return sBuilder.ToString();
        }

        #region IEnumerable<T> implementation.

        public IEnumerator<PackageDetailsModel> GetEnumerator()
        {
            return Packages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Packages.GetEnumerator();
        }

        #endregion
    }
}