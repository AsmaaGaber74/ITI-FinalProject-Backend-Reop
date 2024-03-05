using Jumia.Dtos.ResultView;
using Jumia.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Application.Services
{
    public interface IProductService
    {
        //here the service functions 
        Task<ResultView<ProductDTO>> Create(ProductDTO product);

    }
}
