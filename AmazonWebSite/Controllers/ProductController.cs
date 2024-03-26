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
        private readonly IConfiguration configuration;

        public ProductController(IProductService productService, IItemServices itemServices, IProductImageService productImageService, IConfiguration configuration)
        {
            _productService = productService;
            _itemServices = itemServices;
            _productImageService = productImageService;
            this.configuration = configuration;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllPagination(10, 1);
            var items = await _itemServices.GetAllPagination(10, 1);
            var productsDTO = products.Entities.Select(p => new GetAllPaginationUser
            {
                Price = p.Price,
                Description = p.Description,
                Name = p.Name,
                id = p.Id,
                ProductImages = new List<string>(), // Initialize here to ensure it's not null
                itemscolor = new List<string>() // Assuming you'll populate this similarly
            }).ToList();

            var basePath = configuration.GetValue<string>("MvcProject:WwwRootPath");

            foreach (var item in productsDTO)
            {
                var productImagePaths = (await _productImageService.GetByProductIdAsync(item.id)).Select(p => p.Path).ToList();

                foreach (var imagePath in productImagePaths)
                {
                    try
                    {
                        var fullPath = Path.Combine(basePath, imagePath.Replace("/", "\\").TrimStart('\\'));
                        if (System.IO.File.Exists(fullPath))
                        {
                            var imageBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
                            var base64String = Convert.ToBase64String(imageBytes);
                            item.ProductImages.Add(base64String);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error
                        // Consider how to handle errors; skipping image in this example
                    }
                }

                // Fetch and assign colors; assuming this is done correctly above
                var productColors = items.Entities.Where(p => p.ProductId == item.id).Select(p => p.Color).ToList();
                item.itemscolor = productColors;
            }

            return Ok(productsDTO);
        }


        [HttpGet]
        public async Task<IActionResult> GetOne(int id)
        {
            var product = await _productService.GetOne(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var productDTO = new GetOneUser
            {

                price = product.Price,
                Name = product.Name,
                ProductDescription = product.Description,
                Id = product.Id,
                BrandName = product.BrandName,
                Productimages = new List<string>(), // Initialize here to ensure it's not null
                itemimages = new List<string>(), // Assuming similar adjustment needed
                colors = new List<string>() // Initialize here; assuming you'll populate this similarly
            };

            var basePath = configuration.GetValue<string>("MvcProject:WwwRootPath");
            var productImagePaths = (await _productImageService.GetByProductIdAsync(id)).Select(p => p.Path).ToList();

            foreach (var imagePath in productImagePaths)
            {
                try
                {
                    var fullPath = Path.Combine(basePath, imagePath.Replace("/", "\\").TrimStart('\\'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        var imageBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
                        var base64String = Convert.ToBase64String(imageBytes);
                        productDTO.Productimages.Add(base64String);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            // Assuming there's a method to fetch items related to this product
            // For item images and colors, apply similar logic as needed based on your model
            //var items = await _itemServices.GetOne(id); // This method needs to be defined or adjusted according to your actual service layer
            //var itemColors = items.Select(i => i.Color).ToList();
            //productDTO.colors = itemColors;




            return Ok(productDTO);
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
        [HttpGet("searchname")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var Products = await _productService.SearchByName(name, 10, 1);
            return Ok(Products);
        }
        [HttpGet("searchbycategory")]
        public async Task<IActionResult> SearchByCategory(int catid)
        {
            var products = await _productService.SearchByCategoriey(catid, 10, 1);
            return Ok(products);
        }
        [HttpGet("searhbyprice")]
        public async Task<IActionResult> SearchByPrice(int minprice, int maxprice)
        {
            var peoducts = await _productService.SearchByPrice(minprice, maxprice, 10, 1);
            return Ok(peoducts);
        }

    }
}