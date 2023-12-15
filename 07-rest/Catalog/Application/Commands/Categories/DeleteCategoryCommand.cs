using MediatR;

namespace Catalog.Application.Commands.Categories
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public int CategoryId { get; }
        public DeleteCategoryCommand(int categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
