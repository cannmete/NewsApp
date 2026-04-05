using AutoMapper;
using NewsApp.API.DTOs;
using NewsApp.API.Models;
using NewsApp.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsApp.API.DTOs;
using NewsApp.API.Models;
using NewsApp.API.Repositories;
using System.Security.Claims;

namespace NewsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly GenericRepository<Comment> _repository;
        private readonly IMapper _mapper;

        public CommentController(GenericRepository<Comment> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Sadece belirli bir habere ait yorumları getirme
        [HttpGet("ByNews/{newsId}")]
        public async Task<IActionResult> GetByNewsId(int newsId)
        {
            // İleride buraya "habere göre filtreleme" mantığı eklenecek, şimdilik tümünü çeken basit yapı
            var comments = await _repository.GetAllAsync();
            var newsComments = comments.Where(c => c.NewsId == newsId).ToList();

            // CommentDto'nuz olduğunu varsayarak (Yoksa DTO'yu eklememiz gerekir)
            // var dtos = _mapper.Map<List<CommentDto>>(newsComments);
            return Ok(newsComments);
        }

        [HttpPost]
        [Authorize] // Sadece giriş yapmış (Token'ı olan) kullanıcılar yorum yapabilir
        public async Task<IActionResult> Post(CommentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var comment = _mapper.Map<Comment>(dto);

            comment.AppUserId = userId; // Yorumu yapan kişiyi kaydediyoruz
            comment.CreatedDate = DateTime.Now;

            await _repository.AddAsync(comment);
            return Ok(new ResultDto { Status = true, Message = "Yorum başarıyla eklendi." });
        }

        [HttpDelete("{id}")]
        [Authorize] // Sisteme giriş yapmış herkes erişebilir (Rol kısıtlaması yok)
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _repository.GetByIdAsync(id);
            if (comment == null)
                return NotFound(new ResultDto { Status = false, Message = "Yorum bulunamadı." });

            // İsteği yapan kullanıcının kimliğini (ID) alıyoruz
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Kullanıcının yönetici (Admin) olup olmadığını sorguluyoruz
            bool isAdmin = User.IsInRole("Admin");

            // Eğer kullanıcı yönetici değilse VE yorumu kendisi yazmadıysa işlemi engelliyoruz
            if (!isAdmin && comment.AppUserId != currentUserId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResultDto { Status = false, Message = "Bu yorumu silme yetkiniz bulunmuyor." });
            }

            await _repository.DeleteAsync(comment);
            return Ok(new ResultDto { Status = true, Message = "Yorum başarıyla silindi." });
        }
    }
}