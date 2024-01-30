using System;
using MessagePipe;
using Voidex.Badge.Runtime;
using Voidex.Badge.Runtime.Interfaces;

namespace Voidex.Badge.Sample
{
    public class MessagePipeMessaging : IPubSub<BadgeChangedMessage>
    {
        private readonly IPublisher<string, BadgeChangedMessage> _publisher;
        private readonly ISubscriber<string, BadgeChangedMessage> _subscriber;
        public MessagePipeMessaging()
        {
            var builder = new BuiltinContainerBuilder();
            builder.AddMessagePipe(o => o.EnableCaptureStackTrace = true);
            builder.AddMessageBroker<string, BadgeChangedMessage>();

            var provider = builder.BuildServiceProvider();
            GlobalMessagePipe.SetProvider(provider);

            _publisher = GlobalMessagePipe.GetPublisher<string, BadgeChangedMessage>();
            _subscriber = GlobalMessagePipe.GetSubscriber<string, BadgeChangedMessage>();
        }
        
        public void Publish(BadgeChangedMessage topic)
        {
            _publisher.Publish(topic.key, topic);
        }

        public IDisposable Subscribe(string key, Action<BadgeChangedMessage> callback)
        {
            return _subscriber.Subscribe(key, callback);
        }
    }
}