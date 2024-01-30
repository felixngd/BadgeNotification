using System;

namespace Voidex.Badge.Runtime.Interfaces
{
    /// <summary>
    /// Interface for a pub/sub system. You can use any pub/sub system you want, as long as it implements this interface.
    /// </summary>
    /// <example>
    /// Here is an example of a pub/sub system using the <a href="package:com.cysharp.messagepipe">MessagePipe</a> package.
    /// <code>
    /// using MessagePipe;
    /// using System;
    /// using Voidex.RedDot.Runtime.Interfaces;
    /// public class MessagePipePubSub : IPubSub&lt;MyMessage&gt;
    /// {
    ///     public void Publish(MyMessage topic)
    ///     {
    ///         var p = GlobalMessagePipe.GetPublisher&lt;MyMessage&gt;();
    ///         p.Publish(topic);
    ///     }
    ///
    ///     public void Subscribe(MyMessage topic, Action&lt;MyMessage&gt; callback)
    ///     {
    ///         var s = GlobalMessagePipe.GetSubscriber&lt;MyMessage&gt;();
    ///         s.Subscribe(topic, callback);
    ///     }
    /// }
    ///
    /// public struct MyMessage{}
    /// </code>
    /// </example>
    public interface IPubSub<TMessage>
    {
        void Publish(TMessage topic);
        IDisposable Subscribe(string topic, Action<TMessage> callback);
    }
}