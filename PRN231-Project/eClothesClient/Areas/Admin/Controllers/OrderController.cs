using BusinessObjects.DTOs;
using BusinessObjects.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using eClothesClient.Configuration;
using eClothesClient.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace eClothesClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RedirectUnauthenticated]
    public class OrderController : Controller
    {
        private readonly HttpClient client = null;
        private string OrderApi = "";
        private string UserApi = "";

        public OrderController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApi = "https://localhost:7115/api/Order/GetOrders";
            UserApi = "https://localhost:7115/api/User/GetUsers";
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, int PageNumber = 1, int PageSize = 5)
        {
            var uriBuilder = new UriBuilder(OrderApi);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["PageNumber"] = PageNumber.ToString();
            query["PageSize"] = PageSize.ToString();

            if (startDate.HasValue)
            {
                query["startDate"] = startDate.Value.ToString("yyyy-MM-dd");
            }

            if (endDate.HasValue)
            {
                query["endDate"] = endDate.Value.ToString("yyyy-MM-dd");
            }

            uriBuilder.Query = query.ToString();
            var apiUrl = uriBuilder.ToString();

            var response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseContent);

                var paginationHeader = response.Headers.GetValues("X-Pagination").FirstOrDefault();
                var paginationMetadata = JsonConvert.DeserializeObject<PaginationMetadata>(paginationHeader);

                ViewBag.Orders = orders;
                ViewBag.PaginationMetadata = paginationMetadata;

                return View();
            }
            else
            {
                // Handle the API error response
                var errorResponse = await response.Content.ReadAsStringAsync();
                return View("Error");
            }
        }
        public async Task<IActionResult> Details(int id)
        {
            var apiUrl = "https://localhost:7115/api/OrderDetail/GetOrderDetails/" + $"{id}";
            var response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var orderDetails = JsonConvert.DeserializeObject<IEnumerable<OrderDetailDTO>>(responseContent);

                ViewBag.OrderDetails = orderDetails;
                return View();
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                return View("Error");
            }

        }
        public async Task<IActionResult> Edit(int id)
        {
            HttpResponseMessage orderResponse = await client.GetAsync($"https://localhost:7115/api/Order/GetOrderById/{id}");
            string strOrder = await orderResponse.Content.ReadAsStringAsync();
            OrderDTO? orderDTO = JsonConvert.DeserializeObject<OrderDTO>(strOrder);
            return View(orderDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int Id, int Quantity, DateTime DateOrdered, string PaymentMethod, string DeliveryLocation, decimal TotalPrice)
        {
            var orderDTO = new OrderCreateUpdateDTO();
            var apiUrl = "https://localhost:7115/api/Order/GetOrderById/" + $"{Id}";
            var response = await client.GetAsync(apiUrl);
            var responseContent = await response.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject<OrderDTO>(responseContent);
            orderDTO.Id = Id;
            orderDTO.UserId = order.UserId;
            orderDTO.PaymentMethod = PaymentMethod;
            orderDTO.Quantity = Quantity;
            orderDTO.DateOrdered = DateOrdered;
            orderDTO.TotalPrice = TotalPrice;
            orderDTO.DeliveryLocation = DeliveryLocation;
            var stringContent = new StringContent(JsonConvert.SerializeObject(orderDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response2 = await client.PutAsync("https://localhost:7115/api/Order/UpdateOrder/" + orderDTO.Id, stringContent);
            order.DateOrdered = DateOrdered;
            order.DeliveryLocation = DeliveryLocation;
            if (response2.IsSuccessStatusCode)
            {
                ViewData["Message"] = "Edit successfully!";
                return View(order);
            }

            ViewData["Message"] = "Edit fail, try again!";
            return View(order);
        }


        public async Task<IActionResult> ExportExcel(DateTime? startDate, DateTime? endDate)
        {

            var apiUrl = "https://localhost:7115/api/Order/GetOrders" + $"?";

            if (startDate.HasValue)
            {
                apiUrl += $"&startDate={startDate}";
            }
            if (startDate.HasValue)
            {
                apiUrl += $"&endDate={endDate}";
            }
            var response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseContent);

                var paginationHeader = response.Headers.GetValues("X-Pagination").FirstOrDefault();
                var paginationMetadata = JsonConvert.DeserializeObject<PaginationMetadata>(paginationHeader);

                ViewBag.Orders = orders;
                ViewBag.PaginationMetadata = paginationMetadata;

                using (var workbook = new XLWorkbook())
                {
                    ExcelConfiguration.exportOrder(orders.ToList(), workbook);

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "orders.xlsx");
                    }
                }

                return View("Index");
            }

            else
            {
                // Handle the API error response
                var errorResponse = await response.Content.ReadAsStringAsync();
                return View("Error");
            }
        }
    }
}
