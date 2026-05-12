using NewsApp.API.DTOs;

namespace NewsApp.API.DTOs
{
    public class NewsDto : BaseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImagePath { get; set; } // Mevcut (Kapak Fotoğrafı)
        public string? ImagePath2 { get; set; }
        public string? ImagePath3 { get; set; }
        public string? ImagePath4 { get; set; }
        public string? ImagePath5 { get; set; }
        public int ViewCount { get; set; }
        public int CommentCount { get; set; }
        public double AverageRating { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? AppUserId { get; set; }
        public string? AppUserName { get; set; }
    }
}