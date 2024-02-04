using System;
using MessagePipe;
using Voidex.Badge.Runtime;

namespace Voidex.Badge.Sample
{
    public static class MessagePipeExtensions
    {
        public static IDisposable Subscribe(this MessagePipeMessaging pipe, string key, System.Action<BadgeChangedMessage> action, params MessageHandlerFilter<BadgeChangedMessage>[] filters)
        {
            return pipe?._subscriber.Subscribe(key, action, filters);
        }
    }
}