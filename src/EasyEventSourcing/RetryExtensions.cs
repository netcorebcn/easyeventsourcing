using System;
using System.Threading.Tasks;
using Polly;

namespace EasyEventSourcing
{
    internal static class RetryExtensions
    {
        internal static async Task DefaultRetryAsync(Func<Task> action) =>
            await RetryAsync(action);

        internal static async Task RetryAsync(Func<Task> action, int retries = 5) =>
            await Policy.Handle<Exception>()
            .WaitAndRetryAsync(retries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            .ExecuteAsync(action);
    }
}
