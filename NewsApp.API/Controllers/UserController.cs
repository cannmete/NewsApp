using AutoMapper;
using NewsApp.API.DTOs;
using NewsApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NewsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] 
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        // Tüm kullanıcıları listeleme
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            var dtos = _mapper.Map<List<UserDto>>(users);
            return Ok(dtos);
        }

        // ID'ye göre tek bir kullanıcıyı getirme
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(new ResultDto { Status = false, Message = "Kullanıcı bulunamadı." });

            var dto = _mapper.Map<UserDto>(user);
            return Ok(dto);
        }

        // Kullanıcı bilgilerini güncelleme
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(new ResultDto { Status = false, Message = "Kullanıcı bulunamadı." });

            // Sadece izin verdiğimiz alanları güncelliyoruz
            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.UserName = dto.UserName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(new ResultDto { Status = true, Message = "Kullanıcı başarıyla güncellendi." });

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest(new ResultDto { Status = false, Message = $"Hata: {errors}" });
        }

        // Kullanıcıyı tamamen silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(new ResultDto { Status = false, Message = "Kullanıcı bulunamadı." });

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Ok(new ResultDto { Status = true, Message = "Kullanıcı başarıyla silindi." });

            return BadRequest(new ResultDto { Status = false, Message = "Kullanıcı silinemedi." });
        }
    }
}