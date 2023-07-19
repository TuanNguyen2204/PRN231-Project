using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using System.Net.Http.Headers;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Bibliography;
using BusinessObjects.DTOs;
using Newtonsoft.Json;
using ClosedXML.Excel;
using eClothesClient.Configuration;
using System.Text;
using System.Net;

namespace eClothesClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly HttpClient client = null;
        private string UserApiUrl = "";

        public UsersController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            UserApiUrl = "https://localhost:7115/api/User/GetUsers";
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index(string? name, int PageNumber = 1, int PageSize = 10)
        {
            var apiUrl = UserApiUrl + $"?&PageNumber={PageNumber}&PageSize={PageSize}";
            if (!string.IsNullOrEmpty(name))
            {
                apiUrl += $"&Name={name}";
            }
            var response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<IEnumerable<UserDTO>>(responseContent);

                var paginationHeader = response.Headers.GetValues("X-Pagination").FirstOrDefault();
                var paginationMetadata = JsonConvert.DeserializeObject<PaginationMetadata>(paginationHeader);

                ViewBag.Users = users;
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

        public async Task<IActionResult> ExportExcel(string? name)
        {
            var apiUrl = "https://localhost:7115/api/User/ExportExcel" + $"?";
            if (!string.IsNullOrEmpty(name))
            {
                apiUrl += $"&Name={name}";
            }
            var response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<IEnumerable<UserDTO>>(responseContent);

                var paginationHeader = response.Headers.GetValues("X-Pagination").FirstOrDefault();
                var paginationMetadata = JsonConvert.DeserializeObject<PaginationMetadata>(paginationHeader);

                ViewBag.Users = users;
                ViewBag.PaginationMetadata = paginationMetadata;
                using (var workbook = new XLWorkbook())
                {
                    ExcelConfiguration.exportUser(users.ToList(), workbook);

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "users.xlsx");
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

        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage userResponse = await client.GetAsync("https://localhost:7115/api/User/GetUserDetail" + "/" + id);
            string strUser = await userResponse.Content.ReadAsStringAsync();
            UserDTO? userDTO = JsonConvert.DeserializeObject<UserDTO>(strUser);
            return View(userDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserDTO userDTO)
        {

            var stringContent = new StringContent(JsonConvert.SerializeObject(userDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("https://localhost:7115/api/User/UpdateUser/" + userDTO.Id, stringContent);

            if (response.IsSuccessStatusCode)
            {
                ViewData["Message"] = "Edit successfully!";
                return View(userDTO);
            }

            ViewData["Message"] = "Edit fail, try again!";
            return View(userDTO);
        }

        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserCreateDTO user)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://localhost:7115/api/User/CreateUser", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    ViewData["Message"] = "Create successfully!";
                    return View(user);
                }
                else if (response.StatusCode == HttpStatusCode.FailedDependency)
                {
                    ViewData["Message"] = "Email is exist!";
                }
                else
                {
                    ViewData["Message"] = "Create fail, try again!";
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ViewData["Message"] = "An error occurred: " + ex.Message;
                return View(user);
            }
        }
        public async Task<IActionResult> Delete(int? id)
        {
            await client.DeleteAsync("https://localhost:7115/api/User/DeleteUser" + "/" + id);

            return RedirectToAction(nameof(Index));
        }


    }
}
