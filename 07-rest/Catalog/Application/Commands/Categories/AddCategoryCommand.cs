using Catalog.Models;
using MediatR;

namespace Catalog.Application.Commands.Categories
{
    public class AddCategoryCommand : IRequest<int>
    {
        public CategoryDto Category { get; }

        public AddCategoryCommand(CategoryDto category)
        {
            Category = category;
        }
    }
}
