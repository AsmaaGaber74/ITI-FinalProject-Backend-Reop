using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Dtos
{
    public class ProductDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SellerID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        public DateTime DateListed { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUserDto Seller { get; set; }

        [ForeignKey("Id")]
        public virtual CategoryDto Category { get; set; }

        //public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<CartItemDto> OrderDetails { get; set; }
        public virtual ICollection<ItemDto> items { get; set; }
        public virtual ICollection<ProductImageDto> ProductImages { get; set; }
    }
}
