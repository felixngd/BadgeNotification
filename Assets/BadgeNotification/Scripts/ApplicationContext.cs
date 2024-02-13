using UnityEngine;
using Voidex.Badge.Extender;
using Voidex.Badge.Runtime;
using Voidex.Badge.Runtime.Serialization;
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
            SampleBadgeMessaging.Initialize();
            GlobalData.InitBadgeNotification(badgeGraph);
        }

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <param name="key"></param>
        public void GetBadgeValue(string key)
        {
            var exists = GlobalData.BadgeNotification.GetBadge(key);
            if (exists == null)
            {
                Debug.Log($"Badge not found: {key}");
                return;
            }

            var badge = GlobalData.BadgeNotification.GetBadgeCount(key);
            Debug.Log($"Badge value: {badge}");
        }

        public void SerializeBadgeGraph()
        {
            var json = BadgeNotificationConverter.Serialize<BadgeNotification, BadgeValue>(GlobalData.BadgeNotification, value => { return value.nodeType == NodeType.Single; });

            Debug.Log(json);
        }

        public void DeserializeBadgeGraph(string json)
        {
            var badgeNotification = BadgeNotificationConverter.Deserialize<BadgeNotification, BadgeValue>(json);

            foreach (var badge in badgeNotification.GetBadges())
            {
                Debug.Log($"Badge: {badge.key} Value: {badge.badgeCount}");
            }
        }
        
        public void LogAllBadges(string keyPrefix)
        {
            foreach (var badge in GlobalData.BadgeNotification.GetBadgesBy(keyPrefix))
            {
                Debug.Log($"Badge: {badge.key} count: {badge.badgeCount} rank: {badge.value.rank}");
            }
        }
    }
    
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ApplicationContext))]
    public class ApplicationContextEditor : UnityEditor.Editor
    {
        public string key;
        public string json;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var myScript = (ApplicationContext) target;
            key = UnityEditor.EditorGUILayout.TextField("Key", key);
            if (GUILayout.Button("Get Badge Value"))
            {
                myScript.GetBadgeValue(key);
            }
            
            if (GUILayout.Button("Log All Badges"))
            {
                myScript.LogAllBadges(key);
            }

            if (GUILayout.Button("Serialize Badge Graph"))
            {
                myScript.SerializeBadgeGraph();
            }

            json = UnityEditor.EditorGUILayout.TextField("Json", json);

            if (GUILayout.Button("Deserialize Badge Graph"))
            {
                myScript.DeserializeBadgeGraph(json);
            }
        }
    }

#endif
}