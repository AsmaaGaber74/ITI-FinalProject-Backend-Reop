using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Application.Contract
{
    public interface IPaymentReposatory 
    {
        Task<Payment> Create(Payment payment);
        Task<int> SaveChanges();
    }
}
