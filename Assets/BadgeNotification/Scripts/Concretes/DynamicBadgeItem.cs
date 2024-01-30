using System;
using UnityEngine;
using Voidex.Badge.Runtime;

namespace Voidex.Badge.Sample
{
    public class DynamicBadgeItem : MonoBehaviour
    {
        [SerializeField] protected BadgeNode badgeNode;
        private IDisposable _disposable;
        public string key;
        
        public TMPro.TextMeshProUGUI text;

        protected virtual void OnDestroy()
        {
            //unsubscribe from the badge node
            _disposable.Dispose();
        }

        private void OnBadgeChanged(BadgeChangedMessage message)
        {
            if (message.key.Equals(key))
            {
                if(message.value > 1){
                    text.text = message.value.ToString();
                    text.gameObject.SetActive(true);
                }else if(message.value == 1){
                    text.gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
        
        public void SetIdAndSubscribe(string key)
        {
            //subscribe to the badge node
            this.key = key;
            _disposable = GlobalMessaging<BadgeChangedMessage>.Subscribe(key, OnBadgeChanged);
        }
    }
}
