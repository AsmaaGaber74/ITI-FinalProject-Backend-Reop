using Jumia.Application.Contract;
using Jumia.Context;
using Jumia.Dtos.ResultView;
using Jumia.InfraStructure.Repository;
using Jumia.model;
using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.InfraStructure
{
    public class ProductRepository : Repository<Product, int>, IProductReposatory
    {
        private readonly JumiaContext _jumiacontext;

        public ProductRepository(JumiaContext jumiaContext) : base(jumiaContext)
        {
            _jumiacontext = jumiaContext;
        }

        public Task<IQueryable<Product>> SearchByCategoriey(int catid)
        {
            return Task.FromResult(_jumiacontext.products.Select(p => p).Where(p => p.CategoryID == catid));
        }

        public Task<IQueryable<Product>> SearchByName(string name)
        {
            return Task.FromResult(_jumiacontext.products.Where(p => p.NameEn.Contains(name) || p.DescriptionEn.Contains(name)));
        }
        public Task<IQueryable<Product>> SearchByNameAr(string name)
        {
            return Task.FromResult(_jumiacontext.products.Where(p => p.NameAr.Contains(name) || p.DescriptionAr.Contains(name)));
        }
        public Task<IQueryable<Product>> SearchByPrice(decimal minprice, decimal maxprice)
        {
            return Task.FromResult(_jumiacontext.products.Where(p => p.Price >= minprice && p.Price <= maxprice));
        }
    }
}