using AutoMapper;
using aspRESTwebAPI.Dto;
using aspRESTwebAPI.Interfaces;
using aspRESTwebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace aspRESTwebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

       
        /// <summary>
        /// Gets a list of Orders.
        /// </summary>
        /// <returns>The list of Orders.</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        public IActionResult GetOrders()
        {
            var orders = _orderRepository.GetOrders();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        
        /// <summary>
        /// Gets an Order by ID.
        /// </summary>
        /// <param name="id">The ID of the Order.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(400)]
        public IActionResult GetOrder(int id)
        {
            var order = _orderRepository.GetOrder(id);

            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

       
        /// <summary>
        /// Adds a new Order.
        /// </summary>
        /// <param name="orderDto">The Order data that are going to be inserted.</param>
        /// <returns>The created Order.</returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(OrderDto))]
        [ProducesResponseType(400)]
        public IActionResult AddOrder([FromBody] WriteOrderDto orderDto)
        {
            if (orderDto == null)
                return BadRequest("Object is null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderEntity = _mapper.Map<Order>(orderDto);

            _orderRepository.AddOrder(orderEntity);

            var createdOrderDto = _mapper.Map<OrderDto>(orderEntity);

            return CreatedAtAction("GetOrder", new { id = createdOrderDto.OrderId }, createdOrderDto);
        }

        
        /// <summary>
        /// Update an Order
        /// </summary>
        /// <param name="id">The ID of the Order you want to update.</param>
        /// <param name="orderDto">Add all the updated values.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOrder(int id, [FromBody] WriteOrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingOrder = _orderRepository.GetOrder(id);

            if (existingOrder == null)
                return NotFound($"Order with ID {id} not found.");

            var updatedOrder = _mapper.Map(orderDto, existingOrder);
            _orderRepository.UpdateOrder(updatedOrder);
            return Ok();
        }


        /// <summary>
        /// Deletes an Order
        /// </summary>
        /// <param name="id">The ID of the Order you want to delete.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOrder(int id)
        {
            var existingOrder = _orderRepository.GetOrder(id);

            if (existingOrder == null)
                return NotFound($"Order with ID {id} does not exist.");

            _orderRepository.DeleteOrder(id);
            return Ok();
        }
    }
}
