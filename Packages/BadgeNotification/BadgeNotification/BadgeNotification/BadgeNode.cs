using Cysharp.Text;
using UnityEngine;
using UnityEngine.Serialization;
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
            // var p = GetInputValue<string>(FIELD);
            // name = string.IsNullOrEmpty(key) ? DEFAULT_NAME : $"{p}{Const.SEPARATOR}{key}";
        }

        [Output] public string child;
        [Input] public string parent;

        public string key;

        [TextArea]
        public string description;

        private void OnValidate()
        {
            var p = GetInputValue<string>(FIELD);
            name = string.IsNullOrEmpty(key) ? DEFAULT_NAME : ZString.Concat(p, Const.SEPARATOR, key);
            //save this asset
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public override object GetValue(NodePort port)
        {
            var input = GetInputValue<string>(FIELD);
            var value = ZString.Concat(input, Const.SEPARATOR, key);
            return value;
        }
    }
}