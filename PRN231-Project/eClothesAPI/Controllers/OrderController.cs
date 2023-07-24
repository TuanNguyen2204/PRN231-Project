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
        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderCreateDTO order)
        {
            try
            {
                if (order is null)
                {
                    _logger.LogError("Order object sent from client is null.");
                    return BadRequest("Order object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid order object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var orderEntity = _mapper.Map<Order>(order);

                //// Create a new list to store unique OrderDetail entities
                //var orderDetails = new List<OrderDetail>();

                //// Loop through the order details in the request and process them
                //foreach (var orderDetailDTO in order.OrderDetails)
                //{
                //    var orderDetailEntity = _mapper.Map<OrderDetail>(orderDetailDTO);

                //    // Check if an OrderDetail with the same ProductId and OrderId already exists
                //    // in the list, and if so, update its Quantity instead of adding a new one
                //    var existingOrderDetail = orderDetails.FirstOrDefault(od =>
                //        od.ProductId == orderDetailEntity.ProductId &&
                //        od.OrderId == orderDetailEntity.OrderId &&
                //        od.SizeId == orderDetailEntity.SizeId &&
                //        od.ColorId == orderDetailEntity.ColorId
                //    );

                //    if (existingOrderDetail != null)
                //    {
                //        existingOrderDetail.Quantity += orderDetailEntity.Quantity;
                //    }
                //    else
                //    {
                //        orderDetails.Add(orderDetailEntity);
                //    }
                //}

                //// Set the order details for the order entity
                //orderEntity.OrderDetails = orderDetails;

                // Create the order in the database
                _repository.Order.CreateOrder(orderEntity);

                _repository.Save();
                return Ok(orderEntity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public IActionResult CreateOrder([FromBody] OrderCreateDTO order)
        //{
        //    try
        //    {
        //        if (order is null)
        //        {
        //            _logger.LogError("Order object sent from client is null.");
        //            return BadRequest("Order object is null");
        //        }
        //        if (!ModelState.IsValid)
        //        {
        //            _logger.LogError("Invalid order object sent from client.");
        //            return BadRequest("Invalid model object");
        //        }
        //        //var odEntity = _mapper.Map<Order>(order);




        //        //_repository.Order.CreateOrder(odEntity);

        //        //_repository.Save();
        //        // Create the Order entity and set its properties
        //        var orderEntity = new Order
        //        {
        //            UserId = order.UserId,
        //            DateOrdered = order.DateOrdered,
        //            PaymentMethod = order.PaymentMethod,
        //            DeliveryLocation = order.DeliveryLocation,
        //            TotalPrice = order.TotalPrice
        //        };

        //        // Add the Order entity to the repository and save changes to generate the OrderId
        //        _repository.Order.CreateOrder(orderEntity);
        //        _repository.Save();
        //        foreach (var orderDetailDTO in order.OrderDetails)
        //        {
        //            // Create the OrderDetail entity and set its properties
        //            var orderDetailEntity = new OrderDetail
        //            {
        //                OrderId = orderEntity.Id, // Use the generated OrderId
        //                ProductId = orderDetailDTO.ProductId,
        //                SizeId = orderDetailDTO.SizeId,
        //                ColorId = orderDetailDTO.ColorId,
        //                Quantity = orderDetailDTO.Quantity,
        //                Price = orderDetailDTO.Price
        //            };

        //            // Add the OrderDetail entity to the repository
        //            _repository.OrderDetail.CreateOrderDetail(orderDetailEntity);
        //        }


        //        // Save changes to the repository to add the OrderDetails to the order
        //        _repository.Save();
        //        return Ok(orderEntity);
        //    }
        //    catch (Exception ex)
        //    {
        //        //_logger.LogError($"Something went wrong inside CreateProduct action: {ex.Message}");
        //        //return StatusCode(500, "Internal server error");
        //        throw new Exception(ex.Message);
        //    }
        //}

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
