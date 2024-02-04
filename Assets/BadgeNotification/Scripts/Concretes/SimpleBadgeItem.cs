using System;
using UnityEngine;
using Voidex.Badge.Runtime;
using Voidex.Badge.Sample.Features.User;

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
            if(GlobalData.BadgeNotification == null) return;
            var key = badgeNode.GetValue(null).ToString();
            var value = GlobalData.BadgeNotification.GetBadgeValue(key);
            gameObject.SetActive(value > 0);
        }
    }
}