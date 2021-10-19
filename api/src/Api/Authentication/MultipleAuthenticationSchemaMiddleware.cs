using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Authentication
{
    public class MultipleAuthenticationSchemaMiddleware
    {
        private readonly RequestDelegate _next;

        public MultipleAuthenticationSchemaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var result = await Authenticate(context);

            if (result.Succeeded)
            {
                context.User = new ClaimsPrincipal(result.Principal.Identities);
            }

            await _next(context);
        }

        private static async Task<AuthenticateResult> Authenticate(HttpContext context)
        {
            var identityServerResult = await context.AuthenticateAsync();
            if (identityServerResult.Succeeded)
            {
                return identityServerResult;
            }

            var firebaseResult = await context.AuthenticateAsync(AuthenticationConstants.FirebaseScheme);
            return firebaseResult;
        }
    }
}
