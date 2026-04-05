using NewsApp.API.DTOs;
using NewsApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsApp.API.DTOs;
using NewsApp.API.Models;

namespace NewsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
                return BadRequest(new ResultDto { Status = false, Message = "Bu rol zaten mevcut!" });

            var result = await _roleManager.CreateAsync(new AppRole { Name = roleName });
            if (result.Succeeded)
                return Ok(new ResultDto { Status = true, Message = $"{roleName} rolü başarıyla oluşturuldu." });

            return StatusCode(StatusCodes.Status500InternalServerError, new ResultDto { Status = false, Message = "Rol oluşturulamadı." });
        }

        
        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser(string userName, string roleName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return NotFound(new ResultDto { Status = false, Message = "Kullanıcı bulunamadı." });

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                return NotFound(new ResultDto { Status = false, Message = "Böyle bir rol sistemde yok." });

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
                return Ok(new ResultDto { Status = true, Message = $"{userName} adlı kullanıcıya {roleName} yetkisi verildi." });

            return BadRequest(new ResultDto { Status = false, Message = "Rol ataması başarısız oldu." });
        }

        
        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }
    }
}