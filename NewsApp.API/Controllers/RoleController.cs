using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Models;
using NewsApp.API.DTOs;

namespace NewsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // 1. ROLLERİ GETİR (AJAX GET isteği buraya düşer)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // MVC tarafındaki tablonun ID ve Name okuyabilmesi için ikisini de gönderiyoruz
            var roles = await _roleManager.Roles.Select(r => new { r.Id, r.Name }).ToListAsync();
            return Ok(roles);
        }

        // AJAX'tan gelen JSON verisini karşılamak için ufak bir sınıf
        public class RoleAddDto { public string Name { get; set; } }

        // 2. ROL EKLE (AJAX POST isteği buraya düşer)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleAddDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new ResultDto { Status = false, Message = "Rol adı boş olamaz." });

            var roleExists = await _roleManager.RoleExistsAsync(dto.Name);
            if (roleExists)
                return BadRequest(new ResultDto { Status = false, Message = "Bu rol zaten mevcut!" });

            var result = await _roleManager.CreateAsync(new AppRole { Name = dto.Name });
            if (result.Succeeded)
                return Ok(new ResultDto { Status = true, Message = "Rol başarıyla oluşturuldu." });

            return StatusCode(500, new ResultDto { Status = false, Message = "Rol oluşturulamadı." });
        }

        // 3. ROL SİL (AJAX DELETE isteği buraya düşer)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound(new ResultDto { Status = false, Message = "Rol bulunamadı." });

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return Ok(new ResultDto { Status = true, Message = "Rol başarıyla silindi." });

            return BadRequest(new ResultDto { Status = false, Message = "Silme işlemi başarısız oldu." });
        }
    }
}