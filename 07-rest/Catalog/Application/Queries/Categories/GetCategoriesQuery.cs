using Catalog.Models;
using MediatR;

namespace Catalog.Application.Queries.Categories
{
    public class GetCategoriesQuery : IRequest<List<CategoryDto>> { }
}
