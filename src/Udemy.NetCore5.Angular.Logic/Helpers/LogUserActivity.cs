using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Udemy.NetCore5.Angular.Logic.Extensions;
using Udemy.NetCore5.Angular.Logic.Interfaces;

namespace Udemy.NetCore5.Angular.Logic.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next().ConfigureAwait(false);

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            var repository = resultContext.HttpContext.RequestServices.GetService<IAppUserRepository>();

            var userId = resultContext.HttpContext.User.GetUserId();
            var user = await repository.GetUserByIdAsync(userId).ConfigureAwait(false);
            user.LastActive = DateTime.UtcNow;
            await repository.SaveAllChangesAsync().ConfigureAwait(false);
        }
    }
}