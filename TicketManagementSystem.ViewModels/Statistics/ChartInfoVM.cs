using TicketManagementSystem.ViewModels.Statistics.Enums;

namespace TicketManagementSystem.ViewModels.Statistics
{
    public class ChartInfoVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ChartType Type { get; set; }
        public ChartComputingStrategy ComputingStrategy { get; set; }
        public int SortOrder { get; set; }
        public string Color { get; set; }
        public bool Is3D { get; set; }
        public bool IsLegend { get; set; }
    }
}
