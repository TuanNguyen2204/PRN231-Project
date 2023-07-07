using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repositories.Interfaces;

namespace eClothesAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private IMapper _mapper;

        public OrderDetailController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet("{orderID}")]
        public IActionResult GetOrderDetails(int orderID)
        {
            try
            {
                var orderDetails = _repository.OrderDetail.GetOrderDetails(orderID);
                var orderDetailsResult = _mapper.Map<IEnumerable<OrderDetailDTO>>(orderDetails);
                return Ok(orderDetailsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOrderDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
