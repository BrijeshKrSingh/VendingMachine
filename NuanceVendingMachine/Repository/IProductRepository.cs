using NuanceVendingMachine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuanceVendingMachine.Repository
{
    public interface IProductRepository
    {
        Task AddProduct(Product product);
        Task DeleteProduct(int id);
        Task<IEnumerable<Product>> GetProducts();
        Task UpdateProduct(Product product);
        Task<Product> GetProductById(int id);
        Task<IEnumerable<ProductType>> GetProductTypes();
    }
}