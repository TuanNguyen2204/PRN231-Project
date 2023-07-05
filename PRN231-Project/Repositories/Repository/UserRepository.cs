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
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ClothesStoreContext ClothesStoreContext) : base(ClothesStoreContext)
        {
        }

        public void DeleteUser(User User)
        {
            Delete(User);
        }

        public IEnumerable<User> ExportExel()
        {
            return FindAll()
                 .OrderBy(u => u.Name)
                 .ToList();
        }

        public User GetUserById(int userId)
        {
            return FindByCondition(u => u.Id.Equals(userId)).FirstOrDefault();
        }

        public PagedList<User> GetUsers(UserParameters UserParameters)
        {
            var users = FindAll();
            SearchByName(ref users, UserParameters.Name);
            return PagedList<User>.ToPagedList(users.OrderBy(u => u.Username),
                    UserParameters.PageNumber,
                    UserParameters.PageSize);
        }
        private void SearchByName(ref IQueryable<User> users, string userName)
        {
            if (!users.Any() || string.IsNullOrWhiteSpace(userName))
                return;
            users = users.Where(o => o.Name.ToLower().Contains(userName.Trim().ToLower()));
        }

        public void UpdateUser(User user)
        {
            Update(user);
        }
    }
}
