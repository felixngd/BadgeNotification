using System;
using UnityEngine;
using Voidex.Badge.Extender;
using Voidex.Badge.Runtime;
using XNodeEditor;

namespace Voidex.Badge.Editor
{
    [CustomNodeEditor(typeof(BadgeNode))]
    public class BadgeNodeEditor : NodeEditor
    {
        private BadgeNode _badgeNode;
        IDisposable _disposable;

        private string _badgeCount;
        private string _badgeValue;

        public override void OnHeaderGUI()
        {
            _badgeNode = target as BadgeNode;
            if (_badgeNode == null)
            {
                return;
            }

            GUILayout.Label(_badgeNode.key, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            GUILayout.Label(_badgeCount);
        }

#if BADGE_NOTIFICATION_DEBUG
        public void DrawRetrieveData()
        {
            // Retrieve the BadgeData
            var badgeData = RetrieveValue();

            if (UnityEditor.EditorApplication.isPlaying)
            {
                _badgeCount = "Badge Count: " + badgeData;
            }
        }

        private int RetrieveValue()
        {
            if (_badgeNode == null)
            {
                return 0;
            }

            BadgeNotificationRegister.GetBadgeNotification(_badgeNode.graph.GetInstanceID(), out var bn);
            if (bn == null) return 0;

            var badge = bn.GetBadgeCount(_badgeNode.GetValue(null).ToString());
            return badge;
        }
#endif
    }
}