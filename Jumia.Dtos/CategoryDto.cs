using System.ComponentModel.DataAnnotations;

namespace Jumia.Dtos
{
    public class CategoryDto
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public virtual ICollection<ProductDTO> Products { get; set; }
    }
}