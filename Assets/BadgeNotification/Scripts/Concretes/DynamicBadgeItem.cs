using System;
using UnityEngine;
using Voidex.Badge.Runtime;

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
            var value = ApplicationContext.BadgeNotification.GetBadgeValue(key);
            gameObject.SetActive(value > 0);
        }
        protected void OnDestroy()
        {
            //unsubscribe from the badge node
            _disposable?.Dispose();
        }

        private void OnBadgeChanged(BadgeChangedMessage message)
        {
            if (message.key.Equals(key))
            {
                if(message.value > 1){
                    text.text = message.value.ToString();
                    text.gameObject.SetActive(true);
                    gameObject.SetActive(true);
                }else if(message.value == 1){
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
            _disposable = GlobalMessaging<BadgeChangedMessage>.Subscribe(this.key, OnBadgeChanged);
        }
    }
}
