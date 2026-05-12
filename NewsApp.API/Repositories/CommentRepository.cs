using NewsApp.API.Models;
using NewsApp.API.Repositories;
using System;

namespace NewsApp.API.Repositories
{
    public class CommentRepository : GenericRepository<Comment>
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }
    }
}