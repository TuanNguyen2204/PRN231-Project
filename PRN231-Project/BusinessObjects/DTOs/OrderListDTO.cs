﻿using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class OrderListDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime DateOrdered { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string DeliveryLocation { get; set; } = null!;
        public decimal TotalPrice { get; set; }
       
        public List<OrderDetailDTO>? OrderDetails { get; set; }
    }
}
