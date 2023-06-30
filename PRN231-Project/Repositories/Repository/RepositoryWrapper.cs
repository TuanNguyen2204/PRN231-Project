using BusinessObjects.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ClothesStoreContext _context;
        private IColorRepository _colorRepository;

        public RepositoryWrapper(ClothesStoreContext context)
        {
            _context = context;
        }

        public IColorRepository Color
        {
            get
            {
                if (_colorRepository == null)
                {
                    _colorRepository = new ColorRepository(_context);
                }
                return _colorRepository;
            }
        }

        public void Save()
        {
           _context.SaveChanges();
        }
    }
}
