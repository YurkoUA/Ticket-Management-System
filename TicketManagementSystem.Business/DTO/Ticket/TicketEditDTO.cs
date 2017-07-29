using System.ComponentModel.DataAnnotations;
using TicketManagementSystem.Business.Attributes;

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
        [SerialNumber(ErrorMessage = "Номер серії повинен бути від 01 до 50.")]
        public string SerialNumber { get; set; }

        [StringLength(128, ErrorMessage = "Примітка не може бути більшою за 128 символів.")]
        public string Note { get; set; }

        [StringLength(32, ErrorMessage = "Дата не може бути довша за 32 символи.")]
        public string Date { get; set; }
    }
}
