using catalog.infrastructure.Entities;
using catalog.infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Commands.Items
{
    public class AddItemCommandHandler : IRequestHandler<AddItemCommand, int>
    {
        private readonly CatalogContext _dbContext;

        public AddItemCommandHandler(CatalogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            var categoryExists = await _dbContext.Categories
                .Where(x => x.Id == request.Item.CategoryId)
                .AnyAsync(cancellationToken);

            if (!categoryExists)
            {
                throw new Exception($"The category with id {request.Item.CategoryId} does not exist");
            }

            var item = new Item
            {
                Name = request.Item.Name,
                Description = request.Item.Description,
                CategoryId = request.Item.CategoryId,
                Price = request.Item.Price
            };

            _dbContext.Items.Add(item);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return item.Id;
        }
    }
}
