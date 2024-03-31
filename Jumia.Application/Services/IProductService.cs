using Jumia.Dtos.ResultView;
using Jumia.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jumia.Dtos.ViewModel.category;
using Jumia.Dtos.ViewModel.Product;
using Jumia.Dtos.ViewModel.User;
using Jumia.Model;

namespace Jumia.Application.Services
{
    public interface IProductService
    {
        //here the service functions 
        Task<ResultView<ProuductViewModel>> Create(ProuductViewModel proudect);


        Task<ResultView<ProuductViewModel>> SoftDelete(ProuductViewModel proudect);
        Task<ResultDataList<ProuductViewModel>> GetAllPagination(int items, int pagenumber);
        Task<ProuductViewModel> GetOne(int ID);

        Task<ResultView<ProuductViewModel>> Update(ProuductViewModel proudect);


        Task<List<CateogaryViewModel>> GetAllCategories();
        Task<List<LoginViewModel>> GetAllSellers();
        Task<int> SaveShanges();


        Task<ResultDataList<ProuductViewModel>> SearchByName(string name, int items, int pagenumber);
        Task<ResultDataList<ProuductViewModel>> SearchByPrice(decimal minprice, decimal maxprice, int items, int pagenumber);
        Task<ResultDataList<ProuductViewModel>> SearchByCategoriey(int catid, int items, int pagenumber);
        Task<ResultDataList<ProuductViewModel>> SearchByBrand(string name, int items, int pagenumber);
        Task<List<string>> GetAllBrands();


    }
}