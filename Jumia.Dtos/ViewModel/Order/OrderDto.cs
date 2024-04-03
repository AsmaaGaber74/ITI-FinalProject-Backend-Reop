using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Jumia.Dtos.ViewModel.User;

namespace Jumia.Dtos.ViewModel.Order
{
    public class OrderDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserID { get; set; }
        [Required] public string UserName { get; set; }
        public int AddressId { get; set; }
        public string ?City { get; set; }
        public string ? Street { get; set; }
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