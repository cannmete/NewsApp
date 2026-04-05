using NewsApp.API.DTOs;

namespace NewsApp.API.DTOs
{
    public class NewsDto : BaseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImagePath { get; set; }
        public int ViewCount { get; set; }
        public int CategoryId { get; set; }
        public string AppUserId { get; set; }
    }
}