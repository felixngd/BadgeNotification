using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Voidex.Badge.Runtime;
using XNode;
using XNodeEditor;

namespace Voidex.Badge.Editor
{
    [CustomNodeGraphEditor(typeof(Badge.Runtime.BadgeGraph))]
    public class BadgeGraphEditor : NodeGraphEditor
    {
        public override void OnOpen()
        {
            base.OnOpen();
            //set title
            window.titleContent = new GUIContent("Badge Graph");
        }
#if BADGE_NOTIFICATION_DEBUG
        public override void AddContextMenuItems(GenericMenu menu, Type compatibleType = null, NodePort.IO direction = NodePort.IO.Input)
        {
            base.AddContextMenuItems(menu, compatibleType, direction);
            
            menu.AddItem(new GUIContent("Refresh"), false, Refresh);
        }

        private void Refresh()
        {
            var badgeGraph = target as Badge.Runtime.BadgeGraph;
            if (badgeGraph == null)
            {
                Debug.LogError("BadgeGraph is null");
                return;
            }
            
            foreach (var badgeNodeEditor in GetAllBadgeNodeEditors())
            {
                badgeNodeEditor.DrawRetrieveData();
            }
        }
        
        public IEnumerable<BadgeNodeEditor> GetAllBadgeNodeEditors()
        {
            var badgeNodes = target.nodes;
            foreach (var badgeNode in badgeNodes)
            {
                var editor = NodeEditor.GetEditor(badgeNode, window);
                if (editor != null && editor is BadgeNodeEditor)
                {
                    yield return editor as BadgeNodeEditor;
                }
            }
        }
#endif
    }
}