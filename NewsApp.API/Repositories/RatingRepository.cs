using NewsApp.API.Models;
using NewsApp.API.Models;
using NewsApp.API.Repositories;
using System;

namespace NewsApp.API.Repositories
{
    public class RatingRepository : GenericRepository<Rating>
    {
        public RatingRepository(AppDbContext context) : base(context)
        {
        }
    }
}