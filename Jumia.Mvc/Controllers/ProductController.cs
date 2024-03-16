using Jumia.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Jumia.Context;
using Jumia.Dtos;
using Jumia.Dtos.ViewModel.Product;
namespace Jumia.Mvc.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductService _proudectService;
        private readonly IProductImageService _productImageService;

        public ProductController(IProductService productService, IProductImageService productImageService)
        {
            _proudectService = productService;
            _productImageService = productImageService;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var proudectsDataList = await _proudectService.GetAllPagination(100, 1);
            var proudects = proudectsDataList.Entities;

            if (!String.IsNullOrEmpty(searchString))
            {
                proudects = proudects.Where(p => p.Name.Contains(searchString)).ToList();
            }


            return View(proudects);
        }
        public async Task<ActionResult> Create()
        {
            var categories = await _proudectService.GetAllCategories();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            var sellers = await _proudectService.GetAllSellers();
            ViewBag.Sellers = new SelectList(sellers, "Id", "UserName");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(ProuductViewModel proudect)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var result = await _proudectService.Create(proudect);

                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction("Index");

                }
                else
                {
                    var categories = await _proudectService.GetAllCategories();
                    ViewBag.Categories = new SelectList(categories, "Id", "Name");
                    return View(proudect);


                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while creating the book.";
                return View(proudect);
            }
        }
        public async Task<ActionResult> Edit(int id)
        {
            var proudect = await _proudectService.GetOne(id);

            var categories = await _proudectService.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");


            var sellers = await _proudectService.GetAllSellers();
            ViewBag.Sellers = new SelectList(sellers, "Id", "UserName");

            return View(proudect);
        }
        [HttpPost]

        public async Task<ActionResult> Edit(int id, ProuductViewModel newproudect)
        {


            if (!ModelState.IsValid)
            {
                try
                {

                    var result = await _proudectService.Update(newproudect);
                    if (result.IsSuccess)
                    {
                        TempData["SuccessMessage"] = result.Message;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Error = result.Message;
                        return View(newproudect);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "An error occurred while updating the book.";
                    return View(newproudect);
                }
            }
            else
            {
                return View(newproudect);
            }

        }
        public async Task<ActionResult> Delete(int id)
        {
            var proudect = await _proudectService.GetOne(id);
            return View(proudect);
        }

        [HttpPost]

        public async Task<ActionResult> Delete(int id, ProuductViewModel deletedproudect)
        {
            var proudect = await _proudectService.GetOne(id);
            var result = await _proudectService.SoftDelete(proudect);
            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public ActionResult AssignImage(int productId)
        {
            ViewBag.ProductId = productId;
            return View();
        }

        // Action to save the assigned image
        [HttpPost]
        public async Task<ActionResult> AssignImage(int productId, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", fileName);

                // Ensure the directory exists
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Save the file to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                // Save the file path (relative to wwwroot) and product ID in your database
                var productImageDto = new ProductImageDto
                {
                    Path = $"/images/products/{fileName}", // Relative path to be used in your web app
                    ProductID = productId
                };

                await _productImageService.CreateAsync(productImageDto);
            }

            return RedirectToAction(nameof(DisplayImages), new { productId = productId });
        }

        [HttpGet]

        public async Task<ActionResult> DisplayImages(int productId)
        {
            var images = await _productImageService.GetByProductIdAsync(productId);
            return View(images);
        }


        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            // Retrieve the ProductImage by its ID
            var productImage = await _productImageService.GetByIdAsync(id);
            if (productImage == null)
            {
                TempData["ErrorMessage"] = "Image not found.";
                return RedirectToAction(nameof(DisplayImages)); // or handle the error appropriately
            }

            // Delete the image asynchronously
            bool result = await _productImageService.DeleteAsync(productImage.Id);

            if (result)
            {
                TempData["SuccessMessage"] = "Image marked as deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Image could not be marked as deleted.";
            }

            // Adjust redirection as needed
            return RedirectToAction(nameof(DisplayImages), new { productId = productImage.ProductID });
        }

    }
}