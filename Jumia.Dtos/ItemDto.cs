using System.ComponentModel.DataAnnotations.Schema;

namespace Jumia.Dtos
{
    public class ItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string String { get; set; }
        [ForeignKey("Id")]
        public ProductDTO Product { get; set; }
    }
}