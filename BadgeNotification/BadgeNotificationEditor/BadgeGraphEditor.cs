using UnityEngine;
using Voidex.Badge.Runtime;
using XNode;
using XNodeEditor;

namespace RedDotSystem.RedDotEditor
{
    [CustomNodeGraphEditor(typeof(BadgeGraph))]
    public class BadgeGraphEditor : NodeGraphEditor
    {
        public override void OnOpen()
        {
            base.OnOpen();
            //set title
            window.titleContent = new GUIContent("Badge Graph");
            
        }
    }
}