using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ClothesStoreContext ClothesStoreContext) : base(ClothesStoreContext)
        {
        }

       
        public IEnumerable<Product> GetAllProducts() => FindAll().ToList();

      

        public IEnumerable<Product> FindProductById(int id)
        {
            return FindByCondition(p => p.Id == id).Include(c=>c.Category);
        }
    }
    
}
