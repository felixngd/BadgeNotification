using UnityEngine;
using Voidex.Badge.Runtime.Interfaces;

namespace Voidex.Badge.Runtime
{
    public static class BadgeMessaging
    {
        static IPubSub<BadgeChangedMessage> s_pubSub;
        public static void Initialize(IPubSub<BadgeChangedMessage> pubSub)
        {
            //GlobalMessaging<BadgeChangedMessage>.Initialize(pubSub);
            s_pubSub = pubSub;
        }
        public static T GetMessagingService<T>()
        {
            return (T)s_pubSub;
        }
        
        public static void UpdateBadge(string key, int value)
        {
            s_pubSub.Publish(new BadgeChangedMessage
            {
                key = key,
                value = value
            });
        }
        
        public static void UpdateBadge(BadgeData badgeData)
        {
            s_pubSub.Publish(new BadgeChangedMessage
            {
                key = badgeData.key,
                value = badgeData.value
            });
            
        }
        
        public static void Publish(BadgeChangedMessage message)
        {
            s_pubSub.Publish(message);
        }
        
        public static void Subscribe(string key, System.Action<BadgeChangedMessage> action)
        {
            s_pubSub.Subscribe(key, action);
        }
    }
}