using System;
using Newtonsoft.Json;
using Voidex.Trie;

namespace Voidex.Badge.Runtime.Serialization
{
    public static class BadgeNotificationConverter
    {
        public static string Serialize<TBadgeNotification, TValue>(TBadgeNotification trieMap, Predicate<TrieNode<BadgeData<TValue>>> condition) where TBadgeNotification : BadgeNotificationBase<TValue> where TValue : struct
        {
            var serializableTrieMap = ConvertToSerializable(trieMap, condition);
            return JsonConvert.SerializeObject(serializableTrieMap);
        }

        public static TBadgeNotification Deserialize<TBadgeNotification, TValue>(string json) where TBadgeNotification : BadgeNotificationBase<TValue>, new() where TValue : struct
        {
            var serializableTrieMap = JsonConvert.DeserializeObject<SerializableBadgeTrieMap<TValue>>(json);
            return ConvertToTrieMap<TBadgeNotification, TValue>(serializableTrieMap);
        }
        
        public static T ConvertToTrieMap<T, TValue>(SerializableBadgeTrieMap<TValue> serializableBadgeTrieMap) where T : BadgeNotificationBase<TValue>, new() where TValue : struct
        {
            T trieMap = new T();
            foreach (var node in serializableBadgeTrieMap.nodes)
            {
                trieMap.AddBadge(node);
            }
            return trieMap;
        }
        
        private static SerializableBadgeTrieMap<TValue> ConvertToSerializable<TValue>(BadgeNotificationBase<TValue> trieMap, Predicate<TrieNode<BadgeData<TValue>>> condition) where TValue : struct
        {
            SerializableBadgeTrieMap<TValue> serializableBadgeTrieMap = new SerializableBadgeTrieMap<TValue>();
            var values = trieMap._trieMap.TrieNodes();
            foreach (var value in values)
            {
                if(condition!=null && !condition(value)) continue;
                serializableBadgeTrieMap.nodes.Add(value);
            }
            
            return serializableBadgeTrieMap;
        }
        
    }
}