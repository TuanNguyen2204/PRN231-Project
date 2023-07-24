using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class Token
    {
        public string access_token { get; set; }
        public string expires_access_token { get; set; }
        public string refresh_token { get; set; }
        public string expires_refresh_token { get; set; }
    }
}
