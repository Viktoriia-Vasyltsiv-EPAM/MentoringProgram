using Catalog.Models;
using MediatR;

namespace Catalog.Application.Commands.Items
{
    public class UpdateItemCommand : IRequest<bool>
    {
        public ItemDto Item { get; }
        public UpdateItemCommand(ItemDto item)
        {
            Item = item;
        }
    }
}
