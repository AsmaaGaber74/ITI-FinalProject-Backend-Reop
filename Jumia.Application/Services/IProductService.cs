using Jumia.Dtos.ResultView;
using Jumia.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jumia.Dtos.ViewModel;

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

    }
}