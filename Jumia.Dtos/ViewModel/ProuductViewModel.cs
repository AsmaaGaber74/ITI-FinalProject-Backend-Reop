using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Jumia.Dtos.ViewModel
{
    public class ProuductViewModel
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        //[Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        public DateTime DateListed { get; set; }
        public string? SellerID { get; set; }

        public virtual CateogaryViewModel Category { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public virtual LoginViewModel? Seller { get; set; }
        public string? SellerName { get; set; }
        public bool IsDeleted { get; set; }=false;

        // public virtual ICollection<ProductImageViewModel> ProductImages { get; set; }
    }
}