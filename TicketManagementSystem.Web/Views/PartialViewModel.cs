namespace TicketManagementSystem.Web
{
    public class PartialViewModel
    {
        public string Title { get; set; }

        public string PartialViewName { get; set; }
        public int ModelId { get; set; }
        public object ModelObject { get; set; }

        public PartialViewModel() { }

        public PartialViewModel(int modelId, object model, string partialName, string title)
        {
            ModelId = modelId;
            ModelObject = model;
            PartialViewName = partialName;
            Title = title;
        }
    }
}