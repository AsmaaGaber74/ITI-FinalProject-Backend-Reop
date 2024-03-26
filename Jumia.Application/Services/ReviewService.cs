using Jumia.Application.Contract;
using Jumia.Dtos.ViewModel.Review;
using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<ReviewUserDTO> AddReviewAsync(ReviewUserDTO reviewDto)
        {
            var review = new Review
            {
                // Map DTO to Review entity
                ProductID = reviewDto.ProductID.Value,
                UserID = reviewDto.UserID,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                DatePosted = DateTime.UtcNow // Assuming UTC for example
            };

            await _reviewRepository.CreateAsync(review);
            await _reviewRepository.SaveChangesAsync();

            return reviewDto; // Or map the created entity back to DTO
        }

        public async Task<List<ReviewAdminDTO>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync(); // Assume this method exists and fetches all reviews
            var reviewDtos = reviews.Select(r => new ReviewAdminDTO
            {
                Id = r.Id,
                ProductID = r.ProductID,
                UserID = r.UserID,
                Rating = r.Rating,
                Comment = r.Comment,
                DatePosted = r.DatePosted
            }).ToList();

            return reviewDtos;
        }
        public async Task<ReviewUserDTO> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id); // Assume GetByIdAsync exists in the repository
            if (review == null)
            {
                return null; // Or handle the null case as per your application's needs
            }

            var reviewDto = new ReviewUserDTO
            {
                Id = review.Id,
                ProductID = review.ProductID,
                UserID = review.UserID,
                Rating = review.Rating,
                Comment = review.Comment,
                DatePosted = review.DatePosted
            };

            return reviewDto;
        }
        public async Task<List<ReviewUserDTO>> GetReviewsByProductIdAsync(int productId)
        {
            var reviews = await _reviewRepository.GetByProductIdAsync(productId);
            var reviewDtos = reviews.Select(r => new ReviewUserDTO
            {
                Id = r.Id,
                ProductID = r.ProductID,
                UserID = r.UserID,
                Rating = r.Rating,
                Comment = r.Comment,
                DatePosted = r.DatePosted
            }).ToList();

            return reviewDtos;
        }
    }
}
