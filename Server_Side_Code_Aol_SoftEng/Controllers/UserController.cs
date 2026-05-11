using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server_Side_Code_Aol_SoftEng.Models;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;

namespace Server_Side_Code_Aol_SoftEng.Controllers
{
    [Authorize(Roles = "Admin,Developer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> OnPost([FromBody] UserCreateModel requestBody)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => x.Value.Errors.First().ErrorMessage)
                    .FirstOrDefault();

                return BadRequest(new
                {
                    Message = firstError ?? "Validasi gagal."
                });
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(requestBody.Password, BCrypt.Net.BCrypt.GenerateSalt(12));

            await _userService.CreateUserAsync(requestBody, hashedPassword);

            return Ok(new { message = "User baru berhasil dibuat." });
        }
        //[HttpGet("auth-test")]
        //public IActionResult AuthTest() => Ok("Test Authorization");
    }
}
