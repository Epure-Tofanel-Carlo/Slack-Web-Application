using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SlackDAW1.Data;
using SlackDAW1.Models;
using System.Threading.Tasks;

namespace SlackDAW1
{

    public class UserChannelsMiddleware
    {
        private readonly RequestDelegate _next;

        public UserChannelsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            
            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                if (context.User.Identity.IsAuthenticated)
                {
                    var userChannels = db.UserChannels
                        .Where(uc => uc.UserID == userManager.GetUserId(context.User))
                        .Include(c => c.Channel.Category)
                        .Select(c => c.Channel)
                        .ToList()
                        ;

                    context.Items["UserChannels"] = userChannels;
                }

                await _next(context);
            }
        }
    }


    public static class UserChannelsMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserChannelsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserChannelsMiddleware>();
        }
    }
}
