using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Business.DTO
{
    public class ColorEditDTO
    {
        public int Id { get; set; }

        [StringLength(32, MinimumLength = 3, ErrorMessage = "Назва повинна бути від 3 до 32 символів.")]
        public string Name { get; set; }

        public bool CanBeDeleted { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
