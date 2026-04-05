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
    public class NewsController : ControllerBase
    {
        private readonly GenericRepository<News> _repository;
        private readonly IMapper _mapper;

        public NewsController(GenericRepository<News> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var news = await _repository.GetAllAsync();
            var dtos = _mapper.Map<List<NewsDto>>(news);
            return Ok(dtos);
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Post(NewsDto dto)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var news = _mapper.Map<News>(dto);
            news.AppUserId = userId;

            await _repository.AddAsync(news);
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
    }
}