using BusinessObjects.DTOs;
using ClosedXML.Excel;

namespace eClothesClient.Configuration
{
    public class ExcelConfiguration
    {
        public static XLWorkbook exportProduct(List<ProductDTO> listProducts, XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Products");
            worksheet.Cell(1, 1).Value = "CLOTHES SHOP";
            worksheet.Cell(2, 1).Value = "LIST OF PRODUCTS";
            var currentRow = 3;
            worksheet.Cell(currentRow, 1).Value = "Product id";
            worksheet.Cell(currentRow, 2).Value = "Product name";
            worksheet.Cell(currentRow, 3).Value = "Price";
            worksheet.Cell(currentRow, 4).Value = "Image";
            worksheet.Cell(currentRow, 5).Value = "Category Name";
            foreach (var product in listProducts)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = product.Id;
                worksheet.Cell(currentRow, 2).Value = product.Name;
                worksheet.Cell(currentRow, 3).Value = product.Price;
                worksheet.Cell(currentRow, 4).Value = product.Image;
                worksheet.Cell(currentRow, 5).Value = product.CategoryName;
                
            }
            // Auto-fit columns after adding data
            worksheet.Columns().AdjustToContents();
            return workbook;
        }

        public static XLWorkbook exportInventory(List<InventoryDTO> listInventories, XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Inventories");
            worksheet.Cell(1, 1).Value = "CLOTHES SHOP";
            worksheet.Cell(2, 1).Value = "LIST OF Inventories";
            var currentRow = 3;
            worksheet.Cell(currentRow, 1).Value = "Id";
            worksheet.Cell(currentRow, 2).Value = "Product name";
            worksheet.Cell(currentRow, 3).Value = "Size";
            worksheet.Cell(currentRow, 4).Value = "Color";
            worksheet.Cell(currentRow, 5).Value = "Quantity";
            worksheet.Cell(currentRow, 6).Value = "Category Name";
            foreach (var product in listInventories)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = product.Id;
                worksheet.Cell(currentRow, 2).Value = product.ProductName;
                worksheet.Cell(currentRow, 3).Value = product.SizeName;
                worksheet.Cell(currentRow, 4).Value = product.ColorName;
                worksheet.Cell(currentRow, 5).Value = product.Quantity;
                worksheet.Cell(currentRow, 6).Value = product.CategoryName;

            }
            // Auto-fit columns after adding data
            worksheet.Columns().AdjustToContents();
            return workbook;
        }
        public static XLWorkbook exportUser(List<UserDTO> listUsers, XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Users");
            worksheet.Cell(1, 1).Value = "CLOTHES SHOP";
            worksheet.Cell(2, 1).Value = "LIST OF Users";
            var currentRow = 3;
            worksheet.Cell(currentRow, 1).Value = "UserId";
            worksheet.Cell(currentRow, 2).Value = "User name";
            worksheet.Cell(currentRow, 3).Value = "Email";
            worksheet.Cell(currentRow, 4).Value = "Address";
            worksheet.Cell(currentRow, 5).Value = "Role";
            foreach (var user in listUsers)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = user.Id;
                worksheet.Cell(currentRow, 2).Value = user.Name;
                worksheet.Cell(currentRow, 3).Value = user.Email;
                worksheet.Cell(currentRow, 4).Value = user.Address;
                worksheet.Cell(currentRow, 5).Value = user.IsSeller ? "Seller" : "Customer";

            }
            // Auto-fit columns after adding data
            worksheet.Columns().AdjustToContents();
            return workbook;
        }

        public static XLWorkbook exportOrder(List<OrderDTO> listOrders, XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Orders");
            worksheet.Cell(1, 1).Value = "CLOTHES SHOP";
            worksheet.Cell(2, 1).Value = "LIST OF Orders";
            var currentRow = 3;
            worksheet.Cell(currentRow, 1).Value = "Id";
            worksheet.Cell(currentRow, 2).Value = "User name";
            worksheet.Cell(currentRow, 3).Value = "Total Quantity";
            worksheet.Cell(currentRow, 4).Value = "Orderd Date";
            worksheet.Cell(currentRow, 5).Value = "Total Price";
            foreach (var order in listOrders)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = order.Id;
                worksheet.Cell(currentRow, 2).Value = order.Username;
                worksheet.Cell(currentRow, 3).Value = order.Quantity;
                worksheet.Cell(currentRow, 4).Value = order.DateOrdered;
                worksheet.Cell(currentRow, 5).Value = order.TotalPrice;

            }
            // Auto-fit columns after adding data
            worksheet.Columns().AdjustToContents();
            return workbook;
        }

    }
}
