using System;
using Cysharp.Text;
using UnityEngine;
using Voidex.Badge.Extender;
using Voidex.Badge.Runtime;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    public class DynamicBadgeItem : MonoBehaviour
    {
        private IDisposable _disposable;
        public string key;
        
        public TMPro.TextMeshProUGUI text;

        private void OnEnable()
        {
            if(string.IsNullOrEmpty(key)) return;
            var value = GlobalData.BadgeNotification.GetBadgeCount(key);
            gameObject.SetActive(value > 0);
        }
        protected void OnDestroy()
        {
            //unsubscribe from the badge node
            _disposable?.Dispose();
        }

        private void OnBadgeChanged(BadgeChangedMessage<BadgeValue> message)
        {
            if (message.key.Equals(key))
            {
                if(message.badgeCount > 1){
                    text.SetText(message.badgeCount);
                    text.gameObject.SetActive(true);
                    gameObject.SetActive(true);
                }else if(message.badgeCount == 1){
                    text.gameObject.SetActive(false);
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            
        }
        
        public void Subscribe(string value)
        {
            //subscribe to the badge node
            this.key = value;
            var messagePipe = SampleBadgeMessaging.GetMessagingService<MessagePipeMessaging>();
            _disposable = messagePipe.Subscribe(key, OnBadgeChanged, new ChangedValueFilter<BadgeChangedMessage<BadgeValue>>());
        }
    }
}
