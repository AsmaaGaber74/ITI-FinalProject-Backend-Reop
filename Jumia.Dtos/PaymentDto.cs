using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Jumia.Dtos
{
    public class PaymentDto
    {
        [Key]
        [ForeignKey("Order")]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DatePaid { get; set; }

        public enum PaymentMethod
        {
            PayPal,
            CashOnDelievary,

        }

        [Required]
        [MaxLength(50)]
        public PaymentMethod paymentMethod { get; set; }
        public virtual OrderDto Order { get; set; }
    }
}
