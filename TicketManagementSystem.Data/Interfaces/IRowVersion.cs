using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Data.Interfaces
{
    public interface IRowVersion
    {
        [Timestamp]
        byte[] RowVersion { get; set; }
    }
}
