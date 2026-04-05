using NewsApp.API.Models;
using NewsApp.API.Models;
using NewsApp.API.Repositories;
using System;

namespace NewsApp.API.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}