using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Business.DTO
{
    public class TicketCreateDTO
    {
        [Required(ErrorMessage = "Необхідно вказати номер квитка.")]
        [RegularExpression(@"\d{6}", ErrorMessage = "Номер повинен складатися з шести цифр.")]
        public string Number { get; set; }

        public int? PackageId { get; set; }
        public int ColorId { get; set; }
        public int SerialId { get; set; }

        [Required(ErrorMessage = "Необхідно вказати номер серії.")]
        [RegularExpression(@"\d{2}", ErrorMessage = "Номер серії повинен складатися з двох цифр.")]
        public string SerialNumber { get; set; }

        [StringLength(128, ErrorMessage = "Примітка не може бути більшою за 128 символів.")]
        public string Note { get; set; }

        [StringLength(32, ErrorMessage = "Дата не може бути довша за 32 символи.")]
        public string Date { get; set; }
    }
}
