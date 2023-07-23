using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Models;
using BusinessObjects.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repositories.Interfaces;

namespace eClothesAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private IMapper _mapper;

        public OrderController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetOrders([FromQuery] OrderParameters OrderParameters)
        {
            try
            {
                var Orders = _repository.Order.GetOrders(OrderParameters);
                var ordersResult = _mapper.Map<IEnumerable<OrderDTO>>(Orders);
                _logger.LogInfo($"Returned all Orders from database.");
                var metadata = new
                {
                    Orders.TotalCount,
                    Orders.PageSize,
                    Orders.CurrentPage,
                    Orders.TotalPages,
                    Orders.HasNext,
                    Orders.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(ordersResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOrders action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]

        public IActionResult GetOrderById(int id)
        {
            try
            {
                var order = _repository.Order.GetOrderDetails(id);
                if (order == null)
                {
                    _logger.LogError($"Order with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned order with details for id: {id}");

                    var orderResult = _mapper.Map<OrderDTO>(order);
                    return Ok(orderResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside orderResult action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] OrderCreateUpdateDTO OrderDto)
        {
            try
            {
                if (OrderDto is null)
                {
                    _logger.LogError("Order object sent from client is null.");
                    return BadRequest("Order object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Order object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var OrderEntity = _repository.Order.FindByCondition(x => x.Id == id).FirstOrDefault();
                if (OrderEntity is null)
                {
                    _logger.LogError($"Order with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                _mapper.Map(OrderDto, OrderEntity);
                _repository.Order.UpdateOrder(OrderEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateOrder action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
