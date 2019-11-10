using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagementSystem.Data.Entities
{
    [Table("tStatisticsChart")]
    public class StatisticsChart
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TypeId { get; set; }
        public int ComputingStrategyId { get; set; }
        public int? PageId { get; set; }
        public int SortOrder { get; set; }
        public string SPName { get; set; }
        public string Color { get; set; }
        public bool Is3D { get; set; }
        public bool IsLegend { get; set; }
    }
}
