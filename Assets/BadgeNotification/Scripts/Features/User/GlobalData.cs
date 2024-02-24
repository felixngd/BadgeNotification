using Voidex.Badge.Extender;

namespace Voidex.Badge.Sample.Features.User
{
    public static class GlobalData
    {
        public static GameResources GameResources { get; } = new GameResources();
        public static BadgeNotification BadgeNotification { get; private set; }

#if USE_XNODE
        /// <summary>
        /// In this example I use only one badge notification, but you can use multiple badge notifications in your application.
        /// </summary>
        /// <param name="badgeGraph"></param>
        public static void InitBadgeNotification(Voidex.Badge.Runtime.BadgeGraph badgeGraph)
        {
            BadgeNotification = new BadgeNotification(badgeGraph);
        }
#endif
        
        public static void InitBadgeNotification()
        {
            BadgeNotification = new BadgeNotification();
        }
    }
}