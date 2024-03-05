using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jumia.model;

namespace Jumia.Model
{
    public class Payment : BaseEntity
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
        public virtual Order Order { get; set; }
    }

}
