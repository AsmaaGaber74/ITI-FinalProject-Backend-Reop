using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Dtos.ViewModel.Product
{
    public class GetByCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int categoryid { get; set; }
    }
}