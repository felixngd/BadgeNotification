using System;
using Cysharp.Text;
using UnityEngine;
using Voidex.Badge.Extender;
using Voidex.Badge.Runtime;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    [DisallowMultipleComponent]
    public class UpgradeEquipmentBadge : MonoBehaviour
    {
        [SerializeField] protected BadgeNode badgeNode;
        [SerializeField] private TMPro.TextMeshProUGUI badgeText;
        private IDisposable _disposable;
        
        public ItemSlot itemSlot;
        private string _key;
        protected virtual void Awake()
        {
            _key = badgeNode.GetValue(null).ToString();
            //subscribe to the badge node
            var messagePipe = BadgeMessaging<BadgeValue>.GetMessagingService<MessagePipeMessaging>();
            _disposable = messagePipe.Subscribe(_key, OnBadgeChanged, new ChangedValueFilter<BadgeChangedMessage<BadgeValue>>());
        }
        
        protected virtual void OnDestroy()
        {
            //unsubscribe from the badge node
            _disposable.Dispose();
        }
        
        private void OnValidate()
        {
            if(badgeText == null)
                badgeText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            
            if (itemSlot == null)
                itemSlot = GetComponentInParent<ItemSlot>();
        }
        
        protected void OnBadgeChanged(BadgeChangedMessage<BadgeValue> message)
        {
            if (message.key.Equals(_key))
            {
                if (itemSlot.Item == null) return;
                if (!itemSlot.Item.isEquipped.equipped)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(message.badgeCount > 0);
                    badgeText.SetText(message.badgeCount);
                }
            }
        }

        private void OnEnable()
        {
            if(GlobalData.BadgeNotification == null) return;
            var key = badgeNode.GetValue(null).ToString();
            var value = GlobalData.BadgeNotification.GetBadgeCount(key);
            gameObject.SetActive(value > 0);
        }
    }
}
