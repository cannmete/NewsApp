using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.DTOs;
using NewsApp.API.Models;
using NewsApp.API.Repositories;

namespace NewsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly GenericRepository<Category> _repository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public CategoryController(GenericRepository<Category> repository, IMapper mapper, AppDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _repository.GetAllAsync();
            var dtos = _mapper.Map<List<CategoryDto>>(categories);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return NotFound(new ResultDto { Status = false, Message = "Kategori bulunamadı." });

            var dto = _mapper.Map<CategoryDto>(category);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(CategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _repository.AddAsync(category);
            return Ok(new ResultDto { Status = true, Message = "Kategori başarıyla eklendi." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return NotFound();

            await _repository.DeleteAsync(category);
            return Ok(new ResultDto { Status = true, Message = "Kategori silindi." });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, CategoryDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ResultDto { Status = false, Message = "Kimlik uyuşmazlığı." });

            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return NotFound(new ResultDto { Status = false, Message = "Kategori bulunamadı." });

            // AutoMapper ile DTO'dan gelen yeni verileri, EF Core'un izlediği (tracked) mevcut nesneye aktarıyoruz
            _mapper.Map(dto, category);

            // BaseEntity'den gelen güncellenme tarihini sistemsel olarak atıyoruz
            category.UpdatedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(category);
            return Ok(new ResultDto { Status = true, Message = "Kategori başarıyla güncellendi." });
        }

        // GET: api/Category/Count
        [HttpGet("Count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _context.Categories.CountAsync();
            return Ok(count);
        }
    }
}