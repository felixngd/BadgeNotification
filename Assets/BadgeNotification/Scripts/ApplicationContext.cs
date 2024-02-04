using System;
using UnityEngine;
using Voidex.Badge.Runtime;
using Voidex.Badge.Runtime.Interfaces;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    public class ApplicationContext : MonoBehaviour
    {
        [SerializeField] private BadgeGraph badgeGraph;
        
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            IPubSub<BadgeChangedMessage> pubSub = new MessagePipeMessaging();
            BadgeMessaging.Initialize(pubSub);
            GlobalData.InitBadgeNotification(badgeGraph);
        }
        
        public void GetBadgeValue(string key)
        {
            var exists = GlobalData.BadgeNotification.GetBadge(key);
            if (exists == null)
            {
                Debug.Log($"Badge not found: {key}");
                return;
            }

            var badge = GlobalData.BadgeNotification.GetBadgeValue(key);
            Debug.Log($"Badge value: {badge}");
        }
    }
    
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ApplicationContext))]
    public class ApplicationContextEditor : UnityEditor.Editor
    {
        public string key;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var myScript = (ApplicationContext)target;
            key = UnityEditor.EditorGUILayout.TextField("Key", key);
            if (GUILayout.Button("Get Badge Value"))
            {
                myScript.GetBadgeValue(key);
            }
        }
    }
#endif
}