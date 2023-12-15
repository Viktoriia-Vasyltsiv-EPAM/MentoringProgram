using Catalog.Models;
using MediatR;

namespace Catalog.Application.Queries.Items
{
    public class GetItemsQuery : IRequest<List<ItemDto>>
    {
        public int CategoryId { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}
