using System.ComponentModel.DataAnnotations;

namespace NuanceVendingMachine.Dto
{
    public class ProductDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int AvailableQuantity { get; set; }
        [Required]
        public int ProductTypeId { get; set; }
        [Display(Name = "Type")]
        public string ProductType { get; set; }
        [Display(Name = "Price in cents")]
        public int Price { get; set; }

    }
}
