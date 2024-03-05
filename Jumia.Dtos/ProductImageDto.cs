using System.ComponentModel.DataAnnotations.Schema;

namespace Jumia.Dtos
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public string Path { get; set; }
        [ForeignKey("ProductID")]
        public virtual ProductDTO Product { get; set; }
    }
}