using catalog.infrastructure;
using MediatR;

namespace Catalog.Application.Commands.Items
{
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, bool>
    {
        private readonly CatalogContext _dbContext;

        public UpdateItemCommandHandler(CatalogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            if (request.Item == null)
            {
                throw new ArgumentNullException(nameof(request.Item), "Item cannot be null");
            }

            var item = await _dbContext.Items.FindAsync(request.Item.Id, cancellationToken);

            if (item == null)
            {
                throw new Exception($"Item with id {request.Item.Id} does not exist");
            }

            item.Name = request.Item.Name ?? item.Name;
            item.Description = request.Item.Description ?? item.Description;
            item.Price = request.Item.Price == default ? item.Price : request.Item.Price;

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
