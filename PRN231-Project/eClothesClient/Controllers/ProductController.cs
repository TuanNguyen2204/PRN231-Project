using BusinessObjects.DTOs;
using BusinessObjects.Models;
using eClothesClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

namespace eClothesClient.Controllers
{
    public class ProductController : Controller
    {
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 2)
        {
            var apiUrl = $"https://localhost:7115/api/Products/GetProducts?&PageNumber={pageNumber}&PageSize={pageSize}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(responseContent);

                    var paginationHeader = response.Headers.GetValues("X-Pagination").FirstOrDefault();
                    var paginationMetadata = JsonConvert.DeserializeObject<PaginationMetadata>(paginationHeader);

                    ViewBag.Products = products;
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

        }
    }
}
