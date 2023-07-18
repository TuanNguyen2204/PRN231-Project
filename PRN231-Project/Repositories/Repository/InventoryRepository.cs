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

        public IEnumerable<Inventory> ExportExel(InventoryParameters inventoryParameters)
        {
            var inventories = FindAll();

            if (inventoryParameters.CatId != 0)
            {
                inventories = inventories.Where(p => p.Product.Category.CategoryId == inventoryParameters.CatId);
            }

            if (inventoryParameters.ColorId != 0)
            {
                inventories = inventories.Where(p => p.ColorId == inventoryParameters.ColorId);
            }

            if (inventoryParameters.SizeId != 0)
            {
                inventories = inventories.Where(p => p.SizeId == inventoryParameters.SizeId);
            }
            SearchByName(ref inventories, inventoryParameters.ProductName);
            return inventories;
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

            if (inventoryParameters.CatId != 0)
            {
                inventories = inventories.Where(p => p.Product.Category.CategoryId == inventoryParameters.CatId);
            }

            if (inventoryParameters.ColorId != 0)
            {
                inventories = inventories.Where(p => p.ColorId == inventoryParameters.ColorId);
            }

            if (inventoryParameters.SizeId != 0)
            {
                inventories = inventories.Where(p => p.SizeId == inventoryParameters.SizeId);
            }
            SearchByName(ref inventories, inventoryParameters.ProductName);

            return PagedList<Inventory>.ToPagedList(inventories.Include(p => p.Product).ThenInclude(p => p.Category).Include(s => s.Size).Include(c => c.Color),
                         inventoryParameters.PageNumber,
                         inventoryParameters.PageSize);
        }


        public Inventory GetInventoryDetails(int InventoryId)
        {
            return FindByCondition(i => i.Id.Equals(InventoryId)).Include(p => p.Product).ThenInclude(p => p.Category).Include(s => s.Size).Include(c => c.Color).FirstOrDefault();
        }

       

        public void UpdateInventory(Inventory Inventory)
        {
            Update(Inventory);
        }

        public List<Inventory> GetAllInventories()
        {
            var inventories = FindAll();
            return inventories.ToList();
        }
        private void SearchByName(ref IQueryable<Inventory> inventories, string productName)
        {
            if (!inventories.Any() || string.IsNullOrWhiteSpace(productName))
                return;
            inventories = inventories.Where(o => o.Product.Name.ToLower().Contains(productName.Trim().ToLower()));
        }
    }
}
