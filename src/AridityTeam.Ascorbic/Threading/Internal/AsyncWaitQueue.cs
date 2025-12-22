using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AridityTeam.Threading.Internal
{
    internal sealed class AsyncWaitQueue
    {
        private readonly Queue<TaskCompletionSource<bool>> _queue = new();

        public bool HasWaiters => _queue.Count > 0;

        public Task Enqueue(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(static s =>
                {
                    ((TaskCompletionSource<bool>)s!).TrySetCanceled();
                }, tcs);
            }

            _queue.Enqueue(tcs);
            return tcs.Task;
        }

        public void ReleaseOne()
        {
            if (_queue.Count > 0)
                _queue.Dequeue().TrySetResult(true);
        }

        public void ReleaseAll()
        {
            while (_queue.Count > 0)
                _queue.Dequeue().TrySetResult(true);
        }
    }
}
