using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Models;
using System.Security.Claims;

namespace NewsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CommentController(AppDbContext context) { _context = context; }

        // 1. Habere Ait Onaylı Yorumları Getir
        [HttpGet("News/{newsId}")]
        public async Task<IActionResult> GetNewsComments(int newsId)
        {
            var comments = await _context.Comments
                .Include(c => c.AppUser)
                .Where(c => c.NewsId == newsId && c.IsApproved)
                .OrderByDescending(c => c.Id)
                .Select(c => new {
                    c.Id,
                    c.Text,
                    UserName = c.AppUser != null ? c.AppUser.UserName : "Anonim"
                }).ToListAsync();

            return Ok(comments);
        }

        // 2. Yeni Yorum Ekle (Sadece Giriş Yapanlar)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] CommentRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var comment = new Comment
            {
                NewsId = request.NewsId,
                Text = request.Text,
                AppUserId = userId,
                IsApproved = false 
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return Ok(new { Status = true, Message = "Yorumunuz başarıyla eklendi." });
        }

        // GET: api/Comment/MyComments
        [HttpGet("MyComments")]
        [Authorize] // Sadece giriş yapmış olmak yeterli
        public async Task<IActionResult> GetMyComments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var comments = await _context.Comments
                .Include(c => c.News)
                .Where(c => c.AppUserId == userId)
                .OrderByDescending(c => c.Id)
                .Select(c => new {
                    c.Id,
                    c.Text,
                    c.IsApproved,
                    NewsId = c.NewsId,
                    NewsTitle = c.News != null ? c.News.Title : "Silinmiş Haber"
                })
                .ToListAsync();

            return Ok(comments);
        }

        // GET: api/Comment/Pending (Onay Bekleyenleri Getir)
        [HttpGet("Pending")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingComments()
        {
            var comments = await _context.Comments
                .Include(c => c.News)
                .Include(c => c.AppUser)
                .Where(c => !c.IsApproved) // Sadece false olanlar
                .OrderByDescending(c => c.Id)
                .Select(c => new {
                    c.Id,
                    c.Text,
                    NewsTitle = c.News != null ? c.News.Title : "Silinmiş Haber",
                    UserName = c.AppUser != null ? c.AppUser.UserName : "Anonim"
                }).ToListAsync();

            return Ok(comments);
        }

        // PUT: api/Comment/Approve/5 (Yorumu Onayla)
        [HttpPut("Approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound(new { Message = "Yorum bulunamadı." });

            comment.IsApproved = true; // Onayı verdik!

            // 1. LOG KAYDI: Onaylama işlemi
            _context.SystemLogs.Add(new SystemLog
            {
                Action = "Yorum Onaylandı",
                Details = "Bekleyen bir yorum admin tarafından yayına alındı."
            });

            // Tek SaveChangesAsync ile hem yorumu onaylıyoruz hem de logu kaydediyoruz
            await _context.SaveChangesAsync();

            return Ok(new { Status = true, Message = "Yorum başarıyla yayına alındı." });
        }

        // DELETE: api/Comment/5 (Trol/Spam Yorumu Sil)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            _context.Comments.Remove(comment);

            // 2. LOG KAYDI: Silme işlemi
            _context.SystemLogs.Add(new SystemLog
            {
                Action = "Yorum Reddedildi",
                Details = "Uygunsuz görülen bir yorum sistemden silindi."
            });

            await _context.SaveChangesAsync();
            return Ok(new { Status = true, Message = "Yorum kalıcı olarak silindi." });
        }
    }

    // Minik Veri Taşıyıcı
    public class CommentRequest { public int NewsId { get; set; } public string Text { get; set; } }
}