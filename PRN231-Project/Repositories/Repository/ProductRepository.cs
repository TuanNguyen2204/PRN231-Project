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
            OrderByPrice(ref products, productParameters.OrderBy);
            return PagedList<Product>.ToPagedList(products.Include(x => x.Category),
                    productParameters.PageNumber,
                    productParameters.PageSize);
        }
        public List<Product> GetAllProducts()
        {
            var products = FindAll();
            return products.ToList();
        }
        private void SearchByName(ref IQueryable<Product> products, string productName)
        {
            if (!products.Any() || string.IsNullOrWhiteSpace(productName))
                return;
            products = products.Where(o => o.Name.ToLower().Contains(productName.Trim().ToLower()));
        }
        private void OrderByPrice(ref IQueryable<Product> products, string orderby)
        {
            if (!products.Any() || string.IsNullOrWhiteSpace(orderby))
                return;

            switch (orderby.ToLower())
            {
                case "price-desc":
                    products = products.OrderByDescending(o => o.Price);
                    break;
                case "price-increase":
                    products = products.OrderBy(o => o.Price);
                    break;
                default:
                    // Handle the default case, such as not applying any sorting
                    break;
            }
        }

        public void UpdateProduct(Product product)
        {
            Update(product);
        }

    }
}
