using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Models;
using BusinessObjects.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace eClothesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private IMapper _mapper;

        public ProductsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetProducts([FromQuery] ProductParameters productParameters)
        {
            try
            {
                var products = _repository.Product.GetProducts(productParameters);
                _logger.LogInfo($"Returned all products from database.");
                var productsResult = _mapper.Map<IEnumerable<ProductDTO>>(products);
                return Ok(productsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProductDetail(int id) {
            try
            {
                var product = _repository.Product.GetProductDetails(id);
                if (product == null)
                {
                    _logger.LogError($"Product with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned owner with details for id: {id}");

                    var productResult = _mapper.Map<ProductDTO>(product);
                    return Ok(productResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerWithDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductCreateUpdateDTO product) {
            try
            {
                if(product is null)
                {
                    _logger.LogError("Product object sent from client is null.");
                    return BadRequest("Product object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid product object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var prductEntity = _mapper.Map<Product>(product);
                _repository.Product.CreateProduct(prductEntity);
                _repository.Save();
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateProduct action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("id")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductCreateUpdateDTO productDto)
        {
            try
            {
                if(productDto is null)
                {
                    _logger.LogError("Product object sent from client is null.");
                    return BadRequest("Product object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid product object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var productEntity = _repository.Product.GetProductDetails(id);
                if (productEntity is null)
                {
                    _logger.LogError($"Product with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                _mapper.Map(productDto, productEntity);
                _repository.Product.UpdateProduct(productEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateProduct action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var product = _repository.Product.GetProductDetails(id);
                if (product == null)
                {
                    _logger.LogError($"Product with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                _repository.Product.DeleteProduct(product);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteProduct action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
