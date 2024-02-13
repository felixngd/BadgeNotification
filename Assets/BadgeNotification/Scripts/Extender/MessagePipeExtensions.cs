using System;
using MessagePipe;
using Voidex.Badge.Runtime;

namespace Voidex.Badge.Extender
{
    public static class MessagePipeExtensions
    {
        public static IDisposable Subscribe(this MessagePipeMessaging pipe, string key, System.Action<BadgeChangedMessage<BadgeValue>> action, params MessageHandlerFilter<BadgeChangedMessage<BadgeValue>>[] filters)
        {
            return pipe?._subscriber.Subscribe(key, action, filters);
        }
    }
}