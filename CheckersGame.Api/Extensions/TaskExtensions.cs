using CheckersGame.Api.Core;

namespace CheckersGame.Api.Extensions
{
    public static class LongPolling
    {
        public static async Task WaitWhileAsync(
            Func<bool> condition,
            int frequency = 25,
            int timeout = -1,
            CancellationToken cancellationToken = default)
        {
            var waitTask = Task.Run(async () =>
            {
                while (condition())
                {
                    await Task.Delay(frequency);
                }
            }, cancellationToken);

            if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout, cancellationToken)))
            {
                throw new TimeoutException();
            }
        }
    }
}
