using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Voidex.Badge.Runtime
{
    [DisallowMultipleComponent]
    public abstract class BadgeItem : MonoBehaviour
    {
        #if ODIN_INSPECTOR
        [AssetSelector]
        #endif
        [SerializeField] protected BadgeNode badgeNode;
        private IDisposable _disposable;
        protected virtual void Awake()
        {
            //subscribe to the badge node
           _disposable = GlobalMessaging<BadgeChangedMessage>.Subscribe(badgeNode.GetValue(null).ToString(), OnBadgeChanged);
        }
        
        protected virtual void OnDestroy()
        {
            //unsubscribe from the badge node
            _disposable.Dispose();
        }
        
        protected abstract void OnBadgeChanged(BadgeChangedMessage message);
    }
}