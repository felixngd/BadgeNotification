using UnityEngine;
using Voidex.Badge.Extender;
using Voidex.Badge.Runtime;
using Voidex.Badge.Runtime.Serialization;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    public class ApplicationContext : MonoBehaviour
    {
#if USE_XNODE
        [SerializeField] private BadgeGraph badgeGraph;
#endif

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            SampleBadgeMessaging.Initialize();
#if USE_XNODE
            GlobalData.InitBadgeNotification(badgeGraph);
#endif
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
            var json = BadgeNotificationConverter.Serialize<BadgeNotification, BadgeValue>(GlobalData.BadgeNotification,
                value => { return value.Value.nodeType == NodeType.Single; });

            Debug.Log(json);
        }

        public void DeserializeBadgeGraph(string json)
        {
            var badgeNotification = BadgeNotificationConverter.Deserialize<BadgeNotification, BadgeValue>(json);

            foreach (var badge in badgeNotification.GetBadges())
            {
                Debug.Log($"Badge: {badge.Path} Value: {badge.Value.badgeCount}");
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