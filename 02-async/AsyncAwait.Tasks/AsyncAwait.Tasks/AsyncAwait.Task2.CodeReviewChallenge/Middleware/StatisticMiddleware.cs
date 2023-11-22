using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwait.Task2.CodeReviewChallenge.Headers;
using CloudServices.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AsyncAwait.Task2.CodeReviewChallenge.Middleware;

public class StatisticMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IStatisticService _statisticService;

    public StatisticMiddleware(RequestDelegate next, IStatisticService statisticService)
    {
        _next = next;
        _statisticService = statisticService ?? throw new ArgumentNullException(nameof(statisticService));
    }


    // This approach violates stateless principle, you would better to use separate endpoints to count page visits and get them (perform this requests using Ajax to not block the page from loading)
    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path;

        // Bad practice to wrap an async call in Task, use await instead
        // Task staticRegTask = Task.Run(
        //    () => _statisticService.RegisterVisitAsync(path)
        //    .ConfigureAwait(false)
        //    .GetAwaiter().OnCompleted(UpdateHeaders));


        //Consider to use logger for this
        //Console.WriteLine(staticRegTask.Status); // just for debugging purposes

        await _statisticService.RegisterVisitAsync(path).ConfigureAwait(false);

        // We don't need the local function this operation
        //void UpdateHeaders()
        //{
        //    context.Response.Headers.Add(
        //        CustomHttpHeaders.TotalPageVisits,
        //        _statisticService.GetVisitsCountAsync(path).GetAwaiter().GetResult().ToString());
        //}
        context.Response.Headers.Add(
            CustomHttpHeaders.TotalPageVisits,
            (await _statisticService.GetVisitsCountAsync(path).ConfigureAwait(false)).ToString());


        //Thread.Sleep(3000); // without this the statistic counter does not work
        await _next(context);
    }
}
