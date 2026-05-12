using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.DTOs;
using NewsApp.API.Models;
using System.Security.Claims;

namespace NewsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] 
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public UserController(UserManager<AppUser> userManager, IMapper mapper, AppDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        // Tüm kullanıcıları listeleme
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            var userListDto = new List<UserDto>();

            foreach (var user in users)
            {
                // Her kullanıcı için Identity tablolarından rolleri sorgula
                var roles = await _userManager.GetRolesAsync(user);

                userListDto.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    // Eğer kullanıcının rolü varsa ilkini al, yoksa null bırak
                    RoleName = roles.FirstOrDefault()
                });
            }

            return Ok(userListDto);
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

        [HttpPost("AssignRole")]
        [Authorize(Roles = "Admin")] // Sadece adminler rol atayabilsin
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            // 1. Kullanıcıyı bul
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return NotFound(new { Status = false, Message = "Kullanıcı bulunamadı." });

            // 2. Rol atama sisteminde çakışma olmaması için önce kullanıcının mevcut rollerini temizliyoruz 
            // (Bir kullanıcının tek bir ana rolü olacağını varsayarak)
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // 3. Yeni seçilen rolü kullanıcıya ata
            var result = await _userManager.AddToRoleAsync(user, dto.RoleName);

            if (result.Succeeded)
            {
                return Ok(new { Status = true, Message = "Rol başarıyla atandı." });
            }

            return BadRequest(new { Status = false, Message = "Rol atama işlemi başarısız oldu." });
        }

        [HttpGet("Count")]
        public async Task<IActionResult> GetCount()
        {
            // Veritabanındaki toplam kullanıcı sayısını sayıp döndürüyoruz
            var count = await _context.Users.CountAsync();
            return Ok(count);
        }

        [HttpPut("UpdateProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound(new { Message = "Kullanıcı bulunamadı." });

            // DTO'daki yeni alanı AppUser nesnesine aktarıyoruz
            user.FullName = dto.FullName;
            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.ProfilePhotoPath = dto.ProfilePhotoPath; // Fotoğraf yolu güncelleniyor

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { Status = true, Message = "Profil bilgileri başarıyla güncellendi." });
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("Me")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile() 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            return Ok(new
            {
                user.FullName,
                user.UserName,
                user.Email,
                user.ProfilePhotoPath
            });
        }
    }
}