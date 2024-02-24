using System;
using System.Collections.Generic;

namespace Voidex.Trie
{
    /// <summary>
    /// TrieNodeBase is an internal abstract class to encapsulate recursive, helper etc. methods.
    /// </summary>
    public abstract class TrieNodeBase : IEquatable<TrieNodeBase>
    {
        #region data members

        /// <summary>
        /// The word for the TrieNode.
        /// </summary>
        public string Word { get; }

        public string Path { get; set; }
        
        public TrieNodeBase Parent { get;}

        /// <summary>
        /// Children word->TrieNode map.
        /// </summary>
        private readonly IDictionary<string, TrieNodeBase> _children;

        #endregion

        #region ctors

        /// <summary>
        /// Creates a new TrieNode instance.
        /// </summary>
        /// <param name="word">The character for the TrieNode.</param>
        internal TrieNodeBase(string word)
        {
            Word = word;
            _children = new Dictionary<string, TrieNodeBase>();
        }

        internal TrieNodeBase(string word, TrieNodeBase parent)
        {
            Word = word;
            Parent = parent;
            _children = new Dictionary<string, TrieNodeBase>();
        }

        #endregion

        #region methods

        internal IEnumerable<TrieNodeBase> GetChildrenInner()
        {
            return _children.Values;
        }

        internal TrieNodeBase GetChildInner(string word)
        {
            _children.TryGetValue(word, out var trieNode);
            return trieNode;
        }

        // internal TrieNodeBase GetTrieNodeInner(string prefix)
        // {
        //     TrieNodeBase trieNode = this;
        //     ReadOnlySpan<char> remaining = prefix.AsSpan();
        //
        //     while (!remaining.IsEmpty)
        //     {
        //         int separatorIndex = remaining.IndexOf(Const.SEPARATOR);
        //         ReadOnlySpan<char> word;
        //
        //         if (separatorIndex == -1)
        //         {
        //             word = remaining;
        //             remaining = ReadOnlySpan<char>.Empty;
        //         }
        //         else
        //         {
        //             word = remaining.Slice(0, separatorIndex);
        //             remaining = remaining.Slice(separatorIndex + 1);
        //         }
        //
        //         if (word.IsEmpty || word.IsWhiteSpace())
        //         {
        //             continue;
        //         }
        //
        //         trieNode = trieNode.GetChildInner(word.ToString());
        //         if (trieNode == null)
        //         {
        //             break;
        //         }
        //     }
        //
        //     // if (trieNode == null && Path.Equals(prefix))
        //     // {
        //     //     return this;
        //     // }
        //
        //     return trieNode;
        // }

        internal TrieNodeBase GetTrieNodeInner(string prefix)
        {
            TrieNodeBase trieNode = this;
            int start = 0;
            int end = 0;
        
            while (end <= prefix.Length)
            {
                if (end == prefix.Length || prefix[end] == Const.SEPARATOR)
                {
                    if (end != start)
                    {
                        string word = prefix.Substring(start, end - start);
                        trieNode = trieNode.GetChildInner(word);
                        if (trieNode == null)
                        {
                            break;
                        }
                    }
                    start = end + 1;
                }
                end++;
            }
        
            return trieNode;
        }

        public void SetChild(TrieNodeBase child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }

            _children[child.Word] = child;
        }

        public void RemoveChild(string word)
        {
            _children.Remove(word);
        }

        internal virtual void Clear()
        {
            _children.Clear();
        }

        #region IEquatable<TrieNodeBase>

        public bool Equals(TrieNodeBase other)
        {
            return Word == other.Word;
        }

        public bool HasChildren()
        {
            return _children.Count > 0;
        }

        #endregion

        #endregion
    }
}