using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Jumia.Dtos
{
    public class CartItemDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [ForeignKey("Id")]
        public virtual OrderDto Order { get; set; }

        [ForeignKey("Id")]
        public virtual ProductDTO Product { get; set; }
    }
}