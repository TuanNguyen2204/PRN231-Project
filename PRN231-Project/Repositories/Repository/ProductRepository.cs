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
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ClothesStoreContext ClothesStoreContext) : base(ClothesStoreContext)
        {
        }

        public void CreateProduct(Product product)
        {
            Create(product);
        }

        public void DeleteProduct(Product product)
        {
            Delete(product);
        }

        public Product GetProductDetails(int productId)
        {
            return FindByCondition(p => p.Id.Equals(productId)).Include(c => c.Category).FirstOrDefault();
        }

        public IEnumerable<Product> GetProducts(ProductParameters productParameters)
        {
            return FindAll().Skip((productParameters.PageNumber -1)*productParameters.PageSize).Take(productParameters.PageSize).Include(x => x.Category).ToList();
        }

        public void UpdateProduct(Product product)
        {
            Update(product);
        }
    }
}
