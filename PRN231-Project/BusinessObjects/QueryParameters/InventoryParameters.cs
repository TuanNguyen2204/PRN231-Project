using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.QueryParameters
{
    public class InventoryParameters : QueryStringParameters
    {
        public int CatId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public string? ProductName { get; set; }
    }
}
