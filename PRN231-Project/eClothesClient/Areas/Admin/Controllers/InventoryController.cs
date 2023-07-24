using BusinessObjects.DTOs;
using BusinessObjects.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using eClothesClient.Configuration;
using eClothesClient.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace eClothesClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RedirectUnauthenticated]
    public class InventoryController : Controller
    {
        private readonly HttpClient client = null;
        private string InventoryApiUrl = "";
        private string CategoryApiUrl = "";
        private string ColorApiUrl = "";
        private string SizeApiUrl = "";
        private string ProductApiUrl = "";

        public InventoryController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            InventoryApiUrl = "https://localhost:7115/api/Inventory/GetInventories";
            CategoryApiUrl = "https://localhost:7115/api/Category/GetCategories";
            ColorApiUrl = "https://localhost:7115/api/Color";
            SizeApiUrl = "https://localhost:7115/api/Size";
            ProductApiUrl = "https://localhost:7115/api/Products/GetProducts";
        }

        public async Task<IActionResult> Index(int? CatId, int? ColorId, int? SizeId, string? ProductName,  int PageNumber = 1, int PageSize = 10)
        {
            var apiUrl = InventoryApiUrl + $"?&PageNumber={PageNumber}&PageSize={PageSize}";

            if (CatId.HasValue)
            {
                apiUrl += $"&CatId={CatId}";
            }
            if (ColorId.HasValue)
            {
                apiUrl += $"&ColorId={ColorId}";
            }
            if (SizeId.HasValue)
            {
                apiUrl += $"&SizeId={SizeId}";
            }
            if (!string.IsNullOrEmpty(ProductName))
            {
                apiUrl += $"&ProductName={ProductName}";
            }
            var categoryResponse = await client.GetAsync(CategoryApiUrl);
            if (categoryResponse.IsSuccessStatusCode)
            {
                var categoryResponseContent = await categoryResponse.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<IEnumerable<CategoryDTO>>(categoryResponseContent);
                ViewBag.Categories = categories;
            }

            var colorResponse = await client.GetAsync(ColorApiUrl);
            if (colorResponse.IsSuccessStatusCode)
            {
                var colorResponseContent = await colorResponse.Content.ReadAsStringAsync();
                var colors = JsonConvert.DeserializeObject<IEnumerable<ColorDTO>>(colorResponseContent);
                ViewBag.Colors = colors;
            }
            var sizeResponse = await client.GetAsync(SizeApiUrl);
            if (sizeResponse.IsSuccessStatusCode)
            {
                var sizeResponseContent = await sizeResponse.Content.ReadAsStringAsync();
                var sizes = JsonConvert.DeserializeObject<IEnumerable<SizeDTO>>(sizeResponseContent);
                ViewBag.Sizes = sizes;
            }
            var response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var inventories = JsonConvert.DeserializeObject<IEnumerable<InventoryDTO>>(responseContent);

                var paginationHeader = response.Headers.GetValues("X-Pagination").FirstOrDefault();
                var paginationMetadata = JsonConvert.DeserializeObject<PaginationMetadata>(paginationHeader);

                ViewBag.Inventories = inventories;
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
        public async Task<IActionResult> ExportExcel(int? CatId, int? ColorId, int? SizeId, string? ProductName)
        {
            var apiUrl = "https://localhost:7115/api/Inventory/ExportExcel" + $"?";

            if (CatId.HasValue)
            {
                apiUrl += $"&CatId={CatId}";
            }
            if (ColorId.HasValue)
            {
                apiUrl += $"&ColorId={ColorId}";
            }
            if (SizeId.HasValue)
            {
                apiUrl += $"&SizeId={SizeId}";
            }
            if (!string.IsNullOrEmpty(ProductName))
            {
                apiUrl += $"&ProductName={ProductName}";
            }
            var categoryResponse = await client.GetAsync(CategoryApiUrl);
            if (categoryResponse.IsSuccessStatusCode)
            {
                var categoryResponseContent = await categoryResponse.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<IEnumerable<CategoryDTO>>(categoryResponseContent);
                ViewBag.Categories = categories;
            }

            var colorResponse = await client.GetAsync(ColorApiUrl);
            if (colorResponse.IsSuccessStatusCode)
            {
                var colorResponseContent = await colorResponse.Content.ReadAsStringAsync();
                var colors = JsonConvert.DeserializeObject<IEnumerable<ColorDTO>>(colorResponseContent);
                ViewBag.Colors = colors;
            }
            var sizeResponse = await client.GetAsync(SizeApiUrl);
            if (sizeResponse.IsSuccessStatusCode)
            {
                var sizeResponseContent = await sizeResponse.Content.ReadAsStringAsync();
                var sizes = JsonConvert.DeserializeObject<IEnumerable<SizeDTO>>(sizeResponseContent);
                ViewBag.Sizes = sizes;
            }
            var response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var inventories = JsonConvert.DeserializeObject<IEnumerable<InventoryDTO>>(responseContent);

                var paginationHeader = response.Headers.GetValues("X-Pagination").FirstOrDefault();
                var paginationMetadata = JsonConvert.DeserializeObject<PaginationMetadata>(paginationHeader);

                ViewBag.Inventories = inventories;
                ViewBag.PaginationMetadata = paginationMetadata;
                using (var workbook = new XLWorkbook())
                {
                    ExcelConfiguration.exportInventory(inventories.ToList(), workbook);

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "inventories.xlsx");
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
        public async Task<ActionResult> Create()
        {

            List<InventoryProductDTO> listProduct = await GetProductsAsync();
            List<ColorDTO> listColor = await GetColorAsync();
            List<SizeDTO> listSize = await GetSizeAsync();
            ViewData["ProductId"] = new SelectList(listProduct, "Id", "Name");
            ViewData["ColorId"] = new SelectList(listColor, "ColorId", "ColorName");
            ViewData["SizeId"] = new SelectList(listSize, "SizeId", "SizeName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InventoryCreateUpdateDTO inventoryDTO)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(inventoryDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7115/api/Inventory/CreateInventory", stringContent);

            List<InventoryProductDTO> listProduct = await GetProductsAsync();
            List<ColorDTO> listColor = await GetColorAsync();
            List<SizeDTO> listSize = await GetSizeAsync();
            ViewData["ProductId"] = new SelectList(listProduct, "Id", "Name");
            ViewData["ColorId"] = new SelectList(listColor, "ColorId", "ColorName");
            ViewData["SizeId"] = new SelectList(listSize, "SizeId", "SizeName");

            if (response.IsSuccessStatusCode)
            {
                ViewData["Message"] = "Create successfully!";
                return View(inventoryDTO);
            }

            ViewData["Message"] = "Create fail, try again!";
            return View(inventoryDTO);
        }
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage inventoryResponse = await client.GetAsync("https://localhost:7115/api/Inventory/GetInventoryById" + "/" + id);
            string strInventory = await inventoryResponse.Content.ReadAsStringAsync();
            InventoryCreateUpdateDTO? inventoryDTO = JsonConvert.DeserializeObject<InventoryCreateUpdateDTO>(strInventory);
            List<InventoryProductDTO> listProduct = await GetProductsAsync();
            List<ColorDTO> listColor = await GetColorAsync();
            List<SizeDTO> listSize = await GetSizeAsync();
            ViewData["ProductId"] = new SelectList(listProduct, "Id", "Name");
            ViewData["ColorId"] = new SelectList(listColor, "ColorId", "ColorName");
            ViewData["SizeId"] = new SelectList(listSize, "SizeId", "SizeName");

            return View(inventoryDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(InventoryCreateUpdateDTO inventoryDTO)
        {
            
            var stringContent = new StringContent(JsonConvert.SerializeObject(inventoryDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("https://localhost:7115/api/Inventory/UpdateInventory/" + inventoryDTO.Id, stringContent);
            List<InventoryProductDTO> listProduct = await GetProductsAsync();
            List<ColorDTO> listColor = await GetColorAsync();
            List<SizeDTO> listSize = await GetSizeAsync();
            ViewData["ProductId"] = new SelectList(listProduct, "Id", "Name");
            ViewData["ColorId"] = new SelectList(listColor, "ColorId", "ColorName");
            ViewData["SizeId"] = new SelectList(listSize, "SizeId", "SizeName");


            if (response.IsSuccessStatusCode)
            {
                ViewData["Message"] = "Edit successfully!";
                return View(inventoryDTO);
            }

            ViewData["Message"] = "Edit fail, try again!";
            return View(inventoryDTO);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            await client.DeleteAsync("https://localhost:7115/api/Inventory/DeleteInventory" + "/" + id);

            return RedirectToAction(nameof(Index));
        }
        private async Task<List<InventoryProductDTO>> GetProductsAsync()
        {
            //Get Categories
            HttpResponseMessage productsResponse = await client.GetAsync("https://localhost:7115/api/Products/ExportExcel");
            string strCategories = await productsResponse.Content.ReadAsStringAsync();

            var productDTOs =  JsonConvert.DeserializeObject<List<ProductDTO>>(strCategories);
            var inventoryProducts = productDTOs.Select(x => new InventoryProductDTO
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return inventoryProducts;
        }
        private async Task<List<ColorDTO>> GetColorAsync()
        {
            //Get Categories
            HttpResponseMessage colorsResponse = await client.GetAsync(ColorApiUrl);
            string strColors = await colorsResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ColorDTO>>(strColors);
        }

        private async Task<List<SizeDTO>> GetSizeAsync()
        {
            //Get Categories
            HttpResponseMessage sizesResponse = await client.GetAsync(SizeApiUrl);
            string strSizes = await sizesResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<SizeDTO>>(strSizes);
        }
    }
}
