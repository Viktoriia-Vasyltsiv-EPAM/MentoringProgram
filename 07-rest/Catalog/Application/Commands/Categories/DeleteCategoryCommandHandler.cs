using catalog.infrastructure;
using MediatR;

namespace Catalog.Application.Commands.Categories
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly CatalogContext _dbContext;
        private readonly ILogger<DeleteCategoryCommand> _logger;

        public DeleteCategoryCommandHandler(CatalogContext dbContext, ILogger<DeleteCategoryCommand> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FindAsync(request.CategoryId, cancellationToken);

            if (category is null)
            {
                throw new Exception($"The category with id {request.CategoryId} does not exist");
            }

            try
            {
                _dbContext.Categories.Remove(category);

                await _dbContext.SaveChangesAsync(cancellationToken);

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
