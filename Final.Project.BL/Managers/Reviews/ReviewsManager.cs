﻿
using Final.Project.DAL;

namespace Final.Project.BL;

public class ReviewsManager:IReviewsManager
{
    private readonly IUnitOfWork _unitOfWork;

    public ReviewsManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #region AddReview

    public void AddReview(string userId, int productId, string comment, int rating)
    {
        Review newReview = new Review
        {
            UserId = userId,
            ProductId = productId,
            Comment = comment,
            CreationDate = DateTime.Now,
            Rating = rating, 
             
             
             
        };

        _unitOfWork.ReviewRepo.Add(newReview);
        _unitOfWork.Savechanges();
    }

    #endregion

    #region Get All Review

    public IEnumerable<ReviewReadDto> GetAllReviews()
    {
        IEnumerable<Review> ReviewsFromDb = _unitOfWork.ReviewRepo.GetReviewsWithProductAndUser();
        IEnumerable<ReviewReadDto> reviewReadDtos = ReviewsFromDb.Select(r => new ReviewReadDto()
        {
            ProductId = r.ProductId,
            ProductName = r.Product.Name,
            UserId = r.UserId,
            UserName = r.User.FName + " " + r.User.LName,
            Comment = r.Comment,
            Rating = r.Rating,
            CreationDate = r.CreationDate,
        });
        return reviewReadDtos;
    }

    #endregion

    #region Get Average Rating

    public double GetAverageRating(int productId)
    {
        var reviews = _unitOfWork.ReviewRepo.GetReviewsByProduct(productId);
        return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
    }

    #endregion

    #region Delete Review

    public bool DeleteReview(ReviewKeyDto reviewKey)
    {
        var review = _unitOfWork.ReviewRepo.GetByCompositeId(reviewKey.ProductId, reviewKey.UserId);
        if (review is null)
        {
            return false;
        }

        _unitOfWork.ReviewRepo.Delete(review);
        return _unitOfWork.Savechanges() > 0;
    }

    #endregion

}
