using System;
using System.Threading.Tasks;
using Polly;

namespace EasyEventSourcing
{
    public static class RetryExtensions
    {
        public static async Task DefaultRetryAsync(this Task action) =>
            await RetryAsync(action);

        public static async Task RetryAsync(this Task action, int retries = 5) =>
            await Policy.Handle<Exception>()
            .WaitAndRetryAsync(retries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            .ExecuteAsync(async () => await action);
    }
}
