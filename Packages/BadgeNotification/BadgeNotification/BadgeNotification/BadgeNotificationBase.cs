using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cysharp.Text;
using Voidex.Trie;

namespace Voidex.Badge.Runtime
{
    /// <summary>
    /// The badge system base class that provides the basic functionality for badge notification.
    /// </summary>
    public abstract class BadgeNotificationBase<TValue> where TValue : struct
    {
        internal TrieMap<BadgeData<TValue>> _trieMap;

        public BadgeData<TValue> GetBadge(string key)
        {
            if (_trieMap == null) throw new System.Exception("BadgeSystem not initialized");
            return _trieMap.ValueBy(key);
        }

        /// <summary>
        /// get all badges
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BadgeData<TValue>> GetBadges()
        {
            return _trieMap.Values();
        }

        public IEnumerable<BadgeData<TValue>> GetBadgesBy(string keyPrefix)
        {
            return _trieMap.ValuesBy(keyPrefix);
        }

        /// <summary>
        /// Get trie node by key prefix
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <returns></returns>
        public TrieNode<BadgeData<TValue>> GetTrieNode(string keyPrefix)
        {
            return _trieMap.GetTrieNode(keyPrefix);
        }

        protected BadgeNotificationBase(BadgeGraph badgeGraph)
        {
            _trieMap = new TrieMap<BadgeData<TValue>>();
            foreach (var node in badgeGraph.nodes)
            {
                var key = node.GetValue(null).ToString();
                if (node is BadgeNode badgeNode)
                {
                    var value = new BadgeData<TValue>
                    {
                        key = key, badgeCount = 0,
                        value = default,
                        nodeType = badgeNode.nodeType
                    };
                    _trieMap.Add(key, value);
                    BadgeMessaging<TValue>.UpdateBadge(value);
                }
                else
                {
                    var value = new BadgeData<TValue>
                    {
                        key = key,
                        value = default, badgeCount = 0
                    };
                    _trieMap.Add(key, value);
                    BadgeMessaging<TValue>.UpdateBadge(value);
                }
            }
        }

        protected BadgeNotificationBase()
        {
            _trieMap = new TrieMap<BadgeData<TValue>>();
        }
        
        /// <summary>
        /// Set value for a node by key in the trie.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        public bool SetNodeValue(string key, NodeType nodeType = NodeType.Multiple)
        {
            var node = _trieMap.GetTrieNode(key);
            if (node.Value == null)
            {
                var value = new BadgeData<TValue>
                {
                    key = key, badgeCount = 0,
                    value = default,
                    nodeType = nodeType
                };
                _trieMap.Add(key, value);
                BadgeMessaging<TValue>.UpdateBadge(value);
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Add badge by key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">badge number</param>
        /// <param name="nodeType"></param>
        public void AddBadge(string key, int value, NodeType nodeType = NodeType.Multiple)
        {
            if (_trieMap.HasKey(key)) return;

            var node = _trieMap.GetRootTrieNode();
            if (node.Value != null)
                node.Value.badgeCount += value;

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
                        var child = new TrieNode<BadgeData<TValue>>(path);
                        child.Value = new BadgeData<TValue>
                        {
                            key = fullPathBuilder.ToString(), // Minimize ToString calls
                            value = default, badgeCount = 0,
                            nodeType = nodeType
                        };
                        node.SetChild(child);
                    }

                    node = node.GetChild(path);
                    node.Value.badgeCount += value;
                    _trieMap.Add(node.Value.key, node.Value);

                    //notify ui
                    BadgeMessaging<TValue>.UpdateBadge(node.Value);
                }
            }
        }

        /// <summary>
        /// Add badge by BadgeData
        /// </summary>
        /// <param name="badgeData"></param>
        public void AddBadge(BadgeData<TValue> badgeData)
        {
            if (_trieMap.HasKey(badgeData.key)) return;

            var node = _trieMap.GetRootTrieNode();
            if (node.Value != null)
                node.Value.badgeCount += badgeData.badgeCount;

            ReadOnlySpan<char> keySpan = badgeData.key.AsSpan();
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
                        var child = new TrieNode<BadgeData<TValue>>(path);
                        child.Value = new BadgeData<TValue>
                        {
                            key = fullPathBuilder.ToString(), // Minimize ToString calls
                            value = badgeData.value, badgeCount = 0,
                            nodeType = badgeData.nodeType
                        };
                        node.SetChild(child);
                    }

                    node = node.GetChild(path);
                    node.Value.badgeCount += badgeData.badgeCount;
                    _trieMap.Add(node.Value.key, node.Value);

                    //notify ui
                    BadgeMessaging<TValue>.UpdateBadge(node.Value);
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

            if (delta < 0 && targetNode.Value.badgeCount + delta < 0)
            {
                delta = -targetNode.Value.badgeCount;
            }

            targetNode.Value.badgeCount += delta;
            UpdateParents(key);
            BadgeMessaging<TValue>.UpdateBadge(targetNode.Value);
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

        /// <summary>
        /// Updates the badge count of the badges by a specified delta, set the custom value, only if the badge key ends with a specified postfix.
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <param name="postfix"></param>
        /// <param name="delta"></param>
        /// <param name="value"></param>
        public void UpdateBadges(string keyPrefix, string postfix, int delta, TValue value)
        {
            var node = _trieMap.GetTrieNode(keyPrefix);
            if (node == null) return;

            var children = node.GetChildren();
            foreach (var child in children)
            {
                UpdateNodeAndChildren(child, delta, value, trieNode => trieNode.Value.key.EndsWith(postfix));
                //TODO do not update parents if the node is not updated to avoid unnecessary calculation
                UpdateParents(child.Value.key);
            }
        }

        /// <summary>
        /// Updates the value of the badge and its children by a specified delta, only if the condition is met.
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <param name="delta"></param>
        /// <param name="condition"></param>
        public void UpdateBadges(string keyPrefix, int delta, [NotNull] Func<TrieNode<BadgeData<TValue>>, bool> condition)
        {
            var node = _trieMap.GetTrieNode(keyPrefix);
            if (node == null) return;

            var children = node.GetChildren();
            foreach (var child in children)
            {
                UpdateNodeAndChildren(child, delta, condition);
                UpdateParents(child.Value.key);
            }
        }

        private void UpdateNodeAndChildren(TrieNode<BadgeData<TValue>> node, int delta, Func<TrieNode<BadgeData<TValue>>, bool> condition = null)
        {
            foreach (var child in node.GetChildren())
            {
                UpdateNodeAndChildren(child, delta, condition);
            }

            if (condition == null || condition(node))
            {
                node.Value.badgeCount += delta;
            }
            
            var hasChildren = node.HasChildren();
            if (hasChildren)
            {
                int sum = 0;
                foreach (var child in node.GetChildren())
                {
                    if (child.Value.nodeType == NodeType.Multiple)
                    {
                        sum += child.Value.badgeCount;
                    }
                    else if (child.Value.nodeType == NodeType.Single)
                    {
                        sum += child.Value.badgeCount > 0 ? 1 : 0;
                    }
                }

                node.Value.badgeCount = sum;
            }

            BadgeMessaging<TValue>.UpdateBadge(node.Value);
        }

        private void UpdateNodeAndChildren(TrieNode<BadgeData<TValue>> node, int delta, TValue value, Func<TrieNode<BadgeData<TValue>>, bool> condition = null)
        {
            foreach (var child in node.GetChildren())
            {
                UpdateNodeAndChildren(child, delta, value, condition);
            }

            if (condition == null || condition(node))
            {
                node.Value.badgeCount += delta;
                node.Value.value = value;
            }

            var hasChildren = node.HasChildren();
            if (hasChildren)
            {
                int sum = 0;
                foreach (var child in node.GetChildren())
                {
                    if (child.Value.nodeType == NodeType.Multiple)
                    {
                        sum += child.Value.badgeCount;
                    }
                    else if (child.Value.nodeType == NodeType.Single)
                    {
                        sum += child.Value.badgeCount > 0 ? 1 : 0;
                    }
                }

                node.Value.badgeCount = sum;
            }

            BadgeMessaging<TValue>.UpdateBadge(node.Value);
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
                        sum += child.Value.badgeCount;
                    }
                    else if (child.Value.nodeType == NodeType.Single)
                    {
                        sum += child.Value.badgeCount > 0 ? 1 : 0;
                    }
                }
                
                parent.Value.badgeCount = sum;

                BadgeMessaging<TValue>.UpdateBadge(parent.Value);

                // Recursively update the parents of the current node
                // Convert the parent key span back to a string for recursive call
                UpdateParents(parentKey);
            }
        }

        /// <summary>
        /// Set badge value by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="badgeCount"></param>
        /// <exception cref="Exception"></exception>
        public virtual void SetBadgeCount(string key, int badgeCount)
        {
            var node = _trieMap.GetTrieNode(key);
            if (node == null)
            {
                throw new System.Exception($"Badge {key} not found");
            }

            node.Value.badgeCount = badgeCount;
            BadgeMessaging<TValue>.UpdateBadge(node.Value);

            UpdateParents(key);
        }

        /// <summary>
        /// Set badge count and badge custom value by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="badgeCount"></param>
        /// <param name="value"></param>
        /// <exception cref="Exception"></exception>
        public virtual void SetBadgeValue(string key, int badgeCount, TValue value)
        {
            var node = _trieMap.GetTrieNode(key);
            if (node == null)
            {
                throw new System.Exception($"Badge {key} not found");
            }

            node.Value.badgeCount = badgeCount;
            node.Value.value = value;
            BadgeMessaging<TValue>.UpdateBadge(node.Value);

            UpdateParents(key);
        }
        
        /// <summary>
        /// Set badge custom value by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="Exception"></exception>
        public void SetBadgeValue(string key, TValue value)
        {
            var node = _trieMap.GetTrieNode(key);
            if (node == null)
            {
                throw new System.Exception($"Badge {key} not found");
            }
            
            node.Value.value = value;
            BadgeMessaging<TValue>.UpdateBadge(node.Value);
        }

        private void SetNodeAndChildrenValue(TrieNode<BadgeData<TValue>> node, int count, TValue value, Func<TrieNode<BadgeData<TValue>>, bool> condition = null)
        {
            foreach (var child in node.GetChildren())
            {
                SetNodeAndChildrenValue(child, count, value, condition);
            }

            if (condition == null || condition(node))
            {
                node.Value.badgeCount = count;
                node.Value.value = value;
            }
            
            var hasChildren = node.HasChildren();
            if (hasChildren)
            {
                int sum = 0;
                foreach (var child in node.GetChildren())
                {
                    if (child.Value.nodeType == NodeType.Multiple)
                    {
                        sum += child.Value.badgeCount;
                    }
                    else if (child.Value.nodeType == NodeType.Single)
                    {
                        sum += child.Value.badgeCount > 0 ? 1 : 0;
                    }
                }

                node.Value.badgeCount = sum;
            }

            BadgeMessaging<TValue>.UpdateBadge(node.Value);
        }
        
        private void SetNodeAndChildrenValue(TrieNode<BadgeData<TValue>> node, Action<BadgeData<TValue>> modify, Func<TrieNode<BadgeData<TValue>>, bool> condition = null)
        {
            foreach (var child in node.GetChildren())
            {
                SetNodeAndChildrenValue(child, modify, condition);
            }
            
            if (condition == null || condition(node))
            {
                modify(node.Value);
            }

            var hasChildren = node.HasChildren();
            if (hasChildren)
            {
                int sum = 0;
                foreach (var child in node.GetChildren())
                {
                    if (child.Value.nodeType == NodeType.Multiple)
                    {
                        sum += child.Value.badgeCount;
                    }
                    else if (child.Value.nodeType == NodeType.Single)
                    {
                        sum += child.Value.badgeCount > 0 ? 1 : 0;
                    }
                }

                node.Value.badgeCount = sum;
            }

            BadgeMessaging<TValue>.UpdateBadge(node.Value);
        }

        /// <summary>
        /// Set badge value by key prefix
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <param name="keyPostfix"></param>
        /// <param name="count"></param>
        /// <param name="value"></param>
        public void SetBadgesValue(string keyPrefix, string keyPostfix, int count, TValue value)
        {
            var node = _trieMap.GetTrieNode(keyPrefix);
            if (node == null) return;

            var children = node.GetChildren();
            foreach (var child in children)
            {
                SetNodeAndChildrenValue(child, count, value,trieNode => trieNode.Value.key.EndsWith(keyPostfix));
                UpdateParents(child.Value.key);
            }
        }

        /// <summary>
        /// Sets the value for badges using the specified key prefix, value, and condition.
        /// </summary>
        /// <param name="keyPrefix">The prefix of the badge key.</param>
        /// <param name="count">The value to set the badge to.</param>
        /// <param name="value"></param>
        /// <param name="condition">The condition to be met for the badge to be updated.</param>
        public void SetBadgesValue(string keyPrefix, int count, TValue value, [NotNull] Func<TrieNode<BadgeData<TValue>>, bool> condition)
        {
            var node = _trieMap.GetTrieNode(keyPrefix);
            if (node == null) return;

            var children = node.GetChildren();
            foreach (var child in children)
            {
                SetNodeAndChildrenValue(child, count, value, condition);
                UpdateParents(child.Value.key);
            }
        }
        
        /// <summary>
        /// Sets the value for all the badges that have the specified key prefix, using the specified modify action to update the badge value, and the condition to be met for the badge to be updated.
        /// </summary>
        /// <param name="keyPrefix">The prefix of the badge key.</param>
        /// <param name="modify">The action to update the badge value.</param>
        /// <param name="condition">The condition to be met for the badge to be updated.</param>
        public void SetBadgesValue(string keyPrefix, Action<BadgeData<TValue>> modify, [NotNull] Func<TrieNode<BadgeData<TValue>>, bool> condition)
        {
            var node = _trieMap.GetTrieNode(keyPrefix);
            if (node == null) return;

            var children = node.GetChildren();
            foreach (var child in children)
            {
                SetNodeAndChildrenValue(child, modify, condition);
                UpdateParents(child.Value.key);
            }
        }

        /// <summary>
        /// Get badge value by key
        /// </summary>
        /// <param name="key">The key of the badge.</param>
        /// <returns></returns>
        public int GetBadgeCount(string key)
        {
            var node = _trieMap.GetTrieNode(key);
            return node == null ? 0 : node.Value.badgeCount;
        }

        /// <summary>
        /// Get badge value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue GetBadgeValue(string key)
        {
            var node = _trieMap.GetTrieNode(key);
            return node == null ? default : node.Value.value;
        }

        /// <summary>
        /// Get badge data by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public BadgeData<TValue> GetBadgeData(string key)
        {
            return _trieMap.ValueBy(key);
        }
    }
}