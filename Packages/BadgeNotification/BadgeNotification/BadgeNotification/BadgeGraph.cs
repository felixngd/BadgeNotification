#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using Voidex.Trie;

namespace Voidex.Badge.Runtime
{
    [CreateAssetMenu(fileName = "RedDotGraph", menuName = "BadgeNotification/Graph")]
    public class BadgeGraph : XNode.NodeGraph
    {
        public TrieMap<BadgeData> trieMap = new TrieMap<BadgeData>();
#if ODIN_INSPECTOR
        [Button]
#endif
        private void LogData()
        {
            foreach (var node in nodes)
            {
                var key = node.GetValue(null).ToString();
                trieMap.Add(key, new BadgeData
                {
                    key = key,
                    value = 0
                });
            }

            name = "RedDot Graph";

            foreach (var key in trieMap.Keys())
            {
                Debug.Log(key);
            }
        }
    }
}