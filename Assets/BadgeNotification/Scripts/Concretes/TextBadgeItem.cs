using System;
using UnityEngine;
using UnityEngine.UI;
using Voidex.Badge.Runtime;

namespace Voidex.Badge.Sample
{
    public class TextBadgeItem : BadgeItem
    {
        [SerializeField] private TMPro.TextMeshProUGUI text;

        private void OnEnable()
        {
            if(ApplicationContext.BadgeNotification == null) return;
            var key = badgeNode.GetValue(null).ToString();
            var value = ApplicationContext.BadgeNotification.GetBadgeValue(key);
            text.text = value.ToString();
        }

        protected override void OnBadgeChanged(BadgeChangedMessage message)
        {
            if (message.key.Equals(badgeNode.GetValue(null).ToString()))
            {
                text.text = message.value.ToString();
            }
            
            gameObject.SetActive(message.value > 0);
        }
    }
}