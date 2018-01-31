using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PublishSubscribe.Message;

namespace PublishSubscribe
{
    public class MessagingService : IMessagingService
    {
        private static readonly object Lock = new object();
        private static volatile MessagingService _instance;

        public static IMessagingService Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MessagingService();
                    }
                }
                return _instance;
            }
        }

        private readonly ConcurrentDictionary<string, List<IMessageItem>> _subscribers =
            new ConcurrentDictionary<string, List<IMessageItem>>();

        ///<inheritdoc />
        public void Send(string message)
        {
            var subscribers = GetSubscribersFromMessage(message);

            if (subscribers.Count <= 0) return;

            foreach (var subscriber in subscribers.OfType<IMessageItemWithoutArgs>())
            {
                subscriber?.Execute();
            }
        }

        ///<inheritdoc />
        public async Task SendAsync(string message)
        {
            var subscribers = GetSubscribersFromMessage(message);

            if (subscribers.Count <= 0) return;

            await Task.WhenAll(subscribers.OfType<IMessageItemWithoutArgs>().Select(sub => sub.ExecuteAsync()));
        }

        ///<inheritdoc />
        public void Send<TArg>(string message, TArg payload)
        {
            var subscribers = GetSubscribersFromMessage(message);

            if (subscribers.Count <= 0) return;

            foreach (var subscriber in subscribers)
            {
                if (subscriber is IMessageItem<TArg> subscriberWithPayload)
                {
                    subscriberWithPayload.Execute(payload);
                }
            }
        }

        ///<inheritdoc />
        public async Task SendAsync<TArg>(string message, TArg payload)
        {
            var subscribers = GetSubscribersFromMessage(message);

            if (subscribers.Count <= 0) return;

            await Task.WhenAll(subscribers.OfType<IMessageItem<TArg>>().Select(sub => sub.ExecuteAsync(payload)));
        }

        ///<inheritdoc />
        public void Subscribe(string message, Action callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            var messageItem = new MessageItem(callback);

            AddSubscriber(message, messageItem);
        }

        ///<inheritdoc />
        public void Subscribe<TArg>(string message, Action<TArg> callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            var messageItem = new MessageItem<TArg>(callback);

            AddSubscriber(message, messageItem);
        }

        private void AddSubscriber(string message, IMessageItem messageItem)
        {
            if (_subscribers.TryGetValue(message, out List<IMessageItem> subscribers))
            {
                var array = new IMessageItem[subscribers.Count + 1];
                subscribers.CopyTo(array);
                array[subscribers.Count] = messageItem;
                _subscribers.TryUpdate(message, array.ToList(), subscribers);
            }
            else
            {
                _subscribers.TryAdd(message, new List<IMessageItem>() { messageItem });
            }
        }

        private List<IMessageItem> GetSubscribersFromMessage(string message)
        {
            if (_subscribers.TryGetValue(message, out List<IMessageItem> subscribers))
            {
                return subscribers;
            }
            return new List<IMessageItem>();
        }

    }
}