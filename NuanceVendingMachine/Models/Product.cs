using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuanceVendingMachine.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Stock { get; set; }

        [ForeignKey("ProductTypeId")]
        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }
     }
}
