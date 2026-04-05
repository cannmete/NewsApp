using NewsApp.API.Models;

namespace NewsApp.API.Models
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; }
        public bool IsApproved { get; set; } = false;

        public int NewsId { get; set; }
        public News News { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}