using System;
using MessagePipe;
using Voidex.Badge.Runtime;
using Voidex.Badge.Runtime.Interfaces;

namespace Voidex.Badge.Extender
{
    public class MessagePipeMessaging : IPubSub<BadgeChangedMessage<BadgeValue>>
    {
        public readonly IPublisher<string, BadgeChangedMessage<BadgeValue>> _publisher;
        public readonly ISubscriber<string, BadgeChangedMessage<BadgeValue>> _subscriber;
        public MessagePipeMessaging()
        {
            var builder = new BuiltinContainerBuilder();
            builder.AddMessagePipe(o => o.EnableCaptureStackTrace = true);
            builder.AddMessageBroker<string, BadgeChangedMessage<BadgeValue>>();

            var provider = builder.BuildServiceProvider();
            GlobalMessagePipe.SetProvider(provider);

            _publisher = GlobalMessagePipe.GetPublisher<string, BadgeChangedMessage<BadgeValue>>();
            _subscriber = GlobalMessagePipe.GetSubscriber<string, BadgeChangedMessage<BadgeValue>>();
        }
        
        public void Publish(BadgeChangedMessage<BadgeValue> topic)
        {
            _publisher.Publish(topic.key, topic);
        }

        public IDisposable Subscribe(string key, Action<BadgeChangedMessage<BadgeValue>> callback)
        {
            return _subscriber.Subscribe(key, callback);
        }
    }
}