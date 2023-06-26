using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            Inventories = new HashSet<Inventory>();
            OrderDetails = new HashSet<OrderDetail>();
            Reviews = new HashSet<Review>();
            Sales = new HashSet<Sale>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Image { get; set; } = null!;
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
