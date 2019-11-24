using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagementSystem.Data.Entities
{
    [Table("tStatisticsPage")]
    public class StatisticsPage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
    }
}
