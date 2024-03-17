using Jumia.Application.Contract;
using Jumia.Context;
using Jumia.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.InfraStructure.Repository
{
    public class ReviewRepository : Repository<Review, int>, IReviewRepository
    {
        private readonly JumiaContext _context;

        public ReviewRepository(JumiaContext jumiaContext) : base(jumiaContext) { }

        // Add Review specific methods here
        public async Task<IEnumerable<Review>> GetByUserIdAsync(string userId)
        {
            return await _context.reviews.Where(r => r.UserID == userId).ToListAsync();
        }

        // Additional methods as needed...
    }
}
