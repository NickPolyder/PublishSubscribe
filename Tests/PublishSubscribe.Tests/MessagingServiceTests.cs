using System;
using Xunit;

namespace PublishSubscribe.Tests
{
    public class MessagingServiceTests
    {
        [Fact]
        public void SubscribePublishSimpleMessage()
        {
            const string message = "Message";

            var subscriberCalled = false;

            MessagingService.Instance.Subscribe(message, () =>
            {
                subscriberCalled = true;
            });

            MessagingService.Instance.Send(message);

            Assert.True(subscriberCalled);

        }

        [Fact]
        public void SubscribePublishMessageWithArgs()
        {
            const string message = "MessageWithArgs";

            var subscriberCalled = false;

            MessagingService.Instance.Subscribe<bool>(message, (arg) =>
            {
                subscriberCalled = arg;
            });

            MessagingService.Instance.Send(message, true);

            Assert.True(subscriberCalled);

        }

        [Fact]
        public void SubscribeWithArgsPublishWithWrongArgs()
        {
            const string message = "MessageWithArgs";

            var subscriberCalled = false;

            MessagingService.Instance.Subscribe<bool>(message, (arg) =>
            {
                subscriberCalled = arg;
            });

            MessagingService.Instance.Send(message, "yeah");

            Assert.False(subscriberCalled);

        }
    }
}
