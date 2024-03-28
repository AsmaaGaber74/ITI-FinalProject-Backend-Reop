using Jumia.Application.Contract;
using Jumia.Context;
using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.InfraStructure.Repository
{
    public class PaymentRepository : IPaymentReposatory
    {
        private readonly JumiaContext _context;

        public PaymentRepository(JumiaContext context)
        {
            _context = context;
        }
        public async Task<Payment> Create(Payment payment)
        {
            return (await _context.payments.AddAsync(payment)).Entity;
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
