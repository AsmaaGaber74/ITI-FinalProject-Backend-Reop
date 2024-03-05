using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Jumia.Dtos
{
    public class OrderDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public DateTime DatePlaced { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUserDto User { get; set; }

        public virtual ICollection<CartItemDto> OrderDetails { get; set; }
        public virtual PaymentDto Payment { get; set; }

    }
}