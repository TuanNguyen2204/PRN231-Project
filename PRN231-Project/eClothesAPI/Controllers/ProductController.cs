using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace eClothesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public ProductController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _repository.Product.GetAllProducts();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product =  _repository.Product.FindProductById(id);
            return Ok(product);
        }

    }
}
