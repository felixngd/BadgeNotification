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
        public string Word { get; private set; }

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

        internal TrieNodeBase GetTrieNodeInner(string prefix)
        {
            TrieNodeBase trieNode = this;
            ReadOnlySpan<char> remaining = prefix.AsSpan();
            
            while (!remaining.IsEmpty)
            {
                int separatorIndex = remaining.IndexOf(Const.SEPARATOR);
                ReadOnlySpan<char> word;
            
                if (separatorIndex == -1)
                {
                    word = remaining;
                    remaining = ReadOnlySpan<char>.Empty;
                }
                else
                {
                    word = remaining.Slice(0, separatorIndex);
                    remaining = remaining.Slice(separatorIndex + 1);
                }
            
                if (word.IsEmpty || word.IsWhiteSpace())
                {
                    continue;
                }
                
                trieNode = trieNode.GetChildInner(word.ToString());
                if (trieNode == null)
                {
                    break;
                }
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