using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class SizeDTO
    {
       
        public int SizeId { get; set; }
      
        public string SizeName { get; set; } = null!;
    }
}
