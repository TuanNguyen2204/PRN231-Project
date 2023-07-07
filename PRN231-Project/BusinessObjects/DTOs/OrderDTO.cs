using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Quantity { get; set; }
        public DateTime DateOrdered { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string DeliveryLocation { get; set; } = null!;
        public decimal TotalPrice { get; set; }
    }
}
