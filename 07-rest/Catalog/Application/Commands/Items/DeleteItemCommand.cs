using MediatR;

namespace Catalog.Application.Commands.Items
{
    public class DeleteItemCommand : IRequest<bool>
    {
        public int ItemId { get; }
        public DeleteItemCommand(int itemId)
        {
            ItemId = itemId;
        }
    }
}
