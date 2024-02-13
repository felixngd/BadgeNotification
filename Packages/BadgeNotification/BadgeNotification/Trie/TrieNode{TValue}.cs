using System.Collections.Generic;
using System.Linq;

namespace Voidex.Trie
{
    /// <summary>
    /// TrieNode[TValue] node to save TValue item.
    /// </summary>
    /// <typeparam name="TValue">Type of Value at each TrieNode.</typeparam>
    public class TrieNode<TValue> : TrieNodeBase
    {
        #region data members

        /// <summary>
        /// TValue item.
        /// </summary>
        public TValue Value
        {
            get { return _value; }
            set
            {
                _value = value;
                hasValue = value != null;
            }
        }

        private bool hasValue;
        private TValue _value;

        #endregion

        #region ctors

        /// <summary>
        /// Creates a new TrieNode instance.
        /// </summary>
        public TrieNode(string word)
            : base(word)
        {
        }

        #endregion

        /// <summary>
        /// Returns true if contains value.
        /// </summary>
        public bool HasValue()
        {
            return hasValue;
        }

        #region TrieNodeBase

        internal override void Clear()
        {
            base.Clear();
            Value = default(TValue);
            hasValue = false;
        }

        public TrieNode<TValue> GetChild(string word)
        {
            return base.GetChildInner(word) as TrieNode<TValue>;
        }

        public bool HasChild(string word)
        {
            return GetChild(word) != null;
        }

        public TrieNode<TValue> GetTrieNode(string prefix)
        {
            return base.GetTrieNodeInner(prefix) as TrieNode<TValue>;
        }

        public IEnumerable<TrieNode<TValue>> GetChildren()
        {
            return base.GetChildrenInner().Cast<TrieNode<TValue>>();
        }
        
        #endregion
    }
}