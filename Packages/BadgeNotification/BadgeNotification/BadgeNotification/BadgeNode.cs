using UnityEngine;
using Voidex.Trie;
using XNode;

namespace Voidex.Badge.Runtime
{
    [System.Serializable]
    public class BadgeNode : XNode.Node
    {
        private const string FIELD = "parent";
        private const string DEFAULT_NAME = "BadgeNode";
        protected override void Init()
        {
            base.Init();
            name = string.IsNullOrEmpty(key) ? DEFAULT_NAME : key;
        }

        [Output] public string child;
        [Input] public string parent;

        public string key;

        [TextArea]
        public string description;

        private void OnValidate()
        {
            name = string.IsNullOrEmpty(key) ? DEFAULT_NAME : key;
        }

        public override object GetValue(NodePort port)
        {
            var input = GetInputValue<string>(FIELD);
            var value = $"{input}{Const.SEPARATOR}{key}";
            return value;
        }
    }
}