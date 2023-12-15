using catalog.infrastructure;
using catalog.infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Commands.Categories
{

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, int>
    {
        private readonly CatalogContext _dbContext;

        public AddCategoryCommandHandler(CatalogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.Category is null)
            {
                throw new ArgumentNullException(nameof(request.Category));
            }

            var exists = await _dbContext.Categories.AnyAsync(x => x.Name == request.Category.Name, cancellationToken);

            if (exists)
            {
                throw new Exception($"Category with name \"{request.Category.Name}\" already exists");
            }

            var category = new Category
            {
                Name = request.Category.Name
            };

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return category.Id;
        }
    }
}
