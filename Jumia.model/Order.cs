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
    public class Order : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        //public int UserID { get; set; }

        [Required]
        public DateTime DatePlaced { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<CartItem> OrderDetails { get; set; }
        public virtual Payment Payment { get; set; }
        
    }

}
