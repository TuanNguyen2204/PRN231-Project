using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class ProductCreateUpdateDTO
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Please enter the product name.")]
        public string? Name { get; set; } = null!;
        [Required(ErrorMessage = "Please enter the Description.")]
        public string? Description { get; set; } = null!;
        [Required(ErrorMessage = "Please enter the product price.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a valid price.")]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "Please choose the product image file")]
        public string? Image { get; set; } = null!;
        public int? CategoryId { get; set; }
    }
}
