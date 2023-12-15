using catalog.infrastructure;
using MediatR;

namespace Catalog.Application.Commands.Categories
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly CatalogContext _dbContext;

        public UpdateCategoryCommandHandler(CatalogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.Category is null)
            {
                throw new ArgumentNullException(nameof(request.Category));
            }

            var category = await _dbContext.Categories.FindAsync(request.Category.Id, cancellationToken);

            if (category is null)
            {
                throw new Exception($"The category with id {request.Category.Id} does not exist");
            }

            category.Name = request.Category.Name;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
