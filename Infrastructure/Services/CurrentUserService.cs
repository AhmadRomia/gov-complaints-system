using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContext;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var idClaim = _httpContext.HttpContext?.User?.FindFirst("uid")?.Value
                              ?? _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Guid.TryParse(idClaim, out var id) ? id : (Guid?)null;
            }
        }

        public string? Email =>
            _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public string? UserName =>
            _httpContext.HttpContext?.User?.Identity?.Name;

        public bool IsAuthenticated =>
            _httpContext.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public async Task<bool> IsInRoleAsync(string role)
        {
            return _httpContext.HttpContext?.User?.IsInRole(role) ?? false;
        }
    }

}
