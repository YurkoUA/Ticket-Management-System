namespace TicketManagementSystem.Infrastructure.ChainOfResponsibility
{
    public interface IChainElement<TModel, TResult>
    {
        IChainElement<TModel, TResult> Next { get; set; }
        TResult HandleRequest(TModel model);
    }

    public interface IChainElement<TModel>
    {
        IChainElement<TModel> Next { get; set; }
        void HandleRequest(TModel model);
    }
}
