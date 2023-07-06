using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessObjects.DTOs
{
    public class InventoryCreateUpdateDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int Quantity { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            InventoryCreateUpdateDTO otherObj = (InventoryCreateUpdateDTO)obj;
            return ProductId == otherObj.ProductId && SizeId == otherObj.SizeId && ColorId == otherObj.ColorId;
        }
    }
}
