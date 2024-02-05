using System;
using Cysharp.Text;
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
        private readonly TrieMap<BadgeData> _trieMap;

        public BadgeData GetBadge(string key)
        {
            if (_trieMap == null) throw new System.Exception("BadgeSystem not initialized");
            return _trieMap.ValueBy(key);
        }

        /// <summary>
        /// Get trie node by key prefix
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <returns></returns>
        public TrieNode<BadgeData> GetTrieNode(string keyPrefix)
        {
            return _trieMap.GetTrieNode(keyPrefix);
        }

        public BadgeNotification(BadgeGraph badgeGraph)
        {
            _trieMap = new TrieMap<BadgeData>();
            foreach (var node in badgeGraph.nodes)
            {
                var key = node.GetValue(null).ToString();
                if (node is BadgeNode badgeNode)
                {
                    var value = new BadgeData
                    {
                        key = key,
                        value = 0,
                        nodeType = badgeNode.nodeType
                    };
                    _trieMap.Add(key, value);
                    BadgeMessaging.UpdateBadge(value);
                }
                else
                {
                    var value = new BadgeData
                    {
                        key = key,
                        value = 0,
                    };
                    _trieMap.Add(key, value);
                    BadgeMessaging.UpdateBadge(value);
                }
            }
        }

        /// <summary>
        /// Add badge by key. If badge already exists, do nothing
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="nodeType"></param>
        public void AddBadge(string key, int value, NodeType nodeType = NodeType.Multiple)
        {
            if (_trieMap.HasKey(key)) return;

            var node = _trieMap.GetRootTrieNode();
            if (node.Value != null)
                node.Value.value += value;

            ReadOnlySpan<char> keySpan = key.AsSpan();
            var fullPathBuilder = ZString.CreateStringBuilder();
            int start = 0;

            for (int i = 0; i <= keySpan.Length; i++)
            {
                if (i == keySpan.Length || keySpan[i] == Const.SEPARATOR)
                {
                    var pathSpan = keySpan.Slice(start, i - start);
                    start = i + 1; // Prepare for the next segment

                    if (fullPathBuilder.Length > 0)
                    {
                        fullPathBuilder.Append(Const.SEPARATOR);
                    }

                    fullPathBuilder.Append(pathSpan);

                    string path = pathSpan.ToString(); // Convert span to string for Trie operations
                    if (!node.HasChild(path))
                    {
                        var child = new TrieNode<BadgeData>(path);
                        child.Value = new BadgeData
                        {
                            key = fullPathBuilder.ToString(), // Minimize ToString calls
                            value = 0,
                            nodeType = nodeType
                        };
                        node.SetChild(child);
                    }

                    node = node.GetChild(path);
                    node.Value.value += value;
                    _trieMap.Add(node.Value.key, node.Value);

                    //notify ui
                    BadgeMessaging.UpdateBadge(node.Value);
                }
            }
        }


        /// <summary>
        /// Update badge value by delta.
        /// If delta is negative, the value will not be less than 0.
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
            ReadOnlySpan<char> remainingKey = key.AsSpan();

            while (!remainingKey.IsEmpty)
            {
                int separatorIndex = remainingKey.IndexOf(Const.SEPARATOR);
                ReadOnlySpan<char> path;
                if (separatorIndex == -1) // No more separators, process the rest of the string
                {
                    path = remainingKey;
                    remainingKey = ReadOnlySpan<char>.Empty;
                }
                else
                {
                    path = remainingKey.Slice(0, separatorIndex);
                    remainingKey = remainingKey.Slice(separatorIndex + 1);
                }

                var child = node.GetChild(path.ToString());
                if (child.Value.nodeType == NodeType.Multiple)
                {
                    child.Value.value += delta;
                }
                else if (child.Value.nodeType == NodeType.Single)
                {
                    child.Value.value = delta > 0 ? 1 : 0;
                }

                node = child;
                // Notify UI
                BadgeMessaging.UpdateBadge(node.Value);
            }
        }

        /// <summary>
        /// Update badge and its children by delta. Only add delta to leaf nodes.
        /// </summary>
        /// <param name="keyPrefix">The prefix of the badge key.</param>
        /// <param name="delta">The amount to change the badge value by.</param>
        public void UpdateBadges(string keyPrefix, int delta)
        {
            var node = _trieMap.GetTrieNode(keyPrefix);

            if (node != null)
            {
                UpdateNodeAndChildren(node, delta);
                UpdateParents(keyPrefix);
            }
        }

        /// <summary>
        /// Updates the value of the badge and its children by a specified delta, only if the badge key ends with a specified postfix.
        /// </summary>
        /// <param name="keyPrefix">The prefix of the badge key.</param>
        /// <param name="postfix">The postfix to match at the end of the badge key.</param>
        /// <param name="delta">The amount to change the badge value by.</param>
        public void UpdateBadges(string keyPrefix, string postfix, int delta)
        {
            var node = _trieMap.GetTrieNode(keyPrefix);
            if (node == null) return;

            var children = node.GetChildren();
            foreach (var child in children)
            {
                UpdateNodeAndChildren(child, delta, trieNode => trieNode.Value.key.EndsWith(postfix));
                UpdateParents(child.Value.key);
            }
        }

        public void UpdateNodeAndChildren(TrieNode<BadgeData> node, int delta, Func<TrieNode<BadgeData>, bool> condition = null)
        {
            bool hasChildren = false;
            foreach (var child in node.GetChildren())
            {
                hasChildren = true;
                UpdateNodeAndChildren(child, delta, condition);
            }

            if (condition == null || condition(node))
            {
                node.Value.value += delta;
            }

            if (hasChildren)
            {
                int sum = 0;
                foreach (var child in node.GetChildren())
                {
                    if (child.Value.nodeType == NodeType.Multiple)
                    {
                        sum += child.Value.value;
                    }
                    else if (child.Value.nodeType == NodeType.Single)
                    {
                        sum += child.Value.value > 0 ? 1 : 0;
                    }
                }

                node.Value.value = sum;
            }

            BadgeMessaging.UpdateBadge(node.Value);
        }

        private void UpdateParents(string key)
        {
            ReadOnlySpan<char> keySpan = key.AsSpan();
            int lastIndex = keySpan.LastIndexOf(Const.SEPARATOR);
            if (lastIndex == -1) return;

            ReadOnlySpan<char> parentKeySpan = keySpan.Slice(0, lastIndex);

            string parentKey = parentKeySpan.ToString();
            var parent = _trieMap.GetTrieNode(parentKey);
            if (parent != null)
            {
                int sum = 0;
                foreach (var child in parent.GetChildren())
                {
                    if (child.Value.nodeType == NodeType.Multiple)
                    {
                        sum += child.Value.value;
                    }
                    else if (child.Value.nodeType == NodeType.Single)
                    {
                        sum += child.Value.value > 0 ? 1 : 0;
                    }
                }

                parent.Value.value = sum;

                BadgeMessaging.UpdateBadge(parent.Value);

                // Recursively update the parents of the current node
                // Convert the parent key span back to a string for recursive call
                UpdateParents(parentKey);
            }
        }

        /// <summary>
        /// Set badge value by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="Exception"></exception>
        public void SetBadgeValue(string key, int value)
        {
            var node = _trieMap.GetTrieNode(key);
            if (node == null)
            {
                throw new System.Exception($"Badge {key} not found");
            }

            node.Value.value = value;
            BadgeMessaging.UpdateBadge(node.Value);

            UpdateParents(key);
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