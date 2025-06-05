using LibraryAPI.Data;
using LibraryAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;


namespace LibraryAPI.Middleware
{
    public class ObjectIdMiddleware
    {
        private readonly RequestDelegate _next;

        public ObjectIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, LibraryDbContext db, CacheService cache)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var oid = context.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
                var name = context.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                var email = context.User.Identity.Name;

                cache?.VerifyLoginAsync(oid, name, email);
            }
            await _next(context);
        }
    }
}
