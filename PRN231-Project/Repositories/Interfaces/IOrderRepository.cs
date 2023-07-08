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
        void UpdateOrder(Order Order);
        void DeleteOrder(Order Order);
        IEnumerable<Order> ExportExel();
    }
}
