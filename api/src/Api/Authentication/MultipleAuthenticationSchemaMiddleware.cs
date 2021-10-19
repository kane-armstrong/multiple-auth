using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

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
            var principal = new ClaimsPrincipal();

            var identityServerResult = await context.AuthenticateAsync();
            if (identityServerResult?.Principal != null)
            {
                principal.AddIdentities(identityServerResult.Principal.Identities);
            }

            var firebaseResult = await context.AuthenticateAsync(AuthenticationConstants.FirebaseScheme);
            if (firebaseResult?.Principal != null)
            {
                principal.AddIdentities(firebaseResult.Principal.Identities);
            }

            context.User = principal;

            await _next(context);
        }
    }
}
