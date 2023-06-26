using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string? ReviewText { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
