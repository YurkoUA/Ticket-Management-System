using TicketManagementSystem.ViewModels.Statistics.Enums;

namespace TicketManagementSystem.ViewModels.Statistics
{
    public class ChartInfoVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string KeyTitle { get; set; }
        public string ValueTitle { get; set; }
        public ChartType Type { get; set; }
        public ChartComputingStrategy ComputingStrategy { get; set; }
        public string Color { get; set; }
        public bool Is3D { get; set; }
        public bool IsLegend { get; set; }
        public string StyleClass { get; set; }
    }
}
