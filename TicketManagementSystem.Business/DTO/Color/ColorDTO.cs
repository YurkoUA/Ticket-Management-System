using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Business.DTO
{
    public class ColorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PackagesCount { get; set; }
        public int TicketsCount { get; set; }
    }
}
