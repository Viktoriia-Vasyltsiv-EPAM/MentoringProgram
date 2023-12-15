using catalog.infrastructure;
using Catalog.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Queries.Items
{
    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, List<ItemDto>>
    {
        private readonly CatalogContext _dbContext;

        public GetItemsQueryHandler(CatalogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _dbContext.Items
                .Where(x => x.CategoryId == request.CategoryId)
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .Select(x => new ItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    CategoryId = x.CategoryId,
                })
                .ToListAsync(cancellationToken);

            if (items is null || items.Count == 0)
            {
                throw new Exception($"No items with category id {request.CategoryId}");
            }

            return items;
        }
    }
}
