using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server_Side_Code_Aol_SoftEng.Models;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;

namespace Server_Side_Code_Aol_SoftEng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserLoginService _userLoginService;
        public LoginController(IUserLoginService userLoginService)
        {
            _userLoginService = userLoginService;
        }
        [HttpPost]
        public async Task<IActionResult> OnPost([FromBody] UserLoginRequestModel requestBody)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => x.Value.Errors.First().ErrorMessage)
                    .FirstOrDefault();

                return BadRequest(new UserLoginResponseModel
                {
                    Message = firstError ?? "Validasi gagal."
                });
            }

            string HashedPassword = await _userLoginService.GetUserHashedPasswordAsync(requestBody.Username);

            if (string.IsNullOrEmpty(HashedPassword))
            {
                return Unauthorized(new UserLoginResponseModel
                {
                    Message = "Kredensial tidak valid."
                });
            }

            if (BCrypt.Net.BCrypt.Verify(requestBody.Password, HashedPassword))
            {
                // Generate JWT Token dan kirim balik
            }
            else
            {
                return Unauthorized(new UserLoginResponseModel
                {
                    Message = "Kredensial tidak valid."
                });
            }
        }
    }
}
