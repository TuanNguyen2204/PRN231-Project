﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.QueryParameters
{
    public class UserParameters : QueryStringParameters
    {
        public string? Name { get; set; }
    }
}
