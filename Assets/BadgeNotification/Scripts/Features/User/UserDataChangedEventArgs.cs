using System;

namespace Voidex.Badge.Sample.Features.User
{
    public class UserDataChangedEventArgs : EventArgs
    {
        public int OldValue { get; set; }
        public int NewValue { get; set; }
    }
}