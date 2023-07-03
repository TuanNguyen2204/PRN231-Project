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
        private IProductRepository _productRepository;
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

        public IProductRepository Product
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_context);
                }
                return _productRepository;
            }
        }

        public void Save()
        {
           _context.SaveChanges();
        }
    }
}
