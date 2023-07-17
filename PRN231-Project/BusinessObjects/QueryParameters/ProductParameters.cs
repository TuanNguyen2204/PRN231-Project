using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.QueryParameters
{
    public class ProductParameters : QueryStringParameters
    {
        public int CatId { get; set; }
        public string? ProductName { get; set; }
        public string? OrderBy { get; set; }
    }
}
