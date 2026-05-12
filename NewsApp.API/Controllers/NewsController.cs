using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.DTOs;
using NewsApp.API.Models;
using NewsApp.API.Repositories;
using System.Security.Claims;

namespace NewsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly GenericRepository<News> _repository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public NewsController(GenericRepository<News> repository, IMapper mapper, AppDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }
        [HttpGet]
        // Parametrelere categoryId eklendi!
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] int? categoryId)
        {
            var newsQuery = _context.News
                .Include(x => x.AppUser)
                .Include(x => x.Category)
                .AsQueryable();

            // 1. KATEGORİ FİLTRESİ (Eğer kategori seçildiyse sadece o kategori gelsin)
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                newsQuery = newsQuery.Where(x => x.CategoryId == categoryId.Value);
            }

            // 2. ARAMA (SEARCH) FİLTRESİ
            if (!string.IsNullOrEmpty(search))
            {
                newsQuery = newsQuery.Where(x => x.Title.Contains(search) || x.Content.Contains(search));
            }

            var newsList = await newsQuery.ToListAsync();
            var dtos = _mapper.Map<List<NewsDto>>(newsList);

            // YORUM VE PUAN HESAPLAMALARI
            foreach (var dto in dtos)
            {
                dto.CommentCount = await _context.Comments.CountAsync(c => c.NewsId == dto.Id && c.IsApproved);
                var hasRating = await _context.Ratings.AnyAsync(r => r.NewsId == dto.Id);
                dto.AverageRating = hasRating ? Math.Round(await _context.Ratings.Where(r => r.NewsId == dto.Id).AverageAsync(r => r.Score), 1) : 0;
            }

            // SIRALAMA (SORTING) İŞLEMİ
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "views": dtos = dtos.OrderByDescending(x => x.ViewCount).ToList(); break;
                    case "comments": dtos = dtos.OrderByDescending(x => x.CommentCount).ToList(); break;
                    case "rating": dtos = dtos.OrderByDescending(x => x.AverageRating).ToList(); break;
                    default: dtos = dtos.OrderByDescending(x => x.Id).ToList(); break;
                }
            }
            else { dtos = dtos.OrderByDescending(x => x.Id).ToList(); }

            return Ok(dtos);
        }

        [HttpPost]
        [Authorize(Roles = "Writer,Admin")] // (Gerekirse Admin'i de ekleyebilirsin)
        public async Task<IActionResult> Post(NewsDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var news = _mapper.Map<News>(dto);
            news.AppUserId = userId;
            news.Category = null;
            news.AppUser = null;

            await _repository.AddAsync(news);
            _context.SystemLogs.Add(new SystemLog
            {
                Action = "Haber Yayımlandı",
                Details = $"'{dto.Title}' başlıklı haber paylaşıldı."
            });
            await _context.SaveChangesAsync();
            return Ok(new ResultDto { Status = true, Message = "Haber başarıyla eklendi." });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Yazar")] // Admin veya yazar güncelleyebilir
        public async Task<IActionResult> Put(int id, NewsDto dto)
        {
            var news = await _repository.GetByIdAsync(id);
            if (news == null) return NotFound(new ResultDto { Status = false, Message = "Haber bulunamadı." });

            // Sadece başlık ve içeriği güncelliyoruz
            news.Title = dto.Title;
            news.Content = dto.Content;
            news.CategoryId = dto.CategoryId;
            news.UpdatedDate = DateTime.Now;

            await _repository.UpdateAsync(news);
            return Ok(new ResultDto { Status = true, Message = "Haber başarıyla güncellendi." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Sadece Admin silebilir
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _repository.GetByIdAsync(id);
            if (news == null) return NotFound();

            await _repository.DeleteAsync(news);
            return Ok(new ResultDto { Status = true, Message = "Haber sistemden silindi." });
        }

        [HttpGet("Count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _context.News.CountAsync();
            return Ok(count);
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var news = await _context.News
                .Include(x => x.AppUser)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (news == null)
                return NotFound(new { Status = false, Message = "Haber bulunamadı." });

            var dto = _mapper.Map<NewsDto>(news);

            // Habere tıklandığında okunma sayısını 1 artırıyoruz.
            news.ViewCount += 1;
            await _context.SaveChangesAsync();

            return Ok(dto);
        }

        // POST: api/News/Upload
        [HttpPost("Upload")]
        // [Authorize(Roles = "Writer,Admin")] // Test aşamasında yetki sorunu yaşamamak için geçici olarak kapatabilirsin
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new { Message = "Dosya seçilmedi veya boş geldi." });

                var extension = Path.GetExtension(file.FileName);
                var newFileName = Guid.NewGuid().ToString() + extension;

                // wwwroot/uploads klasörünün yolunu belirliyoruz
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Klasör yoksa oluşturuyoruz
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var exactPath = Path.Combine(folderPath, newFileName);

                using (var stream = new FileStream(exactPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var imageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{newFileName}";

                return Ok(new { url = imageUrl });
            }
            catch (Exception ex)
            {
                // Sistem çökmek yerine bize hatayı söyleyecek
                return StatusCode(500, new { Message = "Sunucuda hata oluştu: " + ex.Message });
            }


        }

        // GET: api/News/MyNews
        [HttpGet("MyNews")]
        [Authorize(Roles = "Writer,Admin")]
        public async Task<IActionResult> GetMyNews()
        {
            // Token'dan login olmuş kullanıcının ID'sini yakalıyoruz
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var news = await _context.News
                .Include(x => x.Category)
                .Where(x => x.AppUserId == userId)
                .OrderByDescending(x => x.Id)
                .Select(x => new {
                    x.Id,
                    x.Title,
                    x.ViewCount,
                    CategoryName = x.Category != null ? x.Category.Name : "Genel"
                })
                .ToListAsync();

            return Ok(news);
        }
    }
}