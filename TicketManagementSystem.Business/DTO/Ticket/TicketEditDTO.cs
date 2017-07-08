using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Business.DTO
{
    public class TicketEditDTO
    {
        public byte[] RowVersion { get; set; }      
        public int Id { get; set; }        
        public int ColorId { get; set; }        
        public int SerialId { get; set; }

        public bool CanSelectColor { get; set; } = true;
        public bool CanSelectSerial { get; set; } = true;

        [Required(ErrorMessage = "Необхідно вказати номер серії.")]
        [RegularExpression(@"\d{2}", ErrorMessage = "Номер серії повинен складатися з двох цифр.")]
        public string SerialNumber { get; set; }

        [StringLength(128, ErrorMessage = "Примітка не може бути більшою за 128 символів.")]
        public string Note { get; set; }

        [StringLength(32, ErrorMessage = "Дата не може бути довша за 32 символи.")]
        public string Date { get; set; }
    }
}
