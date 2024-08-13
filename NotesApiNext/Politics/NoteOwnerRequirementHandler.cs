using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace NotesApiNext.Politics
{
    public class NoteOwnerRequirementHandler(IHttpContextAccessor accessor)
        : AuthorizationHandler<NoteOwnerRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NoteOwnerRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var accessorResult = accessor
                .HttpContext!
                .Request
                .Query
                .TryGetValue("userId", out var userIdQuery);
            if (!accessorResult || !userIdQuery.Any())
            {
                context.Fail();
                return Task.CompletedTask;
            }
            if (userIdClaim.Value != userIdQuery.First())
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
