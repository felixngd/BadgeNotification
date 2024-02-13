namespace Voidex.Badge.Runtime
{
    public partial struct BadgeChangedMessage<T>
    {
        public string key;
        public T value;
        public int badgeCount;
    }
}