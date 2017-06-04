using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Business.DTO
{
    public class SerialEditDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool CanBeDeleted { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
