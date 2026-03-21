using Application.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Controllers
{
    [ApiController]
    [Route("api/identity/users")]
    [Authorize]
    public class IdentityUserController : ControllerBase
    {
        private readonly IIdentityUserService _userService;

        public IdentityUserController(IIdentityUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Получить всех пользователей (только базовые данные)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllAsync();

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return Ok(result.Data);
        }

        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _userService.GetByIdAsync(id);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            if (result.Data is null)
                return NotFound(new { error = "Пользователь не найден" });

            return Ok(result.Data);
        }

        /// <summary>
        /// Получить текущего пользователя (из токена)
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userId, out var id))
                return Unauthorized(new { error = "Неправильный токен пользователя" });

            var result = await _userService.GetByIdAsync(id);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            if (result.Data is null)
                return NotFound(new { error = "Пользователь не найден" });

            return Ok(result.Data);
        }
    }
}
