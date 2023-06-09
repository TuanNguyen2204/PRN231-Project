﻿using BusinessObjects.Models;
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
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(ClothesStoreContext ClothesStoreContext) : base(ClothesStoreContext)
        {
        }

        public void DeleteOrder(Order Order)
        {
            Delete(Order);
        }

        public IEnumerable<Order> ExportExel()
        {
            return FindAll().Include(x=>x.User).ToList();
        }

        public PagedList<Order> GetOrders(OrderParameters OrderParameters)
        {
            var orders = FindAll();
            FilterByOrderDate(ref orders, OrderParameters.StartDate, OrderParameters.EndDate);
            return PagedList<Order>.ToPagedList(orders.Include(o => o.User),
                    OrderParameters.PageNumber,
                    OrderParameters.PageSize);
        }

        private void FilterByOrderDate(ref IQueryable<Order> orders, DateTime startDate, DateTime endDate)
        {
            orders = orders.Where(o => o.DateOrdered >= startDate && o.DateOrdered <= endDate);
        }

        public void UpdateOrder(Order Order)
        {
            Update(Order);
        }
    }
}
