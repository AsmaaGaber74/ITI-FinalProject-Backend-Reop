﻿using Jumia.Application.Contract;
using Jumia.Context;
using Jumia.model;
using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Application.Repository
{
    public class ProductRepository : Repository<Product, int>, IProductReposatory
    {
        public ProductRepository(JumiaContext jumiaContext) : base(jumiaContext)
        {

        }
    }
}
