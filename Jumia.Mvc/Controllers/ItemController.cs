using Jumia.Application.Services;
using Jumia.Dtos.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Jumia.Mvc.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemServices _itemServices;

        public ItemController(IItemServices itemServices) 
        { 
            _itemServices = itemServices;
        }
        public async Task<IActionResult> Index()
        {
            var items = await _itemServices.GetAllPagination(10,1);
            return View(items);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ItemViewModel itemView)
        {
            try
            {
                var ProductId = await _itemServices.GetProductID(itemView.ProductName);
                itemView.ProductId = ProductId;
                var Result = await _itemServices.Create(itemView);
                if(Result.Entity == null)
                {
                    ViewBag.Error = Result.Message;
                    return View(itemView);
                }
                else
                {
                    return RedirectToAction("index","item");
                }
            }
            catch
            {
                return View();
            }
        } 
        public async Task<IActionResult> Update(int id)
        {
            var item = await _itemServices.GetOne(id);
            var productname = await _itemServices.GetProductName(item.Entity.ProductId);
            item.Entity.ProductName = productname;
            return View(item.Entity);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ItemViewModel item , int id)
        {
            try
            {
                var result = await _itemServices.Update(item);
                if(result.Entity == null)
                {
                    ViewBag.Error = result.Message;
                    return View(item);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch 
            { 
                return View(item);
            }

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _itemServices.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
