using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Models;

namespace NewsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemLogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SystemLogController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLogs()
        {
            // En son yapılan 30 hareketi tarihe göre tersten (en yeni en üstte) getir
            var logs = await _context.SystemLogs
                .OrderByDescending(x => x.Id)
                .Take(30)
                .ToListAsync();

            return Ok(logs);
        }
    }
}