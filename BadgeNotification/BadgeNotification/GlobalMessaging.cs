using System;
using Voidex.Badge.Runtime.Interfaces;

namespace Voidex.Badge.Runtime
{
    public static class GlobalMessaging<TMessage>
    {
        private static IPubSub<TMessage> s_pubSub;

        public static void Initialize(IPubSub<TMessage> pubSub)
        {
            s_pubSub = pubSub;
        }

        public static void Publish(TMessage message)
        {
            s_pubSub.Publish(message);
        }

        public static IDisposable Subscribe(string key, Action<TMessage> action)
        {
            return s_pubSub.Subscribe(key, action);
        }
    }
}