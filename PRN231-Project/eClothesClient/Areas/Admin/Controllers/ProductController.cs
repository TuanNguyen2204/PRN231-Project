using BusinessObjects.DTOs;
using BusinessObjects.Models;
using BusinessObjects.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace eClothesClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnv;
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";
        private string CategoryApiUrl = "";

        public ProductController(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            this.hostingEnv = env;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7115/api/Products/GetProducts";
            CategoryApiUrl = "https://localhost:7115/api/Category/GetCategories";
        }

        public async Task<IActionResult> Index(int? CatId, string? ProductName, int PageNumber = 1, int PageSize = 1)
        {

            var apiUrl = ProductApiUrl + $"?&PageNumber={PageNumber}&PageSize={PageSize}";

            if (CatId.HasValue)
            {
                apiUrl += $"&CatId={CatId}";
            }

            if (!string.IsNullOrEmpty(ProductName))
            {
                apiUrl += $"&ProductName={ProductName}";
            }
            var response = await client.GetAsync(apiUrl);
            var categoryResponse = await client.GetAsync(CategoryApiUrl);
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
        public async Task<ActionResult> Create()
        {

            List<CategoryDTO> listCategories = await GetCategoriesAsync();
            ViewData["CategoryId"] = new SelectList(listCategories, "CategoryId", "CategoryName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductCreateUpdateDTO productDTO, IFormFile? imgFile)
        {
            if (imgFile != null)
            {
                string fileName = imgFile.FileName;
                string fileDir = "imgProduct";
                string filePath = Path.Combine(hostingEnv.WebRootPath, fileDir + "\\" + fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    imgFile.CopyTo(fs);
                }
                productDTO.Image = "../imgProduct/" + imgFile.FileName;
            }
            else
            {
                productDTO.Image = "../imgProduct/default.jpg";
            }

            var stringContent = new StringContent(JsonConvert.SerializeObject(productDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7115/api/Products/CreateProduct", stringContent);

            List<CategoryDTO> listCategories = await GetCategoriesAsync();
            ViewData["CategoryId"] = new SelectList(listCategories, "CategoryId", "CategoryDetails");

            if (response.IsSuccessStatusCode)
            {
                ViewData["Message"] = "Create successfully!";
                return View(productDTO);
            }

            ViewData["Message"] = "Create fail, try again!";
            return View(productDTO);
        }
        private async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            //Get Categories
            HttpResponseMessage categoriesResponse = await client.GetAsync(CategoryApiUrl);
            string strCategories = await categoriesResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<CategoryDTO>>(strCategories);
        }
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage productResponse = await client.GetAsync("https://localhost:7115/api/Products/GetProductDetail" + "/" + id);
            string strProduct = await productResponse.Content.ReadAsStringAsync();
            ProductCreateUpdateDTO? productDTO = JsonConvert.DeserializeObject<ProductCreateUpdateDTO>(strProduct);
            List<CategoryDTO> listCategories = await GetCategoriesAsync();
            ViewData["CategoryId"] = new SelectList(listCategories, "CategoryId", "CategoryName");

            return View(productDTO);
        }
        


    }
}
