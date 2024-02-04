using System;
using UnityEngine;
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
            var messagePipe = BadgeMessaging.GetMessagingService<MessagePipeMessaging>();
            _disposable = messagePipe.Subscribe(_key, OnBadgeChanged, new ChangedValueFilter<BadgeChangedMessage>());
        }
        

        protected void OnDestroy()
        {
            //unsubscribe from the badge node
            _disposable.Dispose();
        }

        private void OnBadgeChanged(BadgeChangedMessage message)
        {
            if (message.key.Equals(_key))
            {
                gameObject.SetActive(message.value > 0);
            }
        }

        private void OnEnable()
        {
            //This case is for when the badge is already updated before the gameobject is enabled.
            //Example: when the gameobject is not spawned yet and the badge is updated.
            if (GlobalData.BadgeNotification == null) return;
            var value = GlobalData.BadgeNotification.GetBadgeValue(_key);
            gameObject.SetActive(value > 0);
        }
    }
}
