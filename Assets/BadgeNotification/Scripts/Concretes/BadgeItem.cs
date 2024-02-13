using System;
using UnityEngine;
using Voidex.Badge.Extender;
using Voidex.Badge.Sample;

namespace Voidex.Badge.Runtime
{
    [DisallowMultipleComponent]
    public abstract class BadgeItem : MonoBehaviour
    {
        [SerializeField]
        protected BadgeNode badgeNode;

        private IDisposable _disposable;

        protected virtual void Awake()
        {
            //subscribe to the badge node
            var messagePipe = SampleBadgeMessaging.GetMessagingService<MessagePipeMessaging>();
           _disposable = messagePipe.Subscribe(badgeNode.GetValue(null).ToString(), OnBadgeChanged, new ChangedValueFilter<BadgeChangedMessage<BadgeValue>>());
        }

        protected virtual void OnDestroy()
        {
            //unsubscribe from the badge node
            _disposable.Dispose();
        }

        protected abstract void OnBadgeChanged(BadgeChangedMessage<BadgeValue> message);
    }
}