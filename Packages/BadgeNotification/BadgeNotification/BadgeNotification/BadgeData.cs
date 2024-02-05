namespace Voidex.Badge.Runtime
{
    public class BadgeData
    {
        public string key;
        public int value;
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