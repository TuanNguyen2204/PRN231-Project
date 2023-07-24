using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IOrderDetailRepository  :IRepositoryBase<OrderDetail>
    {
        List<OrderDetail> GetOrderDetails(int OrderId);
        void CreateOrderDetail(OrderDetail OrderDetail);
       OrderDetail GetOrderDetailByProductIdColorIdSizeId(int productId, int colorId, int sizeId);
    }
}
