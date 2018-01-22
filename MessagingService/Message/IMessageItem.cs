using System;
using System.Threading.Tasks;

namespace PublishSubscribe.Message
{
    /// <summary>
    /// Marker Interface
    /// </summary>
    public interface IMessageItem : IDisposable
    { }

    public interface IMessageItemWithoutArgs : IMessageItem
    {
        void Execute();

        Task ExecuteAsync();
    }

    public interface IMessageItem<in TArg> : IMessageItem
    {
        void Execute(TArg arg);

        Task ExecuteAsync(TArg arg);

    }

}