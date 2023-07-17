using BusinessObjects.DTOs;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace eClothesClient.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";
        public static decimal? total = 0;


        public CartController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7115/api/Products/GetProductDetail";

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
       


        [HttpPost]
        public async Task< IActionResult> AddToCart([FromBody] CartItemDTO cartItem)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = new List<CartItemDTO>();

            if (!string.IsNullOrEmpty(cartJson))
            {
                cart = JsonConvert.DeserializeObject<List<CartItemDTO>>(cartJson);
            }

            // Check if the product already exists in the cart with the same color and size
            var existingCartItem = cart.FirstOrDefault(item =>
                item.Product.Id == cartItem.Product.Id &&
                item.ColorId == cartItem.ColorId &&
                item.SizeId == cartItem.SizeId
            );
            //Get Products
            HttpResponseMessage productsResponse = await client.GetAsync(ProductApiUrl + "/" + cartItem.Product.Id);
            string strProduct = await productsResponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            ProductDTO? Product = JsonConvert.DeserializeObject<ProductDTO>(strProduct);

            total = 0;
            if (existingCartItem != null)
            {
                // Update the quantity of the existing cart item
                existingCartItem.Quantity += cartItem.Quantity;
            }
            else
            {
                // Add the cart item to the cart
                cart.Add(cartItem);
                total += Product.Price;
            }
            ViewData["total"] = total;
            // Save the updated cart back to the session
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

            return Ok();
        }

        public async Task<IActionResult> OnClickAction(int productId, int colorId, int sizeId, string? actionType)
        {
            string? cart = HttpContext.Session.GetString("Cart");
            List<CartItemDTO> CartItems = JsonConvert.DeserializeObject<List<CartItemDTO>>(cart);

            // Get Products
            HttpResponseMessage productsResponse = await client.GetAsync(ProductApiUrl + "/" + productId);
            string strProduct = await productsResponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            ProductDTO? Product = JsonConvert.DeserializeObject<ProductDTO>(strProduct);
            int index;

            switch (actionType)
            {
                case "plus":
                    index = getIndexOfProductInCart(Product, colorId, sizeId, CartItems);
                    CartItems[index].Quantity++;
                    break;
                case "minus":
                    index = getIndexOfProductInCart(Product, colorId, sizeId, CartItems);
                    CartItems[index].Quantity--;
                    if (CartItems[index].Quantity == 0)
                        CartItems.RemoveAt(index);
                    break;
                case "remove":
                    index = getIndexOfProductInCart(Product, colorId, sizeId, CartItems);
                    CartItems.RemoveAt(index);
                    break;
                default:
                    break;
            }

            // Serialize the updated cart and store it in the session
            string cartSerialized = JsonConvert.SerializeObject(CartItems);
            HttpContext.Session.SetString("Cart", cartSerialized);

            if (CartItems.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }

            total = GetTotal(CartItems);
            ViewData["total"] = total;

            return RedirectToAction("Index", "Cart");
        }

        public int getIndexOfProductInCart(ProductDTO product, int colorId, int sizeId, List<CartItemDTO> carts)
        {
            for (int i = 0; i < carts.Count; i++)
            {
                if (carts[i].Product.Id == product.Id && carts[i].ColorId == colorId && carts[i].SizeId == sizeId)
                    return i;
            }
            return -1;
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
