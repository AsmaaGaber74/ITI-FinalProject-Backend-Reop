using Jumia.Application.Contract;
using Jumia.Context;
using Jumia.Model; // Ensure this is the correct namespace for your Payment entity
using System.Threading.Tasks;

namespace Jumia.InfraStructure.Repository
{
    public class PaymentRepository : Repository<Payment, int>, IPaymentReposatory
    {
        public PaymentRepository(JumiaContext jumiaContext) : base(jumiaContext)
        {
        }


    }
}