using BusinessObjects.DTOs;
using BusinessObjects.Models;
using BusinessObjects.QueryParameters;
using ClosedXML.Excel;
using eClothesClient.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace eClothesClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnv;
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";
        private string CategoryApiUrl = "";

        public ProductsController(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            this.hostingEnv = env;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7115/api/Products/GetProducts";
            CategoryApiUrl = "https://localhost:7115/api/Category/GetCategories";
        }

        public async Task<IActionResult> Index(int? CatId, string? ProductName, string? orderby, int PageNumber = 1, int PageSize = 10)
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
            if (!string.IsNullOrEmpty(orderby))
            {
                apiUrl += $"&OrderBy={orderby}";
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
                string fileDir = "imagesProduct";
                string filePath = Path.Combine(hostingEnv.WebRootPath, fileDir + "\\" + fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    imgFile.CopyTo(fs);
                }
                productDTO.Image = "../imagesProduct/" + imgFile.FileName;
            }
            else
            {
                productDTO.Image = "../imagesProduct/default.png";
            }

            var stringContent = new StringContent(JsonConvert.SerializeObject(productDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7115/api/Products/CreateProduct", stringContent);

            List<CategoryDTO> listCategories = await GetCategoriesAsync();
            ViewData["CategoryId"] = new SelectList(listCategories, "CategoryId", "CategoryName");

            if (response.IsSuccessStatusCode)
            {
                ViewData["Message"] = "Create successfully!";
                return View(productDTO);
            }

            ViewData["Message"] = "Create fail, try again!";
            return View(productDTO);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductCreateUpdateDTO productDTO, IFormFile? imgFile)
        {
            if (imgFile == null) productDTO.Image = "" + productDTO.Image;
            else productDTO.Image = "../imagesProduct/" + imgFile.FileName;
            var stringContent = new StringContent(JsonConvert.SerializeObject(productDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("https://localhost:7115/api/Products/UpdateProduct/" + productDTO.Id, stringContent);

            List<CategoryDTO> listCategories = await GetCategoriesAsync();
            ViewData["CategoryId"] = new SelectList(listCategories, "CategoryId", "CategoryName");

            if (response.IsSuccessStatusCode)
            {
                if (imgFile != null)
                {
                    string fileName = imgFile.FileName;
                    string fileDir = "imagesProduct";
                    string filePath = Path.Combine(hostingEnv.WebRootPath, fileDir + "\\" + fileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        imgFile.CopyTo(fs);
                    }

                }
                ViewData["Message"] = "Edit successfully!";
                return View(productDTO);
            }

            ViewData["Message"] = "Edit fail, try again!";
            return View(productDTO);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            await client.DeleteAsync("https://localhost:7115/api/Products/DeleteProduct" + "/" + id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportExcel(int? CatId, string? ProductName, string? orderby)
        {

            var apiUrl = "https://localhost:7115/api/Products/ExportExcel" + $"?";

            if (CatId.HasValue)
            {
                apiUrl += $"&CatId={CatId}";
            }

            if (!string.IsNullOrEmpty(ProductName))
            {
                apiUrl += $"&ProductName={ProductName}";
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                apiUrl += $"&OrderBy={orderby}";
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

                using (var workbook = new XLWorkbook())
                {
                    ExcelConfiguration.exportProduct(products.ToList(), workbook);

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "products.xlsx");
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
        private async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            //Get Categories
            HttpResponseMessage categoriesResponse = await client.GetAsync(CategoryApiUrl);
            string strCategories = await categoriesResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<CategoryDTO>>(strCategories);
        }
    }
}
