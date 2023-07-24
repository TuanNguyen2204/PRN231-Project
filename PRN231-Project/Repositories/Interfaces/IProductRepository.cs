using BusinessObjects.Models;
using BusinessObjects.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        PagedList<Product> GetProducts(ProductParameters productParameters);
        Product GetProductDetails(int productId);
        void CreateProduct(Product product);    
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        List<Product> GetAllProducts();
    }
}
