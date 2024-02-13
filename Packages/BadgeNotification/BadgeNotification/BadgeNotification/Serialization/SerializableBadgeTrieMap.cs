using System.Collections.Generic;

namespace Voidex.Badge.Runtime.Serialization
{
    [System.Serializable]
    public class SerializableBadgeTrieMap<TValue> where TValue : struct
    {
        public List<BadgeData<TValue>> nodes;
        
        public SerializableBadgeTrieMap()
        {
            nodes = new List<BadgeData<TValue>>();
        }
    }
}