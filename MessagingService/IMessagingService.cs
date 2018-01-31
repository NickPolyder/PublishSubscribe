using System;
using System.Threading.Tasks;

namespace PublishSubscribe
{
    public interface IMessagingService
    {
        /// <summary>
        /// Sends the <paramref name="message"/> to the subscribers
        /// </summary>
        /// <param name="message"></param>
        void Send(string message);

        /// <summary>
        /// Sends the <paramref name="message"/> to the subscribers asynchronously 
        /// </summary>
        /// <param name="message"></param>
        /// <returns>A Promise task.</returns>
        Task SendAsync(string message);

        /// <summary>
        /// Sends the <paramref name="message"/> with the given <paramref name="payload"/> to the subscribers
        /// </summary>
        /// <param name="message"></param>
        /// <param name="payload"></param>
        void Send<TArg>(string message, TArg payload);

        /// <summary>
        /// Sends the <paramref name="message"/> with the given <paramref name="payload"/> to the subscribers asynchronously 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="payload"></param>
        /// <returns>A Promise task.</returns>
        Task SendAsync<TArg>(string message, TArg payload);

        /// <summary>
        /// Subscribes to the <paramref name="message"/> with the <paramref name="callback"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        void Subscribe(string message, Action callback);

        /// <summary>
        /// Subscribes to the <paramref name="message"/> with the <paramref name="callback"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        void Subscribe<TArg>(string message, Action<TArg> callback);
    }
}