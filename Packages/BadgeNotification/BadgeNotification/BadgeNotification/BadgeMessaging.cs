using UnityEngine;
using Voidex.Badge.Runtime.Interfaces;

namespace Voidex.Badge.Runtime
{
    public static class BadgeMessaging<TMessage> where TMessage : struct
    {
        static IPubSub<BadgeChangedMessage<TMessage>> s_pubSub;
        public static void Initialize(IPubSub<BadgeChangedMessage<TMessage>> pubSub)
        {
            //GlobalMessaging<BadgeChangedMessage>.Initialize(pubSub);
            s_pubSub = pubSub;
        }
        public static T GetMessagingService<T>()
        {
            return (T)s_pubSub;
        }


        public static void UpdateBadge(BadgeData<TMessage> badgeData)
        {
            s_pubSub.Publish(new BadgeChangedMessage<TMessage>
            {
                key = badgeData.key,
                value = badgeData.value,
                badgeCount = badgeData.badgeCount
            });

        }
    }
}