using System;
using System.Collections.Generic;
using MessagePipe;

namespace Voidex.Badge.Sample
{
    public class ChangedValueFilter<T> : MessageHandlerFilter<T>
    {
        T _lastValue;

        public override void Handle(T message, Action<T> next)
        {
            if (EqualityComparer<T>.Default.Equals(message, _lastValue))
            {
                return;
            }

            _lastValue = message;
            next(message);
        }
    }
}