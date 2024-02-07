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
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;

        public ProductController(IProductRepository productRepository, IMapper mapper, ICustomerRepository customerRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Gets a list of Products.
        /// </summary>
        /// <returns>The list of Products.</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);
        }


        /// <summary>
        /// Gets a Product by ID.
        /// </summary>
        /// <param name="id">The ID of the Product.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetProduct(int id)
        {
            var product = _productRepository.GetProduct(id);

            if (product == null)
                return NotFound($"Product with ID {id} not found.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }


        /// <summary>
        /// Adds a new Product.
        /// </summary>
        /// <param name="productDto">The product data that are going to be inserted.</param>
        /// <returns>The created Product.</returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]
        public IActionResult AddProduct([FromBody] WriteProductDto productDto)
        {
            if (productDto == null)
                return BadRequest("Object is null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productEntity = _mapper.Map<Product>(productDto);

            _productRepository.AddProduct(productEntity);

            var createdProductDto = _mapper.Map<ProductDto>(productEntity);

            return CreatedAtAction("GetProduct", new { id = createdProductDto.ProductId }, createdProductDto);
        }


        /// <summary>
        /// Updates a Product
        /// </summary>
        /// <param name="id">The ID of the product you want to update.</param>
        /// <param name="productDto">Add all the updated values.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int id, [FromBody] WriteProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProduct = _productRepository.GetProduct(id);

            if (existingProduct == null)
                return NotFound($"Product with ID {id} not found.");

            var updatedProduct = _mapper.Map(productDto, existingProduct);
            _productRepository.UpdateProduct(updatedProduct);
            return Ok();
        }


        /// <summary>
        /// Deletes a Product
        /// </summary>
        /// <param name="id">The ID of the product you want to delete.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(int id)
        {
            var existingProduct = _productRepository.GetProduct(id);

            if (existingProduct == null)
                return NotFound($"Product with ID {id} does not exist.");

            _productRepository.DeleteProduct(id);
            return Ok();
        }

        // Many-To-Many relationship methods. *These methods are kind of the reversed of the ones in the Customer Controller.

        /// <summary>
        /// Gets all customers that have the same product.
        /// </summary>
        /// <param name="productId">The ID of the product you want to get the customers for.</param>
        /// <returns>The list of the customers.</returns>
        [HttpGet("{productId}/customers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomersForProduct(int productId)
        {
            var customers = _productRepository.GetCustomersForProduct(productId);

            if (customers == null)
                return NotFound($"There are no customer for the product id {productId}.");

            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return Ok(customerDtos);
        }


        /// <summary>
        /// Adds a customer to the product. 
        /// </summary>
        /// <param name="customerId">The ID of the customer you want to add the product to.</param>
        /// <param name="productId">The ID of the product you want to add to the customer.</param>
        [HttpPost("{productId}/customers/{customerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddCustomerToProduct(int productId, int customerId)
        {
            if (!_productRepository.ProductExists(productId))
            {
                return NotFound($"Product with ID {productId} not found.");
            }

            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound($"Customer with ID {customerId} not found.");
            }

            _productRepository.AddCustomerToProduct(productId, customerId);
            return Ok();
        }


        /// <summary>
        /// Removes a customer from the product
        /// </summary>
        /// <param name="customerId">The ID of the customer you want to remove the product from.</param>
        /// <param name="productId">The ID of the product you want to remove from the customer.</param>
        [HttpDelete("{productId}/customers/{customerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveCustomerFromProduct(int productId, int customerId)
        {
            if (!_productRepository.ProductExists(productId))
            {
                return NotFound($"Product with ID {productId} not found.");
            }

            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound($"Customer with ID {customerId} not found.");
            }

            _productRepository.RemoveCustomerFromProduct(productId, customerId);
            return Ok();
        }
    }

}
