using Voidex.Badge.Runtime;

namespace Voidex.Badge.Extender
{
    public class BadgeNotification : BadgeNotificationBase<BadgeValue>
    {
        public BadgeNotification(BadgeGraph badgeGraph) : base(badgeGraph)
        {
#if BADGE_NOTIFICATION_DEBUG
            BadgeNotificationRegister.Register(this, badgeGraph);
#endif
        }

        public BadgeNotification()
        {
        }
    }
}