using catalog.infrastructure;
using Catalog.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Queries.Categories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly CatalogContext _dbContext;

        public GetCategoriesQueryHandler(CatalogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToListAsync(cancellationToken);
        }
    }
}
