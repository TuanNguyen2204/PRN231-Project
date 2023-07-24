using BusinessObjects.DTOs;
using BusinessObjects.Models;
using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;

namespace eClothesClient.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient client = null;
        

        public static decimal? total = 0;

        public OrderController()
        {

            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);


        }

        public async Task<IActionResult> List()
        {
            //var mySessionValue = HttpContext.Session.GetString("user");
            //var userObject = JsonConvert.DeserializeObject<dynamic>(mySessionValue);
            //var customerId = userObject.account.customerId;
            //Get list order
            var customerId = 2;
            HttpResponseMessage ordersResponse = await client.GetAsync("https://localhost:7115/api/Order/GetOrderByUserId/" + customerId);
            string strOrders = await ordersResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<OrderListDTO>? listOrders = JsonConvert.DeserializeObject<List<OrderListDTO>>(strOrders);
           
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = new List<CartItemDTO>();

            if (!string.IsNullOrEmpty(cartJson))
            {
                cart = JsonConvert.DeserializeObject<List<CartItemDTO>>(cartJson);
            }
            total = GetTotal(cart);
            ViewData["total"] = total;
            GetCartCount();
            return View(listOrders);
        }
       
        public IActionResult Index()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = new List<CartItemDTO>();

            if (!string.IsNullOrEmpty(cartJson))
            {
                cart = JsonConvert.DeserializeObject<List<CartItemDTO>>(cartJson);
            }
            total = GetTotal(cart);
            ViewData["total"] = total;
            GetCartCount();
            return View(cart);



        }
        public async Task<ActionResult> Order(OrderCreateDTO orderCreateDto)
        {
            //Get Account/Customer form session
            //var mySessionValue = HttpContext.Session.GetString("user");

            //if (mySessionValue == null) return RedirectToAction("Index", "Cart", new { @alertMessage = "Login to order!" });

            //var userObject = JsonConvert.DeserializeObject<dynamic>(mySessionValue);
            //var customer = userObject.account.customer;

            string? cartsdeserialize = HttpContext.Session.GetString("Cart");
            List<CartItemDTO> CartItems = JsonConvert.DeserializeObject<List<CartItemDTO>>(cartsdeserialize);
            string deliveryLocation = orderCreateDto.DeliveryLocation;

            //Get list orderdetails
            List<OrderDetailsDTO> orderDetails = new List<OrderDetailsDTO>();
            decimal totalPrice = 0;
            foreach (var item in CartItems)
            {
                var od = new OrderDetailsDTO()
                {
                    ProductId = item.Product.Id,
                    SizeId = item.SizeId,
                    ColorId = item.ColorId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price

                };
                orderDetails.Add(od);
                totalPrice += od.Price;
            }
            OrderCreateDTO o = new OrderCreateDTO
            {
                UserId = 2,
                DateOrdered = DateTime.Now,
                PaymentMethod = "Cash offline",
                DeliveryLocation = deliveryLocation,
                TotalPrice = totalPrice,
                OrderDetails = orderDetails
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7115/api/Order/CreateOrder", stringContent);

            if (!response.IsSuccessStatusCode)
            {
                RedirectToAction("Index", "Cart", new { @alertMessage = "Order failed!" });
            }

            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index", "Order", new { @alertMessage = "Order successfully!" });
        }
        public IActionResult GetCartCount()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = new List<CartItemDTO>();

            if (!string.IsNullOrEmpty(cartJson))
            {
                cart = JsonConvert.DeserializeObject<List<CartItemDTO>>(cartJson);
            }

            var count = cart.Count();
            ViewData["cartcount"] = count;
            if (count == 0)
            {
                ViewData["cartcount"] = 0;
            }

            return Ok(count);
        }
        public decimal? GetTotal(List<CartItemDTO> carts)
        {
            total = 0;
            foreach (CartItemDTO cart in carts)
            {
                total += cart.Product.Price * cart.Quantity;
            }
            return total;
        }
    }
}
