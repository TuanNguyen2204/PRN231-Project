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

    }
}
