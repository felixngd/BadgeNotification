namespace Voidex.Badge.Runtime
{
    /// <summary>
    /// BadgeData is a struct that holds the data for a badge in the trie.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class BadgeData<T> where T : struct
    {
        //public string key;
        public int badgeCount;
        public T value;
        public NodeType nodeType;
    }

    public enum NodeType
    {
        /// <summary>
        /// 1 add up to the parent node.
        /// </summary>
        Single,
        /// <summary>
        /// this node's parent is incremented by the value of this node.
        /// </summary>
        Multiple
    }
}