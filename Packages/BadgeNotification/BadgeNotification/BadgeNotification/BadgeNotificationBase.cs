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
        public IEnumerable<TrieNode<BadgeData<TValue>>> GetBadges()
        {
            return _trieMap.TrieNodes();
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

#if USE_XNODE
        protected BadgeNotificationBase(BadgeGraph badgeGraph)
        {
            _trieMap = new TrieMap<BadgeData<TValue>>();
            SetDefaultNodeData(Const.ROOT);

            foreach (var node in badgeGraph.nodes)
            {
                var key = node.GetValue(null).ToString();
                if (node is BadgeNode badgeNode)
                {
                    var value = new BadgeData<TValue>
                    {
                        badgeCount = 0,
                        value = default,
                        nodeType = badgeNode.nodeType
                    };
                    _trieMap.Add(key, value);
                    BadgeMessaging<TValue>.UpdateBadge(key, value);
                }
                else
                {
                    //case of Root node in the graph
                    var value = new BadgeData<TValue>
                    {
                        badgeCount = 0,
                        value = default,
                        nodeType = NodeType.Multiple
                    };
                    _trieMap.Add(key, value);
                    BadgeMessaging<TValue>.UpdateBadge(key, value);
                }
            }
        }
#endif

        protected BadgeNotificationBase()
        {
            _trieMap = new TrieMap<BadgeData<TValue>>();
        }

        /// <summary>
        /// Set data for a node by key in the trie, where badgeCount is 0 and value is default.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        public bool SetDefaultNodeData(string key, NodeType nodeType = NodeType.Multiple)
        {
            var node = _trieMap.GetTrieNode(key);
            if (node.Value == null)
            {
                var value = new BadgeData<TValue>
                {
                    badgeCount = 0,
                    value = default,
                    nodeType = nodeType
                };
                node.Value = value;
                BadgeMessaging<TValue>.UpdateBadge(key, value);
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

                    string word = pathSpan.ToString(); // Convert span to string for Trie operations
                    if (!node.HasChild(word))
                    {
                        var path = fullPathBuilder.ToString(); // Minimize ToString calls
                        var child = new TrieNode<BadgeData<TValue>>(word, node);
                        child.Value = new BadgeData<TValue>
                        {
                            value = default, badgeCount = 0,
                            nodeType = nodeType
                        };
                        child.Path = path;
                        node.SetChild(child);
                    }

                    node = node.GetChild(word);
                    node.Value.badgeCount += value;
                    _trieMap.Add(node.Path, node.Value);

                    //notify ui
                    BadgeMessaging<TValue>.UpdateBadge(key, node.Value);
                }
            }
        }

        /// <summary>
        /// Add badge by BadgeData
        /// </summary>
        /// <param name="badgeData"></param>
        public void AddBadge(TrieNode<BadgeData<TValue>> badgeData)
        {
            if (_trieMap.HasKey(badgeData.Path)) return;

            var node = _trieMap.GetRootTrieNode();
            if (node.Value != null)
                node.Value.badgeCount += badgeData.Value.badgeCount;

            ReadOnlySpan<char> keySpan = badgeData.Path.AsSpan();
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

                    string word = pathSpan.ToString(); // Convert span to string for Trie operations
                    string path = fullPathBuilder.ToString(); // Minimize ToString calls

                    if (!node.HasChild(word))
                    {
                        var child = new TrieNode<BadgeData<TValue>>(word, node)
                        {
                            Value = new BadgeData<TValue>
                            {
                                value = badgeData.Value.value, badgeCount = 0,
                                nodeType = badgeData.Value.nodeType
                            },
                            Path = path
                        };
                        node.SetChild(child);
                    }

                    node = node.GetChild(word);
                    node.Value.badgeCount += badgeData.Value.badgeCount;
                    _trieMap.Add(node.Path, node.Value);

                    //notify ui
                    BadgeMessaging<TValue>.UpdateBadge(path, node.Value);
                }
            }
        }


        /// <summary>
        /// Update badge value by delta.
        /// If delta is negative, the value will not be less than 0.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="delta"></param>
        public void UpdateBadgeCount(string key, int delta)
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
            BadgeMessaging<TValue>.UpdateBadge(key, targetNode.Value);

            var node = targetNode.Parent;

            while (node != null)
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

                //stop if the node is root
                if (node.Value == null)
                {
                    break;
                }

                node.Value.badgeCount = sum;
                BadgeMessaging<TValue>.UpdateBadge(node.Path, node.Value);

                node = node.Parent;
            }
        }

        /// <summary>
        /// Update badge and its children by delta. Only add delta to leaf nodes.
        /// </summary>
        /// <param name="keyPrefix">The prefix of the badge key.</param>
        /// <param name="delta">The amount to change the badge value by.</param>
        public void UpdateBadgesCount(string keyPrefix, int delta)
        {
            var node = _trieMap.GetTrieNode(keyPrefix);

            if (node != null)
            {
                UpdateNodeAndChildren(node, delta);
                UpdateParents(node);
            }
        }

        /// <summary>
        /// Updates the value of the badge and its children by a specified delta, only if the badge key ends with a specified postfix.
        /// </summary>
        /// <param name="keyPrefix">The prefix of the badge key.</param>
        /// <param name="postfix">The postfix to match at the end of the badge key.</param>
        /// <param name="delta">The amount to change the badge value by.</param>
        public void UpdateBadgesCount(string keyPrefix, string postfix, int delta)
        {
            var node = _trieMap.GetTrieNode(keyPrefix);
            if (node == null) return;

            var children = node.GetChildren();
            foreach (var child in children)
            {
                UpdateNodeAndChildren(child, delta, trieNode => trieNode.Path.EndsWith(postfix));
                UpdateParents(child);
            }
        }

        /// <summary>
        /// Updates the badge count of the badges by a specified delta, set the custom value, only if the badge key ends with a specified postfix.
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <param name="postfix"></param>
        /// <param name="delta"></param>
        /// <param name="value"></param>
        public void UpdateBadgesCount(string keyPrefix, string postfix, int delta, TValue value)
        {
            var node = _trieMap.GetTrieNode(keyPrefix);
            if (node == null) return;

            var children = node.GetChildren();
            foreach (var child in children)
            {
                UpdateNodeAndChildren(child, delta, value, trieNode => trieNode.Path.EndsWith(postfix));
                //TODO do not update parents if the node is not updated to avoid unnecessary calculation
                UpdateParents(child);
            }
        }

        /// <summary>
        /// Updates the value of the badge and its children by a specified delta, only if the condition is met.
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <param name="delta"></param>
        /// <param name="condition"></param>
        public void UpdateBadgesCount(string keyPrefix, int delta, [NotNull] Func<TrieNode<BadgeData<TValue>>, bool> condition)
        {
            var node = _trieMap.GetTrieNode(keyPrefix);
            if (node == null) return;

            var children = node.GetChildren();
            foreach (var child in children)
            {
                UpdateNodeAndChildren(child, delta, condition);
                UpdateParents(child);
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

            BadgeMessaging<TValue>.UpdateBadge(node.Path, node.Value);
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

            BadgeMessaging<TValue>.UpdateBadge(node.Path, node.Value);
        }

        private void UpdateParents(TrieNode<BadgeData<TValue>> node)
        {
            var parent = node.Parent;
            while (parent != null)
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

                //stop if the node is root
                if (parent.Value == null)
                {
                    break;
                }

                parent.Value.badgeCount = sum;
                BadgeMessaging<TValue>.UpdateBadge(parent.Path, parent.Value);

                parent = parent.Parent;
            }
        }

        private void UpdateLeafNode(TrieNode<BadgeData<TValue>> node, int delta)
        {
            node.Value.badgeCount += delta;
            BadgeMessaging<TValue>.UpdateBadge(node.Path, node.Value);
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
            BadgeMessaging<TValue>.UpdateBadge(node.Path, node.Value);

            UpdateParents(node);

            // var parent = node.Parent;
            // while (parent != null)
            // {
            //     int sum = 0;
            //     foreach (var child in parent.GetChildren())
            //     {
            //         if (child.Value.nodeType == NodeType.Multiple)
            //         {
            //             sum += child.Value.badgeCount;
            //         }
            //         else if (child.Value.nodeType == NodeType.Single)
            //         {
            //             sum += child.Value.badgeCount > 0 ? 1 : 0;
            //         }
            //     }
            //
            //     //stop if the node is root
            //     if (parent.Value == null)
            //     {
            //         break;
            //     }
            //
            //     parent.Value.badgeCount = sum;
            //     BadgeMessaging<TValue>.UpdateBadge(parent.Path, parent.Value);
            //     
            //     parent = parent.Parent;
            // }
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
            BadgeMessaging<TValue>.UpdateBadge(node.Path, node.Value);

            UpdateParents(node);
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
            BadgeMessaging<TValue>.UpdateBadge(node.Path, node.Value);
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

            BadgeMessaging<TValue>.UpdateBadge(node.Path, node.Value);
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

            BadgeMessaging<TValue>.UpdateBadge(node.Path, node.Value);
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
                SetNodeAndChildrenValue(child, count, value, trieNode => trieNode.Path.EndsWith(keyPostfix));
                UpdateParents(child);
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
                UpdateParents(child);
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
                UpdateParents(child);
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