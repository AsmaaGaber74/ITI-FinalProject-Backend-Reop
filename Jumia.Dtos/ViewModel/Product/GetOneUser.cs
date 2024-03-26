using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Dtos.ViewModel.Product
{
    public class GetOneUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public ICollection<string> Productimages { get; set; }
        public string ProductDescription { get; set; }
        public ICollection<string> colors { get; set; }
        public ICollection<string> itemimages { get; set; }
        public decimal price { get; set; }
    }
}