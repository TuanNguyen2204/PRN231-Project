using BusinessObjects.DTOs;
using BusinessObjects.Models;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;
using eClothesAPI.Config;
using eClothesClient.Middleware;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;

namespace eClothesClient.Controllers
{
    [RedirectUnauthenticated]
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

        public async Task<IActionResult> List(string? alertMessage)
        {

            string accessToken = HttpContext.Request.Cookies["access_token"];
            var tokenHandler = new JwtSecurityTokenHandler();
            string userId = "";
            // Try to parse the token
            if (tokenHandler.CanReadToken(accessToken))
            {
                // Decode the token
                var jwtToken = tokenHandler.ReadJwtToken(accessToken);

                // Access the claims within the token
                var claims = jwtToken.Claims;

                // Now you can access specific claim values by their names
                userId = claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            }
            var customerId = userId;
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
            ViewData["alertMessage"] = alertMessage;
            return View(listOrders);
        }
       
        public async Task<IActionResult> Index()
        {
            string accessToken = HttpContext.Request.Cookies["access_token"];
            var tokenHandler = new JwtSecurityTokenHandler();
            string userId = "";
            // Try to parse the token
            if (tokenHandler.CanReadToken(accessToken))
            {
                // Decode the token
                var jwtToken = tokenHandler.ReadJwtToken(accessToken);

                // Access the claims within the token
                var claims = jwtToken.Claims;

                // Now you can access specific claim values by their names
                userId = claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            }
            var customerId = userId;
            HttpResponseMessage userResponse = await client.GetAsync("https://localhost:7115/api/User/GetUserDetail/" + customerId);
            string strUser = await userResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            UserDTO user = JsonConvert.DeserializeObject<UserDTO>(strUser);
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = new List<CartItemDTO>();

            if (!string.IsNullOrEmpty(cartJson))
            {
                cart = JsonConvert.DeserializeObject<List<CartItemDTO>>(cartJson);
            }
            total = GetTotal(cart);
            ViewData["total"] = total;
            ViewData["user"] = user;
            GetCartCount();
            return View(cart);



        }
        public async Task<ActionResult> Order(OrderCreateDTO orderCreateDto)
        {
            string accessToken = HttpContext.Request.Cookies["access_token"];
            var tokenHandler = new JwtSecurityTokenHandler();
            string userId = "";
            // Try to parse the token
            if (tokenHandler.CanReadToken(accessToken))
            {
                // Decode the token
                var jwtToken = tokenHandler.ReadJwtToken(accessToken);

                // Access the claims within the token
                var claims = jwtToken.Claims;

                // Now you can access specific claim values by their names
                userId = claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            }
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
            int totalQuantity = 0;
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
                totalPrice += od.Price * od.Quantity;
                totalQuantity += od.Quantity;
            }
            OrderCreateDTO o = new OrderCreateDTO
            {
                UserId = Int32.Parse(userId),
                DateOrdered = DateTime.Now,
                PaymentMethod = "Cash offline",
                DeliveryLocation = deliveryLocation,
                TotalPrice = totalPrice,
                OrderDetails = orderDetails,
                Quantity= totalQuantity,
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7115/api/Order/CreateOrder", stringContent);

            if (!response.IsSuccessStatusCode)
            {
                RedirectToAction("Index", "Order", new { @alertMessage = "Order failed!" });
            }
            
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("List", "Order", new { @alertMessage = "Order successfully!" });
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
