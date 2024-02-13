
using Voidex.Badge.Extender;

namespace Voidex.Badge.Sample.Features.User
{
    public static class GlobalData
    {
        public static GameResources GameResources { get; } = new GameResources();
        public static BadgeNotification BadgeNotification { get; private set; }
        
        public static void InitBadgeNotification(Voidex.Badge.Runtime.BadgeGraph badgeGraph)
        {
            BadgeNotification = new BadgeNotification(badgeGraph);
        }
    }
}