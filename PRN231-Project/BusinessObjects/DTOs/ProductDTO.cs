using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Image { get; set; } = null!;
        public string CategoryName { get; set; } = null!;

    }
}
