using Catalog.Models;
using MediatR;

namespace Catalog.Application.Commands.Categories
{
    public class UpdateCategoryCommand : IRequest<bool>
    {
        public CategoryDto Category { get; }
        public UpdateCategoryCommand(CategoryDto category)
        {
            Category = category;
        }
    }
}
