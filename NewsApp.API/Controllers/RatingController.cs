using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Models;
using System.Security.Claims;

namespace NewsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RatingController(AppDbContext context) { _context = context; }

        // Puan Ekle veya Güncelle (Sadece Giriş Yapanlar)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRating([FromBody] RatingRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Kullanıcı bu habere daha önce puan vermiş mi bakıyoruz
            var existingRating = await _context.Ratings.FirstOrDefaultAsync(r => r.NewsId == request.NewsId && r.AppUserId == userId);

            if (existingRating != null)
            {
                existingRating.Score = request.Score; // Verdiyse puanını güncelliyoruz
            }
            else
            {
                await _context.Ratings.AddAsync(new Rating
                {
                    NewsId = request.NewsId,
                    AppUserId = userId,
                    Score = request.Score
                });
            }

            await _context.SaveChangesAsync();
            return Ok(new { Status = true, Message = "Puanınız kaydedildi." });
        }
    }

    // Minik Veri Taşıyıcı
    public class RatingRequest { public int NewsId { get; set; } public int Score { get; set; } }
}