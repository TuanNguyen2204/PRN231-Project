using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Size
    {
        public Size()
        {
            Carts = new HashSet<Cart>();
            Inventories = new HashSet<Inventory>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int SizeId { get; set; }
        public string SizeName { get; set; } = null!;

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
