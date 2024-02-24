using System;

namespace Voidex.Badge.Runtime
{
    public partial struct BadgeChangedMessage<T> : IEquatable<BadgeChangedMessage<T>> where T : struct
    {
        public string key;
        public T value;
        public int badgeCount;

        public bool Equals(BadgeChangedMessage<T> other)
        {
            return key == other.key && value.Equals(other.value) && badgeCount == other.badgeCount;
        }

        public override bool Equals(object obj)
        {
            return obj is BadgeChangedMessage<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(key, value, badgeCount);
        }
    }
}