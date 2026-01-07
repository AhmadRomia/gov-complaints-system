using Application.Common.Features.ComplsintUseCase.Queries;
using Application.Notifier.Core.Firebase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController(IFirebaseCoreService firebaseCoreService) : ControllerBase
    {

        [HttpGet("Subscribe")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Citizen")]
        public async Task<IActionResult> Subscribe([FromQuery] string FcmToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            await firebaseCoreService.SubscribeToTopic(userId, FcmToken);
            return Ok();
        }
          
    }
}
