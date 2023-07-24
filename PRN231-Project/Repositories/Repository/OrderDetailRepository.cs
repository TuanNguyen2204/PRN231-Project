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
    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ClothesStoreContext ClothesStoreContext) : base(ClothesStoreContext)
        {
        }

        public void CreateOrderDetail(OrderDetail OrderDetail)
        {
            Create(OrderDetail);
        }

        public OrderDetail GetOrderDetailByProductIdColorIdSizeId(int productId, int colorId, int sizeId)
        {
            return FindByCondition(od =>
                od.ProductId == productId &&
                od.ColorId == colorId &&
                od.SizeId == sizeId
            ).FirstOrDefault();
        }

        public List<OrderDetail> GetOrderDetails(int OrderId)
        {
            var orderDetails = FindByCondition(o=> o.OrderId == OrderId).Include(p=>p.Product).Include(s=>s.Size).Include(c=>c.Color);
            return orderDetails.ToList();
        }
    }
}
