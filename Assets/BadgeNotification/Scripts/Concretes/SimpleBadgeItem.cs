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
    }
}