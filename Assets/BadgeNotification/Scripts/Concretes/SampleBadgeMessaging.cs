using Voidex.Badge.Extender;
using Voidex.Badge.Runtime;
using Voidex.Badge.Runtime.Interfaces;

namespace Voidex.Badge.Sample
{
    public static class SampleBadgeMessaging
    {
        public static void Initialize()
        {
            IPubSub<BadgeChangedMessage<BadgeValue>> pubSub = new MessagePipeMessaging();
            BadgeMessaging<BadgeValue>.Initialize(pubSub);
        }

        public static T GetMessagingService<T>()
        {
            return BadgeMessaging<BadgeValue>.GetMessagingService<T>();
        }
    }
}