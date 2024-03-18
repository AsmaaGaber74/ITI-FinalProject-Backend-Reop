using Jumia.Application.Services;
using Jumia.Dtos.ViewModel.Product;
using Jumia.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazonWebSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IItemServices _itemServices;
        private readonly IProductImageService _productImageService;

        public ProductController(IProductService productService, IItemServices itemServices, IProductImageService productImageService)
        {
            _productService = productService;
            _itemServices = itemServices;
            _productImageService = productImageService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllPagination(10, 1);
            var items = await _itemServices.GetAllPagination(10, 1);
            var ProductsDTo = products.Entities.Select(p => new GetAllPaginationUser
            {
                Price = p.Price,
                Description = p.Description,
                Name = p.Name,
                id = p.Id
            }).ToList();
            foreach (var item in ProductsDTo)
            {
                var Productimages = (await _productImageService.GetByProductIdAsync(item.id)).Select(p => p.Path).ToList();
                var Productsimage = items.Entities.Where(p => p.ProductId == item.id).Select(p => p.Color).ToList();
                item.itemscolor = Productsimage;
                item.ProductImages = Productimages;
            }
            return Ok(ProductsDTo);
        }
        [HttpGet]
        public async Task<IActionResult> Getone(int id)
        {
            var Products = await _productService.GetOne(id);
            var Productdetails = new GetOneUser
            {
                price = Products.Price,
                ProductDescription = Products.Description,
                Id = Products.Id
            };
            var Productimages = (await _productImageService.GetByProductIdAsync(id)).Select(p => p.Path).ToList();
            Productdetails.Productimages = Productimages;
            var Productitems = (await _itemServices.GetAllPagination(10, 1)).Entities;
            var itemimages = Productitems.Where(p => p.ProductId == id).Select(p => p.ItemImagestring).ToList();
            Productdetails.itemimages = itemimages;
            var itemcolor = Productitems.Where(p => p.ProductId == id).Select(i => i.Color).ToList();
            Productdetails.colors = itemcolor;
            return Ok(Productdetails);
        }
        [HttpGet("bycatogry")]
        public async Task<IActionResult> Getbycatogery(int catid)
        {
            var products = (await _productService.GetAllPagination(10, 1)).Entities;
            var productscatogery = products.Where(p => p.CategoryId == catid)
            .Select(p => new GetByCategory
            {
                Id = p.Id,
                categoryid = p.CategoryId,
                Name = p.Name
            }).ToList();
            return Ok(productscatogery);
        }
    }
}