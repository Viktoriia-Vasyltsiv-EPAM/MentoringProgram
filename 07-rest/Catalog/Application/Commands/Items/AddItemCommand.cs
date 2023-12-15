using Catalog.Models;
using MediatR;

namespace Catalog.Application.Commands.Items
{
    public class AddItemCommand : IRequest<int>
    {
        public ItemDto Item { get; }
        public AddItemCommand(ItemDto item)
        {
            Item = item;
        }
    }
}
