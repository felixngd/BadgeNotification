#if BADGE_NOTIFICATION_DEBUG
using System.Collections.Generic;
using Voidex.Badge.Extender;

namespace Voidex.Badge.Runtime
{
    public static class BadgeNotificationRegister
    {
        static readonly Dictionary<int, BadgeNotification> s_badgeNotifications = new Dictionary<int, BadgeNotification>();

        public static void Register(BadgeNotification badgeNotification, int id)
        {
            s_badgeNotifications[id] = badgeNotification;
        }

#if USE_XNODE
        public static void Register(BadgeNotification badgeNotification, BadgeGraph graph)
        {
            s_badgeNotifications.TryAdd(graph.GetInstanceID(), badgeNotification);
        }
#endif

        public static void Unregister(int id)
        {
            s_badgeNotifications.Remove(id);
        }

#if USE_XNODE
        public static void Unregister(BadgeGraph graph)
        {
            s_badgeNotifications.Remove(graph.GetInstanceID());
        }
#endif

        public static bool GetBadgeNotification(int id, out BadgeNotification badgeNotification)
        {
            return s_badgeNotifications.TryGetValue(id, out badgeNotification);
        }
    }
}

#endif