using System;
using UnityEngine;
using XNode;

namespace Voidex.Badge.Runtime
{
    [System.Serializable]
    public class BadgeNode : XNode.Node
    {
        protected override void Init()
        {
            base.Init();
            name = string.IsNullOrEmpty(key) ? "RedDot" : key;
        }

        [Output] public string child;
        [Input] public string parent;

        public string key;

        [TextArea]
        public string description;

        private void OnValidate()
        {
            name = string.IsNullOrEmpty(key) ? "RedDot" : key;
        }

        public override object GetValue(NodePort port)
        {
            var input = GetInputValue<string>("parent");
            var value = $"{input}|{key}";
            return value;
        }
    }
}