using Catalog.Application.Commands.Categories;
using Catalog.Application.Queries.Categories;
using Catalog.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMediator _mediator;
        public const string CategoriesExceptionTemplate = $"Error raised in {{0}} action of {nameof(CategoriesController)}";

        public CategoriesController(ILogger<CategoriesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(OperationId = "Categories_GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var result = await _mediator.Send(new GetCategoriesQuery());

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, CategoriesExceptionTemplate, nameof(GetCategories));
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(OperationId = "Categories_AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto category)
        {
            try
            {
                var result = await _mediator.Send(new AddCategoryCommand(category));

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, CategoriesExceptionTemplate, nameof(AddCategory));
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [SwaggerOperation(OperationId = "Categories_UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto category)
        {
            try
            {
                var result = await _mediator.Send(new UpdateCategoryCommand(category));

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, CategoriesExceptionTemplate, nameof(UpdateCategory));
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [SwaggerOperation(OperationId = "Categories_DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                var result = await _mediator.Send(new DeleteCategoryCommand(categoryId));

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, CategoriesExceptionTemplate, nameof(DeleteCategory));
                return BadRequest(e.Message);
            }
        }
    }
}
