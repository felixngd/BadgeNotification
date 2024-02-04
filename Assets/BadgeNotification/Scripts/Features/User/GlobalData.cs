using Voidex.Badge.Runtime;

namespace Voidex.Badge.Sample.Features.User
{
    public static class GlobalData
    {
        public static GameResources GameResources { get; } = new GameResources();
        public static BadgeNotification BadgeNotification { get; private set; }
        
        public static void InitBadgeNotification(BadgeGraph badgeGraph)
        {
            BadgeNotification = new BadgeNotification(badgeGraph);
        }
    }
}