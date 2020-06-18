using System;
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
    [Route("api/orders")]
    [Authorize]
    [ApiController]
    public class OrderController : GamificationController
    {
        private readonly IOrderService _orderService;
        private readonly IValidator<CreateOrderModel> _createOrderModelValidator;
        private readonly IValidator<UpdateOrderModel> _updateOrderModelValidator;
        private readonly IValidator<PagingInfo> _pagingInfoValidator;

        public OrderController
        (
            IOrderService orderService,
            IValidator<CreateOrderModel> createOrderModelValidator,
            IValidator<UpdateOrderModel> updateOrderModelValidator,
            IValidator<PagingInfo> pagingInfoValidator
        )
        {
            _orderService = orderService;
            _createOrderModelValidator = createOrderModelValidator;
            _updateOrderModelValidator = updateOrderModelValidator;
            _pagingInfoValidator = pagingInfoValidator;
        }

        /// <summary>
        /// Get paged list of orders
        /// </summary>
        /// <responce code="200">Return the PageModel: pageNumber, pageSize and page of orders</responce>
        /// <response code="422">When the model structure is correct but validation fails</response> 
        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync([FromQuery] PagingInfo pagingInfo)
        {
            var resultValidation = await _pagingInfoValidator.ValidateAsync(pagingInfo);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

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
        /// Update order
        /// </summary>
        /// <responce code="200">Return the updated order</responce>
        /// <response code="403">When user don't have permissions to this action</response>
        /// <responce code="404">When the order does not exist</responce> 
        /// <responce code="422">When the model structure is correct but validation fails</responce> 
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrderAsync([FromForm] UpdateOrderModel model, Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var resultValidation = await _updateOrderModelValidator.ValidateAsync(model);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var item = await _orderService.UpdateOrderAsync(model, orderId);

            return Ok(item);
        }

        /// <summary>
        /// Delete order by Id
        /// </summary>
        /// <responce code="204">When the order successful delete</responce>
        /// <response code="403">When user don't have permissions to this action</response>
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
