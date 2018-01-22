using System;
using System.Threading.Tasks;

namespace PublishSubscribe.Message
{
    public sealed class MessageItem : IMessageItemWithoutArgs
    {
        private Action _callback;
        public MessageItem(Action callback)
        {
            _callback = callback;
        }
        /// <inheritdoc />
        public void Execute()
        {
            _callback?.Invoke();
        }

        /// <inheritdoc />
        public Task ExecuteAsync()
        {
            return Task.Run(_callback);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _callback = null;

        }
    }

    public sealed class MessageItem<TArg> : IMessageItem<TArg>
    {
        private Action<TArg> _callback;

        public MessageItem(Action<TArg> callback)
        {
            _callback = callback;
        }

        /// <inheritdoc />
        public void Execute(TArg arg)
        {
            _callback?.Invoke(arg);
        }

        /// <inheritdoc />
        public Task ExecuteAsync(TArg arg)
        {
            return Task.Run(() => Execute(arg));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _callback = null;

        }
    }
}