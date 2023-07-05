using BusinessObjects.Models;
using BusinessObjects.QueryParameters;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        PagedList<User> GetUsers(UserParameters UserParameters);
        User GetUserById(int userId);
        void UpdateUser(User user);
        void DeleteUser(User User);
        IEnumerable<User> ExportExel();
    }
}
