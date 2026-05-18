using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server_Side_Code_Aol_SoftEng.Models;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;

namespace Server_Side_Code_Aol_SoftEng.Controllers
{
    //[Authorize(Roles = "Admin,Developer,Cashier")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpPost]
        public async Task<IActionResult> OnPost([FromBody] TransactionHeaderCreateDto requestBody)
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

            var username = User.Identity?.Name;

            await _transactionService.ProcessAddTransactionAsync(requestBody, username);

            return Ok(new
            {
                Message = "Berhasil menyimpan transaksi."
            });
        }
        [HttpGet]
        public async Task<IActionResult> OnGetAll()
        {

        }
    }
}
