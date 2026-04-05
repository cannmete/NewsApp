using AutoMapper;
using NewsApp.API.DTOs;
using NewsApp.API.Models;
using NewsApp.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public CategoryController(GenericRepository<Category> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
    }
}