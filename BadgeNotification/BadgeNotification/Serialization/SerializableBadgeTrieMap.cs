using System.Collections.Generic;
using Voidex.Trie;

namespace Voidex.Badge.Runtime.Serialization
{
    [System.Serializable]
    public class SerializableBadgeTrieMap<TValue> where TValue : struct
    {
        public List<TrieNode<BadgeData<TValue>>> nodes;
        
        public SerializableBadgeTrieMap()
        {
            nodes = new List<TrieNode<BadgeData<TValue>>>();
        }
    }
}