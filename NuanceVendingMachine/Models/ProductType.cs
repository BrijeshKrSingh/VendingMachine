using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuanceVendingMachine.Models
{
    [Table("Producttype")]
    public class ProductType
    {
        [Key]
        public int ProductTypeId { get; set; }
        public string TypeName { get; set; }
        public int Price { get; set; }

    }
}
