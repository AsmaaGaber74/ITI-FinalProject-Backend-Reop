using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Jumia.Dtos.ViewModel
{
    public class CateogaryViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public virtual ICollection<ProuductViewModel> Products { get; set; }
    }
}