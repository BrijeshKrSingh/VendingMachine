using Microsoft.EntityFrameworkCore;
using NuanceVendingMachine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuanceVendingMachine.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly VendingMachineDataContext _context;
        public ProductRepository(VendingMachineDataContext context)
        {
            _context = context;

        }
        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.Include(p => p.ProductType).ToListAsync();
        }
        public async Task AddProduct(Product product)
        {
            if (product != null)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProduct(Product product)
        {
            if (product != null)
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
           
        }
        public async Task<IEnumerable<ProductType>> GetProductTypes()
        {
            return await _context.ProductTypes.ToArrayAsync();
              
        }

    }
}
