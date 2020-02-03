using Microsoft.EntityFrameworkCore;
using NuanceVendingMachine.Models;

namespace NuanceVendingMachine.Repository
{
    public class VendingMachineDataContext : DbContext
    {
        public VendingMachineDataContext(DbContextOptions<VendingMachineDataContext> options):base(options)
        {
            Database.SetCommandTimeout(15000);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }


    }
}
