using System;
using UnityEngine;
using Voidex.Badge.Runtime;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    [DisallowMultipleComponent]
    public class EquipBadge : MonoBehaviour
    {
        public BadgeNode badgeNode;
        public ItemSlot itemSlot;
        private IDisposable _disposable;
        private string _key;

        protected void Awake()
        {
            _key = badgeNode.GetValue(null).ToString();
            //subscribe to the badge node
            var messagePipe = BadgeMessaging.GetMessagingService<MessagePipeMessaging>();
            _disposable = messagePipe.Subscribe(_key, OnBadgeChanged, new ChangedValueFilter<BadgeChangedMessage>());
        }

        private void OnValidate()
        {
            if (itemSlot == null)
            {
                itemSlot = GetComponentInParent<ItemSlot>();
            }
        }

        protected virtual void OnDestroy()
        {
            //unsubscribe from the badge node
            _disposable.Dispose();
        }

        protected void OnBadgeChanged(BadgeChangedMessage message)
        {
            if (message.key.Equals(_key))
            {
                if (itemSlot.Item.isEquipped.Item2)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(message.value > 0);
                }
                
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