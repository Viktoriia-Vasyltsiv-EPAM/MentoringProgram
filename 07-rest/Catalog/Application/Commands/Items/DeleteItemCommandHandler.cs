using catalog.infrastructure;
using MediatR;

namespace Catalog.Application.Commands.Items
{
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, bool>
    {
        private readonly CatalogContext _dbContext;
        private readonly ILogger<DeleteItemCommand> _logger;

        public DeleteItemCommandHandler(CatalogContext dbContext, ILogger<DeleteItemCommand> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await _dbContext.Items.FindAsync(request.ItemId, cancellationToken);

                if (item is null)
                {
                    throw new Exception($"Item with id {request.ItemId} does not exist");
                }

                _dbContext.Items.Remove(item);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something happened wrong");
                throw;
            }
        }
    }
}
