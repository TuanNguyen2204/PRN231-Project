using BusinessObjects.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class SizeRepository : RepositoryBase<Size>, ISizeRepository
    {
        public SizeRepository(BusinessObjects.Models.ClothesStoreContext ClothesStoreContext) : base(ClothesStoreContext)
        {
        }
        public IEnumerable<Size> GetAllSizes() => FindAll().ToList();
    }
}
