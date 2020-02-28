using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.Order;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    using System;

    [Route("api/orders")]
    [Authorize]
    [ApiController]
    public class OrderController : GamificationController
    {
        private readonly IOrderService _orderService;
        private readonly IValidator<CreateOrderModel> _createOrderModelValidator;

        public OrderController
        (
            IOrderService orderService,
            IValidator<CreateOrderModel> createOrderModelValidator
        )
        {
            _orderService = orderService;
            _createOrderModelValidator = createOrderModelValidator;
        }

        /// <summary>
        /// Get paged list of orders
        /// </summary>
        /// <responce code="200">Return the PageModel: pageNumber, pageSize and page of orders</responce> 
        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync([FromQuery] PagingInfo pagingInfo)
        {
            var list = await _orderService.GetAllOrderAsync(pagingInfo);

            return Ok(list);
        }

        /// <summary>
        /// Get order by Id
        /// </summary>
        /// <responce code="200">Return some order</responce> 
        /// <responce code="404">When order does not exist</responce> 
        [HttpGet("{orderId}", Name = "GetOrder")]
        public async Task<IActionResult> GetOrderByIdAsync(Guid orderId)
        {
            var item = await _orderService.GetOrderByIdAsync(orderId);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <responce code="201">Return created order</responce> 
        /// <response code="403">When user don't have permissions to this action</response>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddOrderAsync([FromForm] CreateOrderModel model)
        {
            var resultValidation = await _createOrderModelValidator.ValidateAsync(model);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var order = await _orderService.AddOrderAsync(model);

            return CreatedAtRoute(
                "GetOrder",
                new { orderId = order.Id },
                order);
        }

        /// <summary>
        /// Delete order by Id
        /// </summary>
        /// <responce code="204">When the order successful delete</responce>
        /// <response code="404">When the order does not exist</response>
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrderAsync(Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            await _orderService.DeleteOrderAsync(orderId);

            return NoContent();
        }
    }
}
