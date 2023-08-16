using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace HeadlessProjectv1;

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
                new(ClaimTypes.NameIdentifier, userName)
            };
            var identity = new ClaimsIdentity(claims);
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