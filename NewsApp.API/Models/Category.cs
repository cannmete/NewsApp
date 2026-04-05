using NewsApp.API.Models;

namespace NewsApp.API.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<News> NewsList { get; set; }
    }
}