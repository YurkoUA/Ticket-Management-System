namespace TicketManagementSystem.Infrastructure.ChainOfResponsibility
{
    public interface IChainBuilder<TModel, TResult>
    {
        IChainElement<TModel, TResult> Head { get; }
        IChainBuilder<TModel, TResult> ConstructChain(IChainElement<TModel, TResult> element);
    }

    public interface IChainBuilder<TModel>
    {
        IChainElement<TModel> Head { get; }
        IChainBuilder<TModel> ConstructChain(IChainElement<TModel> element);
    }
}
