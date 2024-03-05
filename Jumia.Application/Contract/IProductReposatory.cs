using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Application.Contract
{
    public interface IProductReposatory : IRepository<Product, int>
    {
        //to add any bonus functionality that not found on ireposatory 
    }
}
