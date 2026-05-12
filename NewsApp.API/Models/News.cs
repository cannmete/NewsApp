using NewsApp.API.Models;

namespace NewsApp.API.Models
{
    public class News : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImagePath { get; set; } // Mevcut (Kapak Fotoğrafı)
        public string? ImagePath2 { get; set; }
        public string? ImagePath3 { get; set; }
        public string? ImagePath4 { get; set; }
        public string? ImagePath5 { get; set; }
        public int ViewCount { get; set; } = 0;

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}