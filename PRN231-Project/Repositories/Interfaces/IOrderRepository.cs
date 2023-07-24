using BusinessObjects.Models;
using BusinessObjects.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        PagedList<Order> GetOrders(OrderParameters OrderParameters);
        void CreateOrder(Order Order);
        void UpdateOrder(Order Order);
        void DeleteOrder(Order Order);
        Order GetOrderDetails(int orderId);
        List<Order> GetOrderByUserId(int userid);
    }
}
