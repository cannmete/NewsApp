using NewsApp.API.Models;

namespace NewsApp.API.Models
{
    public class Rating : BaseEntity
    {
        public int Score { get; set; } // Örn: 1-5 arası

        public int NewsId { get; set; }
        public News News { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}