using System.Security.Claims;

namespace WildBearAdventures2.API;

/// <summary>
/// Middleware for setting the test user
/// </summary>
public class SetTestUserMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructor that takes the next middleware
    /// </summary>
    /// <param name="next">The next middleware</param>
    /// <exception cref="ArgumentNullException">If next is null</exception>
    public SetTestUserMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    /// <summary>
    /// Invoke the middleware
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var userName = "admin@admin.com";
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userName),
                new(ClaimTypes.Name, userName),
                new("IsAdmin", "True"),
                new("UserId", "90114c77-05a9-4060-b0f4-d839055690d1"),
            };
            var identity = new ClaimsIdentity(claims, authenticationType: "Test");
            context.User = new ClaimsPrincipal(identity);
            await _next(context);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

/// <summary>
/// Setting up a pipeline for the test user middleware.
/// </summary>
public static class TestUserExtensions
{
    /// <summary>
    /// Configures a pipeline for test user middleware.
    /// </summary>
    public static void UseTestUser(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddleware<SetTestUserMiddleware>();
    }
}