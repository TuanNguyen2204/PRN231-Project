using BusinessObjects.Models;
using BusinessObjects.QueryParameters;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class InventoryRepository : RepositoryBase<Inventory>, IInventoryRepository
    {
        public InventoryRepository(ClothesStoreContext ClothesStoreContext) : base(ClothesStoreContext)
        {
        }

        public void CreateInventory(Inventory Inventory)
        {
            Create(Inventory);
        }

        public void DeleteInventory(Inventory Inventory)
        {
            Delete(Inventory);
        }

        public IEnumerable<Inventory> ExportExel()
        {
            return FindAll().Include(p => p.Product).ThenInclude(p => p.Category).Include(s => s.Size).Include(c => c.Color).ToList();
        }

        public IEnumerable<Color> GetColorByProductId(int productId)
        {
            return FindByCondition(i => i.ProductId.Equals(productId)).Select(inv => inv.Color).Distinct();
        }
        public IEnumerable<Size> GetSizeByColorId(int productId, int colorId)
        {
            return FindByCondition(i => i.ProductId.Equals(productId) && i.ColorId.Equals(colorId)).Select(inv => inv.Size).Distinct();
        }
        public PagedList<Inventory> GetInventories(InventoryParameters inventoryParameters)
        {
            var inventories = FindAll();
            return PagedList<Inventory>.ToPagedList(inventories.Include(p => p.Product).ThenInclude(p => p.Category).Include(s => s.Size).Include(c => c.Color),
                     inventoryParameters.PageNumber,
                     inventoryParameters.PageSize);
        }

        public Inventory GetInventoryDetails(int InventoryId)
        {
            return FindByCondition(i => i.Id.Equals(InventoryId)).FirstOrDefault();
        }

       

        public void UpdateInventory(Inventory Inventory)
        {
            Update(Inventory);
        }
    }
}
