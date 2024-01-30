using System;
using Voidex.Badge.Runtime;

namespace Voidex.Badge.Sample
{
    public class SimpleBadgeItem : BadgeItem
    {
        protected override void OnBadgeChanged(BadgeChangedMessage message)
        {
            if (message.key.Equals(badgeNode.GetValue(null).ToString()))
            {
                gameObject.SetActive(message.value > 0);
            }
        }

        private void OnEnable()
        {
            if(ApplicationContext.BadgeNotification == null) return;
            var key = badgeNode.GetValue(null).ToString();
            var value = ApplicationContext.BadgeNotification.GetBadgeValue(key);
            gameObject.SetActive(value > 0);
        }
    }
}