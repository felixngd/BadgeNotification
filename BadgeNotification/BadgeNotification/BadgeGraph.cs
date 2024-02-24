#if USE_XNODE
using UnityEngine;
using Voidex.Trie;

namespace Voidex.Badge.Runtime
{
    [CreateAssetMenu(fileName = "RedDotGraph", menuName = "BadgeNotification/Graph")]
    public class BadgeGraph : XNode.NodeGraph
    {
#if UNITY_EDITOR
        public TrieMap<BadgeData<int>> trieMap = new TrieMap<BadgeData<int>>();
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        private void LogData()
        {
            foreach (var node in nodes)
            {
                var key = node.GetValue(null).ToString();
                trieMap.Add(key, new BadgeData<int>
                {
                    //key = key,
                    value = 0,
                    badgeCount = 0,
                });
            }

            name = "RedDot Graph";

            foreach (var key in trieMap.Keys())
            {
                Debug.Log(key);
            }
        }
#endif
    }
}
#endif