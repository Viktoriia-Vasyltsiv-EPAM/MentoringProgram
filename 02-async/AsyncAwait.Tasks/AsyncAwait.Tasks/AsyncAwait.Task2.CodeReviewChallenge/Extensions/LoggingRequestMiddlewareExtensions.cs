using AsyncAwait.Task2.CodeReviewChallenge.Middleware;
using Microsoft.AspNetCore.Builder;

namespace AsyncAwait.Task2.CodeReviewChallenge.Extensions;

// Confusing naming
//public static class LoggingRequestMiddlewareExtensions
public static class StatisticMiddlewareExtensions
{
    public static IApplicationBuilder UseStatistic(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<StatisticMiddleware>();
    }
}
