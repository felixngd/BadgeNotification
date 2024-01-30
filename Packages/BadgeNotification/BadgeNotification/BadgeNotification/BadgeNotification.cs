using System.Text;
using UnityEngine;
using Voidex.Trie;

namespace Voidex.Badge.Runtime
{
    /// <summary>
    /// The badge system is a trie data structure. Each node is a badge. Each node has a value.
    /// Usage:
    /// 1. Initialize the badge system with a badge graph. Inject the instance to use or use global static instance
    /// 2. Add badge by key
    /// 3. Update badge by key
    /// 4. Get badge by key
    /// </summary>
    public class BadgeNotification
    {
        private TrieMap<BadgeData> _trieMap;

        public BadgeData GetBadge(string key)
        {
            if (_trieMap == null) throw new System.Exception("BadgeSystem not initialized");
            return _trieMap.ValueBy(key);
        }

        public void Initialize(BadgeGraph badgeGraph)
        {
            _trieMap = new TrieMap<BadgeData>();
            foreach (var node in badgeGraph.nodes)
            {
                var key = node.GetValue(null).ToString();
                var value = new BadgeData
                {
                    key = key,
                    value = 0
                };
                _trieMap.Add(key, value);

                GlobalMessaging<BadgeChangedMessage>.Publish(new BadgeChangedMessage
                {
                    value = value.value, key = value.key
                });
            }
        }

        /// <summary>
        /// Add badge by key. If badge already exists, do nothing
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddBadge(string key, int value)
        {
            if (_trieMap.HasKey(key)) return;

            var node = _trieMap.GetRootTrieNode();
            if (node.Value != null)
                node.Value.value += value;
            var paths = key.Split(Const.SEPARATOR);
            var fullPath = new StringBuilder(50);
            foreach (var path in paths)
            {
                if (fullPath.Length > 0)
                {
                    fullPath.Append(Const.SEPARATOR);
                }

                fullPath.Append(path);

                if (!node.HasChild(path))
                {
                    var child = new TrieNode<BadgeData>(path);
                    child.Value = new BadgeData
                    {
                        key = fullPath.ToString(),
                        value = 0
                    };
                    node.SetChild(child);
                }

                node = node.GetChild(path);

                node.Value.value += value;
                _trieMap.Add(node.Value.key, node.Value);
                Debug.Log($"&&& {node.Value.key} with value {node.Value.value}");

                //notify ui
                GlobalMessaging<BadgeChangedMessage>.Publish(new BadgeChangedMessage
                {
                    value = node.Value.value, key = node.Value.key
                });
            }
            
            var badge = _trieMap.ValueBy(key);
            Debug.Log($"Added badge {badge.key} with value {badge.value}");
        }

        /// <summary>
        /// Update badge value by delta. Delta is negative, badge value will be set to 0
        /// </summary>
        /// <param name="key"></param>
        /// <param name="delta"></param>
        public void UpdateBadge(string key, int delta)
        {
            var targetNode = _trieMap.GetTrieNode(key);
            if (targetNode == null)
            {
                throw new System.Exception($"Badge {key} not found");
            }

            if (delta < 0 && targetNode.Value.value + delta < 0)
            {
                delta = -targetNode.Value.value;
            }

            var node = _trieMap.GetRootTrieNode();
            var paths = key.Split(Const.SEPARATOR);
            foreach (var path in paths)
            {
                var child = node.GetChild(path);
                child.Value.value += delta;
                node = child;

                //notify ui
                GlobalMessaging<BadgeChangedMessage>.Publish(new BadgeChangedMessage
                {
                    value = node.Value.value, key = node.Value.key
                });
            }
        }

        /// <summary>
        /// Get badge value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetBadgeValue(string key)
        {
            var node = _trieMap.GetTrieNode(key);
            return node == null ? 0 : node.Value.value;
        }
    }
}