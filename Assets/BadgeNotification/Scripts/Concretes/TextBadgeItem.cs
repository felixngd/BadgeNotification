using UnityEngine;
using Voidex.Badge.Extender;
using Voidex.Badge.Runtime;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    public class TextBadgeItem : BadgeItem
    {
        [SerializeField] private TMPro.TextMeshProUGUI text;

        private void OnEnable()
        {
            var key = badgeNode.GetValue(null).ToString();
            var value = GlobalData.BadgeNotification.GetBadgeCount(key);
            text.text = value.ToString();
            gameObject.SetActive(value > 0);
        }

        protected override void OnBadgeChanged(BadgeChangedMessage<BadgeValue> message)
        {
            if (message.key.Equals(badgeNode.GetValue(null).ToString()))
            {
                gameObject.SetActive(message.badgeCount > 0);
                text.text = message.badgeCount.ToString();
            }
        }
    }
}