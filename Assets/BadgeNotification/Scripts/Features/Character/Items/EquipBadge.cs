using System;
using UnityEngine;
using Voidex.Badge.Extender;
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
            var messagePipe = BadgeMessaging<BadgeValue>.GetMessagingService<MessagePipeMessaging>();
            _disposable = messagePipe.Subscribe(_key, OnBadgeChanged, new ChangedValueFilter<BadgeChangedMessage<BadgeValue>>());
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

        protected void OnBadgeChanged(BadgeChangedMessage<BadgeValue> message)
        {
            if (message.key.Equals(_key))
            {
                //if the item is not equipped, show badge
                //if the item is equipped and rank of the badge greater than item level, show badge
                //otherwise, hide badge
                if (itemSlot.Item.isEquipped.equipped == false)
                {
                    gameObject.SetActive(message.badgeCount > 0);
                }
                else
                {
                    var item = itemSlot.Item;
                    var itemLevel = item.level;
                    gameObject.SetActive(itemLevel < message.value.rank && message.badgeCount > 0);
                }
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