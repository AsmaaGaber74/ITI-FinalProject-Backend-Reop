using AutoMapper;
using Jumia.Application.Contract;
using Jumia.Dtos;
using Jumia.Dtos.ResultView;
using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Application.Services
{
    public class ProductService:IProductService
    {
        //here the service emplementation for this functions  and we can emplement manual mapping here 
        private readonly IProductReposatory productReposatory;
        private readonly IMapper _mapper;

        public ProductService(IProductReposatory productReposatory,IMapper mapper) 
        {
            this.productReposatory = productReposatory;
            _mapper = mapper;
        }

        public async Task<ResultView<ProductDTO>> Create(ProductDTO product)
        {
            var Query = (await productReposatory.GetAllAsync()); // se;ect * from product
            var OldProduct = Query.Where(p => p.Name == product.Name).FirstOrDefault();
            if (OldProduct != null)
            {
                return new ResultView<ProductDTO> { Entity = null, IsSuccess = false, Message = "Already Exist" };
            }
            else
            {
                var Prd = _mapper.Map<Product>(product);
                var NewPrd = await productReposatory.CreateAsync(Prd);
                await productReposatory.SaveChangesAsync();
                var PrdDto = _mapper.Map<ProductDTO>(NewPrd);
                return new ResultView<ProductDTO> { Entity = PrdDto, IsSuccess = true, Message = "Created Successfully" };
            }

        }
        public async Task<ResultView<ProductDTO>> SoftDelete(ProductDTO product)
        {
            try
            {
                var PRd = _mapper.Map<Product>(product);
                var Oldprd = (await productReposatory.GetAllAsync()).FirstOrDefault(p => p.Id == product.Id);
                Oldprd.IsDeleted = true;
                await productReposatory.SaveChangesAsync();
                var PrdDto = _mapper.Map<ProductDTO>(Oldprd);
                return new ResultView<ProductDTO> { Entity = PrdDto, IsSuccess = true, Message = "Deleted Successfully" };
            }
            catch (Exception ex)
            {
                return new ResultView<ProductDTO> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }
    }
}
