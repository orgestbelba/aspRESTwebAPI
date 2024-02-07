using AutoMapper;
using aspRESTwebAPI.Dto;
using aspRESTwebAPI.Interfaces;
using aspRESTwebAPI.Models;
using aspRESTwebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace aspRESTwebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public CustomerController(ICustomerRepository customerRepository, IMapper mapper, IProductRepository productRepository)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Gets a list of customers.
        /// </summary>
        /// <returns>The list of customers.</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
        public IActionResult GetCustomers()
        {
            var customers = _customerRepository.GetCustomers();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            //Mapped the Customer to CustomerDto so only the neccessary data are shown.
            //This logic is implemented for all the Get Requests
            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return Ok(customerDtos);
        }


        /// <summary>
        /// Gets a customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Customer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCustomer(int id)
        {
            var customer = _customerRepository.GetCustomer(id);

            if (customer == null)
                return NotFound($"Customer with ID {id} not found.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return Ok(customerDto);
        }


        /// <summary>
        /// Adds a new customer.
        /// </summary>
        /// <param name="createCustomerDto">The customer data that are going to be inserted.</param>
        /// <returns>The created customer.</returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CustomerDto))]
        [ProducesResponseType(400)]
        public IActionResult AddCustomer([FromBody] WriteCustomerDto createCustomerDto)
        {
            if (createCustomerDto == null)
            {
                return BadRequest("Object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map CreateCustomerDto to Customer
            var customerEntity = _mapper.Map<Customer>(createCustomerDto);

            _customerRepository.AddCustomer(customerEntity);

            // Map the created customer to CustomerDto
            var createdCustomerDto = _mapper.Map<CustomerDto>(customerEntity);

            // Return 201 Created with the created customer mapped to CustomerDto
            return CreatedAtAction("GetCustomer", new { id = createdCustomerDto.Id }, createdCustomerDto);
        }


        /// <summary>
        /// Update a customer
        /// </summary>
        /// <param name="id">The ID of the customer you want to update.</param>
        /// <param name="updateCustomerDto">Add all the updated values.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCustomer(int id, [FromBody] WriteCustomerDto updateCustomerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCustomer = _customerRepository.GetCustomer(id);

            if (existingCustomer == null)
                return NotFound($"Customer with ID {id} not found.");
            
            // Map UpdateCustomerDto to the existing Customer
            // This logic is implemented for all the write operations
            var updatedCustomer = _mapper.Map(updateCustomerDto, existingCustomer);
            _customerRepository.UpdateCustomer(updatedCustomer);

            return Ok();
        }


        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id">The ID of the customer you want to delete.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCustomer(int id)
        {
            if (!_customerRepository.CustomerExists(id))
            {
                return NotFound($"Customer with ID {id} does not exist.");
            }

            _customerRepository.DeleteCustomer(id);
            return Ok();
        }

       
        // ---- One-To-Many relationship method ----

        /// <summary>
        /// Gets all orders that belong to one customer
        /// </summary>
        /// <param name="customerId">The ID of the customer you want to get the orders for.</param>
        [HttpGet("{customerId}/orders")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrderDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetOrdersForCustomer(int customerId)
        {
            var orders = _customerRepository.GetOrdersForCustomer(customerId);

            if (orders == null)
                return NotFound($"There are no orders for the customer with id {customerId}.");

            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        
        // ---- Many-To-Many relationship methods ----

        /// <summary>
        /// Gets all products that belong to one customer
        /// </summary>
        /// <param name="customerId">The ID of the customer you want to get the products for.</param>
        /// <returns>The list of the products.</returns>
        [HttpGet("{customerId}/products")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        [ProducesResponseType(404)]
        public IActionResult GetProductsForCustomer(int customerId)
        {
            var products = _customerRepository.GetProductsForCustomer(customerId);

            if (products == null)
                return NotFound($"There are no product for the customer with id {customerId}.");

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);
        }

       
        /// <summary>
        /// Add a new product to the customer 
        /// </summary>
        /// <param name="customerId">The ID of the customer you want to add a product to.</param>
        /// <param name="productId">The ID of the product you want to add to the customer.</param>
        [HttpPost("{customerId}/products/{productId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddProductToCustomer(int customerId, int productId)
        {
            //Checking if the relationship exists
            //This logic is implemented for all the methods that deal with the Many-To-Many relationship 
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound($"Customer with ID {customerId} not found.");
            }

            if (!_productRepository.ProductExists(productId))
            {
                return NotFound($"Product with ID {productId} not found.");
            }

            _customerRepository.AddProductToCustomer(customerId, productId);
            return Ok();
        }

       
        /// <summary>
        /// Remove the product from the customer
        /// </summary>
        /// <param name="customerId">The ID of the customer you want to remove a product from.</param>
        /// <param name="productId">The ID of the product you want to remove from the customer.</param>
        [HttpDelete("{customerId}/products/{productId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveProductFromCustomer(int customerId, int productId)
        {
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound($"Customer with ID {customerId} not found.");
            }

            if (!_productRepository.ProductExists(productId))
            {
                return NotFound($"Product with ID {productId} not found.");
            }

            _customerRepository.RemoveProductFromCustomer(customerId, productId);
            return Ok();
        }
    }
}
