using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucure.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;

        public ProductService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<Product> CreatAsync(Product product)
        {
            _uow.Repository<Product>().Add(product);
            await _uow.Complete();
            return product;
        }
        public async Task<Product> UpdateAsync(Product product)
        {
            _uow.Repository<Product>().Update(product);
            await _uow.Complete();
            return product;
        }
        public Task<bool> DeleteAsync(int id)
        {
            var product = _uow.Repository<Product>().GetByIdAsync(id).Result;
            if (product == null) return Task.FromResult(false);
            _uow.Repository<Product>().Delete(product);
            _uow.Complete().Wait();
            return Task.FromResult(true);
        }

     
    }
}
