using NewsApp.API.Models;

namespace NewsApp.API.Repositories
{
    public class NewsRepository : GenericRepository<News>
    {
        public NewsRepository(AppDbContext context) : base(context)
        {
        }
    }
}