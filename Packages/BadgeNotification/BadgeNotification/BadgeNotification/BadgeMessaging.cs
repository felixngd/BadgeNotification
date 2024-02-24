using UnityEngine;
using Voidex.Badge.Runtime.Interfaces;

namespace Voidex.Badge.Runtime
{
    public static class BadgeMessaging<TMessage> where TMessage : struct
    {
        static IPubSub<BadgeChangedMessage<TMessage>> s_pubSub;

        public static void Initialize(IPubSub<BadgeChangedMessage<TMessage>> pubSub)
        {
            s_pubSub = pubSub;
        }

        public static T GetMessagingService<T>()
        {
            return (T) s_pubSub;
        }


        public static void UpdateBadge(string key, BadgeData<TMessage> badgeData)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            if (s_pubSub == null)
            {
                Debug.LogError("BadgeMessaging is not initialized");
                return;
            }

            s_pubSub.Publish(new BadgeChangedMessage<TMessage>
            {
                key = key,
                value = badgeData.value,
                badgeCount = badgeData.badgeCount
            });
        }
    }
}