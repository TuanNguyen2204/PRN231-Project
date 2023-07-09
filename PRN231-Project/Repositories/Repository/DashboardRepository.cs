using BusinessObjects.DTOs;
using BusinessObjects.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ClothesStoreContext _context;

        public DashboardRepository(ClothesStoreContext context)
        {
            _context = context;
        }

        public List<BestSeller> GetBestSellers()
        {
            var bestSellers = _context.Products
                .Join(_context.OrderDetails,
                p => p.Id,
                od => od.ProductId,
                (p, od) => new { p.Id, p.Name, od.Quantity })
                .GroupBy(p => new { p.Id, p.Name })
                .Select(g => new BestSeller
                {
                    ProductName = g.Key.Name,
                    Count = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(p => p.Count)
                        .Take(5)
                        .ToList();
            return bestSellers;
        }

        public List<RevenuePerMonth> GetRevenuePerMonth()
        {
            var revenueMonths = _context.Orders.Select(o =>
                    new
                    {
                        Id = o.Id,
                        Month = o.DateOrdered.Month,
                        Year = o.DateOrdered.Year,
                        Revenue = o.TotalPrice, 
                    }).GroupBy(x => new
                    {
                        x.Year,
                        x.Month,
                    }).Select(x => new RevenuePerMonth()
                    {
                        Year = x.Key.Year,
                        Month = x.Key.Month,
                        RevenueMonth = x.Sum(x=>x.Revenue)
                    }).OrderBy(x => x.Year).ThenBy(x => x.Month).ToList();
            return revenueMonths;
        }

        public List<TopUser> GetTopUsers()
        {
            var topUsers = _context.Orders.Join(_context.Users,
                o => o.UserId,
                u => u.Id,
                (o, u) => new { u.Id, u.Username, o.TotalPrice })
                .GroupBy(p => new { p.Id, p.Username })
                .Select(g => new TopUser
                {
                    Username = g.Key.Username,
                    TotalBill = g.Sum(x => x.TotalPrice)
                }).Take(5).OrderByDescending(x => x.TotalBill).ToList();
            return topUsers;
        }
    }
}
