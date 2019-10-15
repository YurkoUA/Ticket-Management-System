namespace TicketManagementSystem.Infrastructure.ChainOfResponsibility
{
    public class ChainBuilder<TModel, TResult> : IChainBuilder<TModel, TResult>
    {
        public IChainElement<TModel, TResult> Head { get; set; }
        private IChainElement<TModel, TResult> previous { get; set; }

        public IChainBuilder<TModel, TResult> ConstructChain(IChainElement<TModel, TResult> element)
        {
            if (Head == null)
            {
                Head = element;
                previous = Head;
            }
            else
            {
                previous.Next = element;
                previous = previous.Next;
            }

            return this;
        }
    }

    public class ChainBuilder<TModel> : IChainBuilder<TModel>
    {
        public IChainElement<TModel> Head { get; set; }
        private IChainElement<TModel> previous { get; set; }

        public IChainBuilder<TModel> ConstructChain(IChainElement<TModel> element)
        {
            if (Head == null)
            {
                Head = element;
                previous = Head;
            }
            else
            {
                previous.Next = element;
                previous = previous.Next;
            }

            return this;
        }
    }
}
