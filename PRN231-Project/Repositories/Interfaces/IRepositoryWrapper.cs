﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        IColorRepository Color { get; }
        IProductRepository Product { get; }
        IUserRepository User { get; }
        IInventoryRepository Inventory { get; } 
        void Save();
    }
}
