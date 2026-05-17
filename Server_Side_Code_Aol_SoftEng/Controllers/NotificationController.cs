using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server_Side_Code_Aol_SoftEng.Models;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;

namespace Server_Side_Code_Aol_SoftEng.Controllers
{
    [Authorize(Roles = "Developer,Admin,Cashier")]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet("Products")]
        public async Task<IActionResult> OnGetProductsNotification()
        {
            List<ProductNotificationDto> productNotifications = await _notificationService.GetProductNotificationAsync();

            if (productNotifications.Count == 0) return NoContent();
            return Ok(productNotifications);
        }
    }
}
