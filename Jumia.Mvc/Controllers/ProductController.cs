using Jumia.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Jumia.Dtos.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Jumia.Context;
namespace Jumia.Mvc.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductService _proudectService;

        public ProductController(IProductService proudectService)
        {
            _proudectService = proudectService;

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
    }
}