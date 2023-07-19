using BusinessObjects.Models;
using BusinessObjects.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IInventoryRepository : IRepositoryBase<Inventory>
    {
        PagedList<Inventory> GetInventories(InventoryParameters inventoryParameters);
        Inventory GetInventoryDetails(int InventoryId);
        void CreateInventory(Inventory Inventory);
        void UpdateInventory(Inventory Inventory);
        void DeleteInventory(Inventory Inventory);
        List<Inventory> GetAllInventories();

        IEnumerable<Color> GetColorByProductId(int productId);

        IEnumerable<Size> GetSizeByColorId(int productId, int colorId);
    }
}
