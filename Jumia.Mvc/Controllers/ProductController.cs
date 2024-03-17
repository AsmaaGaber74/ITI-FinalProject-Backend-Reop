using Jumia.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Jumia.Dtos.ViewModel;
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
        private readonly IUserService userService;

        public ProductController(IProductService productService, IProductImageService productImageService, IUserService userService)
        {
            _proudectService = productService;
            _productImageService = productImageService;
            this.userService = userService;
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
            var sellers = await userService.GetAllUsersAsync(); // Assume this method exists and fetches all sellers

            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Sellers = new SelectList(sellers, "Id", "UserName"); // Assuming sellers are identified by Id and UserName

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(ProuductViewModel product)
        {
            if (ModelState.IsValid)
            {
                var result = await _proudectService.Create(product);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message;
                    // Repopulate the sellers and categories in case of failure so that the form can display them again
                    var categories = await _proudectService.GetAllCategories();
                    var sellers = await userService.GetAllUsersAsync(); // Same assumption as above
                    ViewBag.Categories = new SelectList(categories, "Id", "Name");
                    ViewBag.Sellers = new SelectList(sellers, "Id", "UserName");
                    return View(product);
                }
            }
            else
            {
                // The model state is not valid, so fetch the lists again to show the form with validation messages
                var categories = await _proudectService.GetAllCategories();
                var sellers = await userService.GetAllUsersAsync(); // Same assumption as above
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                ViewBag.Sellers = new SelectList(sellers, "Id", "UserName");
                return View(product);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var product = await _proudectService.GetOne(id);

            if (product == null)
            {
                return NotFound();
            }

            var categories = await _proudectService.GetAllCategories();
            var sellers = await userService.GetAllUsersAsync(); // Assuming userService can fetch all users

            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            ViewBag.Sellers = new SelectList(sellers, "Id", "UserName", product.SellerID);

            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, ProuductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _proudectService.Update(productViewModel);
                    if (result.IsSuccess)
                    {
                        TempData["SuccessMessage"] = result.Message;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = result.Message;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                }
            }

            // If we get to this point, it means something went wrong; reload categories and sellers
            var categories = await _proudectService.GetAllCategories();
            var sellers = await userService.GetAllUsersAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", productViewModel.CategoryId);
            ViewBag.Sellers = new SelectList(sellers, "Id", "UserName", productViewModel.SellerID);

            return View(productViewModel);
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