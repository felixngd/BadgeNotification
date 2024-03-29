using System;
using UnityEngine;
using Voidex.Badge.Extender;
using Voidex.Badge.Runtime;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    [DisallowMultipleComponent]
    public class UpgradeHeroBadge : MonoBehaviour
    {
        public BadgeNode badgeNode;
        private IDisposable _disposable;
        private string _key;
        protected void Awake()
        {
            _key = badgeNode.GetValue(null).ToString();
            //subscribe to the badge node
            var messagePipe = BadgeMessaging<BadgeValue>.GetMessagingService<MessagePipeMessaging>();
            _disposable = messagePipe.Subscribe(_key, OnBadgeChanged, new ChangedValueFilter<BadgeChangedMessage<BadgeValue>>());
        }
        

        protected void OnDestroy()
        {
            //unsubscribe from the badge node
            _disposable.Dispose();
        }

        private void OnBadgeChanged(BadgeChangedMessage<BadgeValue> message)
        {
            if (message.key.Equals(_key))
            {
                gameObject.SetActive(message.badgeCount > 0);
            }
        }

        private void OnEnable()
        {
            //This case is for when the badge is already updated before the gameobject is enabled.
            //Example: when the gameobject is not spawned yet and the badge is updated.
            if (GlobalData.BadgeNotification == null) return;
            var value = GlobalData.BadgeNotification.GetBadgeCount(_key);
            gameObject.SetActive(value > 0);
        }
    }
}
