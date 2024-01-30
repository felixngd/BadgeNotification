using Voidex.Badge.Runtime.Interfaces;

namespace Voidex.Badge.Runtime
{
    public static class BadgeMessaging
    {
        public static void Initialize(IPubSub<BadgeChangedMessage> pubSub)
        {
            GlobalMessaging<BadgeChangedMessage>.Initialize(pubSub);
        }
        
        public static void UpdateBadge(string key, int value)
        {
            GlobalMessaging<BadgeChangedMessage>.Publish(new BadgeChangedMessage
            {
                key = key,
                value = value
            });
        }
        
        public static void UpdateBadge(BadgeData badgeData)
        {
            GlobalMessaging<BadgeChangedMessage>.Publish(new BadgeChangedMessage
            {
                key = badgeData.key,
                value = badgeData.value
            });
        }

    }
}