using System.ComponentModel.DataAnnotations;
using TicketManagementSystem.Business.Attributes;

namespace TicketManagementSystem.Business.DTO
{
    public class TicketCreateDTO
    {
        [Required(ErrorMessage = "Необхідно вказати номер квитка.")]
        [TicketNumber(ErrorMessage = "Номер повинен складатися з шести цифр.")]
        public string Number { get; set; }

        public int? PackageId { get; set; }
        public int ColorId { get; set; }
        public int SerialId { get; set; }

        [Required(ErrorMessage = "Необхідно вказати номер серії.")]
        [SerialNumber(ErrorMessage = "Номер серії повинен бути від 01 до 50.")]
        public string SerialNumber { get; set; }

        [StringLength(128, ErrorMessage = "Примітка не може бути більшою за 128 символів.")]
        public string Note { get; set; }

        [StringLength(32, ErrorMessage = "Дата не може бути довша за 32 символи.")]
        public string Date { get; set; }
    }
}
