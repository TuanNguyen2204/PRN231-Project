using BusinessObjects.DTOs;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace eClothesClient.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";


        public ProductController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7115/api/Products/GetProducts";

        }
        public async Task<IActionResult> Index(int? categoryId, string productName, int pageNumber = 1, int pageSize = 9)
        {
            var apiUrl = ProductApiUrl + $"?&PageNumber={pageNumber}&PageSize={pageSize}";

            if (categoryId.HasValue)
            {
                apiUrl += $"&CatId={categoryId}";
            }

            if (!string.IsNullOrEmpty(productName))
            {
                apiUrl += $"&ProductName={productName}";
            }
            ViewBag.productName = productName;
            ViewBag.categoryId = categoryId;

            var response = await client.GetAsync(apiUrl);
            var categoryApiUrl = "https://localhost:7115/api/Category/GetCategories";
            var categoryResponse = await client.GetAsync(categoryApiUrl);
            if (categoryResponse.IsSuccessStatusCode)
            {
                var categoryResponseContent = await categoryResponse.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<IEnumerable<CategoryDTO>>(categoryResponseContent);
                ViewBag.Categories = categories;
            }

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


        public async Task<IActionResult> Details(int productId)
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7115/api/Products/GetProductDetail/" + productId);
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
         

            ProductDTO product = JsonConvert.DeserializeObject<ProductDTO>(strData);

            return View(product);
        }

    }
}
