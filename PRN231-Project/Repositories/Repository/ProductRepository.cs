using BusinessObjects.Models;
using BusinessObjects.QueryParameters;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public PagedList<Product> GetProducts(ProductParameters productParameters)
        {
            var products = FindAll();
            if (productParameters.CatId!=0)
            {
                products = FindByCondition(p => p.CategoryId == productParameters.CatId);
            }
            SearchByName(ref products, productParameters.ProductName);
            return PagedList<Product>.ToPagedList(products.OrderBy(p => p.Name).Include(x => x.Category),
                    productParameters.PageNumber,
                    productParameters.PageSize);
        }
        private void SearchByName(ref IQueryable<Product> products, string productName)
        {
            if (!products.Any() || string.IsNullOrWhiteSpace(productName))
                return;
            products = products.Where(o => o.Name.ToLower().Contains(productName.Trim().ToLower()));
        }
        public void UpdateProduct(Product product)
        {
            Update(product);
        }

        public IEnumerable<Product> ExportExel()
        {
            return FindAll().Include(x => x.Category)
                  .OrderBy(p => p.Name)
                  .ToList();
        }
    }
}
