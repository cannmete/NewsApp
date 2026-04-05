using Microsoft.AspNetCore.Identity;

namespace NewsApp.API.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? ProfilePhotoPath { get; set; }

        public ICollection<News> NewsList { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}