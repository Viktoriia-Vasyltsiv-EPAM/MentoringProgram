using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal static class Calculator
{
    // todo: change this method to support cancellation token
    public static Task<long> Calculate(int n, CancellationToken token)
    {
        long sum = 0;

        checked
        {
            for (var i = 0; i < n; i++)
            {
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();
                sum = sum + (i + 1);
                Thread.Sleep(10);
            }
        }        

        return Task.FromResult(sum);
    }
}
