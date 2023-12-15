using Catalog.Application.Commands.Items;
using Catalog.Application.Queries.Items;
using Catalog.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : Controller
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly IMediator _mediator;
        public const string ItemsExceptionTemplate = $"Error raised in {{0}} action of {nameof(ItemsController)}";

        public ItemsController(ILogger<ItemsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{categoryId}/items")]
        [SwaggerOperation(OperationId = "Items_GetItems")]
        public async Task<IActionResult> GetItems([FromRoute] int categoryId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var result = await _mediator.Send(new GetItemsQuery()
                {
                    CategoryId = categoryId,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, ItemsExceptionTemplate, nameof(GetItems));
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(OperationId = "Items_AddItem")]
        public async Task<IActionResult> AddItem([FromBody] ItemDto item)
        {
            try
            {
                var result = await _mediator.Send(new AddItemCommand(item));

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, ItemsExceptionTemplate, nameof(AddItem));
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [SwaggerOperation(OperationId = "Items_UpdateItem")]
        public async Task<IActionResult> UpdateItem([FromBody] ItemDto item)
        {
            try
            {
                var result = await _mediator.Send(new UpdateItemCommand(item));

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, ItemsExceptionTemplate, nameof(UpdateItem));
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{itemId}")]
        [SwaggerOperation(OperationId = "Items_DeleteItem")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            try
            {
                var result = await _mediator.Send(new DeleteItemCommand(itemId));

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, ItemsExceptionTemplate, nameof(DeleteItem));
                return BadRequest(e.Message);
            }
        }
    }
}
