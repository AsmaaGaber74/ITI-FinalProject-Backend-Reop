using AutoMapper;
using Jumia.Application.Contract;
using Jumia.Dtos;
using Jumia.Dtos.ResultView;
using Jumia.Application.Services;
using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Jumia.Dtos.ViewModel.category;
using Jumia.Dtos.ViewModel.Product;
using Jumia.Dtos.ViewModel.User;
namespace Jumia.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductReposatory productReposatory;
        private readonly IMapper _mapper;
        private readonly ICategoryReposatory categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProductService(IProductReposatory productReposatory, IMapper mapper, ICategoryReposatory categoryRepository, UserManager<ApplicationUser> userManager)
        {
            this.productReposatory = productReposatory;
            _mapper = mapper;
            this.categoryRepository = categoryRepository;
            _userManager = userManager;
        }

        public async Task<ResultView<ProuductViewModel>> Create(ProuductViewModel product)
        {
            var Query = (await productReposatory.GetAllAsync());
            var OldProduct = Query.Where(p => p.Id == product.Id).FirstOrDefault();
            if (OldProduct != null)
            {
                return new ResultView<ProuductViewModel> { Entity = null, IsSuccess = false, Message = "Already Exist" };
            }
            else
            {
                var Prd = _mapper.Map<Product>(product);
                var NewPrd = await productReposatory.CreateAsync(Prd);
                await productReposatory.SaveChangesAsync();
                var PrdDto = _mapper.Map<ProuductViewModel>(NewPrd);
                return new ResultView<ProuductViewModel> { Entity = PrdDto, IsSuccess = true, Message = "Created Successfully" };
            }

        }
        public async Task<ResultView<ProuductViewModel>> SoftDelete(ProuductViewModel product)
        {
            try
            {
                var PRd = _mapper.Map<Product>(product);
                var Oldprd = (await productReposatory.GetAllAsync()).FirstOrDefault(p => p.Id == product.Id);
                Oldprd.IsDeleted = true;
                await productReposatory.SaveChangesAsync();
                var PrdDto = _mapper.Map<ProuductViewModel>(Oldprd);
                return new ResultView<ProuductViewModel> { Entity = PrdDto, IsSuccess = true, Message = "Deleted Successfully" };
            }
            catch (Exception ex)
            {
                return new ResultView<ProuductViewModel> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }
        public async Task<ResultView<ProuductViewModel>> Update(ProuductViewModel proudect)
        {
            try
            {
                var uproudect = await productReposatory.GetByIdAsync(proudect.Id);
                uproudect.NameAr = proudect.NameAr;
                uproudect.NameEn = proudect.NameEn;
                uproudect.BrandNameAr = proudect.BrandNameAr;
                uproudect.BrandNameEn = proudect.BrandNameEn;

                uproudect.Price = proudect.Price;
                uproudect.StockQuantity = proudect.StockQuantity;
                uproudect.DescriptionAr = proudect.DescriptionAR;
                uproudect.DescriptionEn = proudect.DescriptionEn;
                uproudect.DateListed = proudect.DateListed;
                uproudect.CategoryID = proudect.CategoryId;
                uproudect.SellerID = proudect.SellerID;



                await productReposatory.SaveChangesAsync();


                var updateProudectDto = _mapper.Map<ProuductViewModel>(uproudect);
                return new ResultView<ProuductViewModel> { Entity = updateProudectDto, IsSuccess = true, Message = "Book updated successfully." };
            }
            catch (Exception ex)
            {
                return new ResultView<ProuductViewModel> { Entity = null, IsSuccess = false, Message = ex.Message };
            }
        }


        public async Task<ProuductViewModel> GetOne(int ID)
        {
            var product = await productReposatory.GetByIdAsync(ID);
            var REturnproudect = _mapper.Map<ProuductViewModel>(product);
            return REturnproudect;
        }

        public async Task<ResultDataList<ProuductViewModel>> GetAllPagination(int items, int pagenumber)
        {
            var AlldAta = (await productReposatory.GetAllAsync());
            var activeProducts = AlldAta.Where(p => p.IsDeleted == false);
            var proudects = activeProducts.Skip(items * (pagenumber - 1)).Take(items).Select(p => new ProuductViewModel()
            {
                Id = p.Id,
                NameAr = p.NameAr,
                NameEn = p.NameEn,
                BrandNameAr = p.BrandNameAr,
                BrandNameEn = p.BrandNameEn,
                CategoryNameAr = p.Category.NameEn,
                CategoryNameEn = p.Category.NameEn,
                
                StockQuantity = p.StockQuantity,
                Price = p.Price,
                DateListed = p.DateListed,
                DescriptionAR = p.DescriptionAr,
                DescriptionEn = p.DescriptionEn,
                SellerName = p.Seller.UserName,
                CategoryId = p.Category.Id,
               
                // IsDeleted=p.IsDeleted,
                // ImgPath = p.ProductImages.FirstOrDefault().Path
            }).ToList();
            ResultDataList<ProuductViewModel> resultDataList = new ResultDataList<ProuductViewModel>();
            resultDataList.Entities = proudects;
            resultDataList.Count = AlldAta.Count();
            return resultDataList;
        }

        public async Task<ResultDataList<ProuductViewModel>> GetAllPaginatedByCategoryId(int categoryId, int items, int pageNumber)
        {
            // Fetch all products (consider fetching only necessary fields to improve performance)
            var query = await productReposatory.GetAllAsync();

            // Filter by category
            var filteredQuery = query.Where(p => p.CategoryID == categoryId && !p.IsDeleted);

            // Apply pagination
            var paginatedProducts = filteredQuery.Skip((pageNumber - 1) * items).Take(items);

            // Map to view model
            var productViewModels = _mapper.Map<List<ProuductViewModel>>(paginatedProducts);

            // Return result
            return new ResultDataList<ProuductViewModel>
            {
                Entities = productViewModels,
                Count = filteredQuery.Count() // Total count before pagination
            };
        }


        public async Task<List<CateogaryViewModel>> GetAllCategories()
        {
            try
            {
                var categories = await categoryRepository.GetAllAsync();
                var categoryViewModels = _mapper.Map<List<CateogaryViewModel>>(categories);
                return categoryViewModels;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<LoginViewModel>> GetAllSellers()
        {
            var sellers = await _userManager.GetUsersInRoleAsync("Seller");
            var sellerViewModels = _mapper.Map<List<LoginViewModel>>(sellers);
            return sellerViewModels;
        }

        public async Task<ResultDataList<ProuductViewModel>> SearchByName(string name, int items, int pagenumber)
        {
            var Products = (await productReposatory.SearchByName(name)).Skip(items * (pagenumber - 1)).Take(items);
            var productsviewmodel = _mapper.Map<List<ProuductViewModel>>(Products);
            return new ResultDataList<ProuductViewModel> { Entities = productsviewmodel.ToList(), Count = productsviewmodel.Count() };
        }

        public async Task<ResultDataList<ProuductViewModel>> SearchByPrice(decimal minprice, decimal maxprice, int items, int pagenumber)
        {
            var Products = (await productReposatory.SearchByPrice(minprice, maxprice)).Skip(items * (pagenumber - 1)).Take(items);
            var productsviewmodel = _mapper.Map<List<ProuductViewModel>>(Products);
            return new ResultDataList<ProuductViewModel> { Entities = productsviewmodel.ToList(), Count = productsviewmodel.Count() };
        }

        public async Task<ResultDataList<ProuductViewModel>> SearchByCategoriey(int catid, int items, int pagenumber)
        {
            var Products = (await productReposatory.SearchByCategoriey(catid)).Skip(items * (pagenumber - 1)).Take(items);
            var productsviewmodel = _mapper.Map<List<ProuductViewModel>>(Products);
            return new ResultDataList<ProuductViewModel> { Entities = productsviewmodel.ToList(), Count = productsviewmodel.Count() };
        }

        public async Task<int> SaveShanges()
        {
           return await productReposatory.SaveChangesAsync();
        }


        public async Task<ResultDataList<ProuductViewModel>> SearchByBrand(string name, int items, int pagenumber)
        {
            var Products = (await productReposatory.SearchByBrand(name)).Skip(items * (pagenumber - 1)).Take(items);
            var productsviewmodel = _mapper.Map<List<ProuductViewModel>>(Products);
            return new ResultDataList<ProuductViewModel> { Entities = productsviewmodel.ToList(), Count = productsviewmodel.Count() };
        }
        public async Task<List<string>> GetAllBrands()
        {
            try
            {
                var products = await productReposatory.GetAllAsync();
                var brandNames = products.Select(p => p.BrandNameEn).Distinct().ToList();

                return brandNames;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<ProuductViewModel>> GetByCategory(int categoryId)
        {
            var products = await productReposatory.GetAllAsync();
            return products.Where(p => p.CategoryID == categoryId)
                           .Select(p => _mapper.Map<ProuductViewModel>(p))
                           .ToList();
        }
    }
}

