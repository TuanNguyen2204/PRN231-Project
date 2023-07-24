using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class OrderCreateDTO
    {
       
        public int UserId { get; set; }
        public DateTime DateOrdered { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string DeliveryLocation { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public List<OrderDetailsDTO> OrderDetails { get; set; }
    }
}
